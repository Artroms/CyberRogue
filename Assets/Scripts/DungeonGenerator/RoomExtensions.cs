using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public static class RoomExtensions
{

    private const int timeout = 1000;
    private const float stepSize = 1;

    public static Room[] MoveRooms(this Room[] rooms)
    {
        RectRoom[] rects = ToRects(rooms);
        rects.AddDelta();
        Move(rects);
        return rooms;
    }

    public static Room[] MoveRooms(this Room[] rooms, int sizeAdd)
    {
        RectRoom[] rects = ToRects(rooms, sizeAdd);
        rects.AddDelta();
        Move(rects);
        return rooms;
    }

    public static Room[] GetAllWithMeta<T>(this Room[] rooms, T type)
    {
        List<Room> meta = new List<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].metaData.Get<T>().Equals(type))
                meta.Add(rooms[i]);
        }
        return meta.ToArray();
    }

    public static void AddMetaData<T>(this Room[] rooms, T type)
    {
        rooms.RemoveMetaData<T>();
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].metaData.Add(type);
        }
    }

    public static Room[] BSP(this Room[] rooms)
    {
        List<Room> newRooms = new List<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            newRooms.AddRange(rooms[i].BSP());
        }
        return newRooms.ToArray();
    }

    public static Room[] Floors(this Room[] rooms)
    {
        List<Room> newRooms = new List<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            newRooms.AddRange(rooms[i].Floors());
        }
        return newRooms.ToArray();
    }

    public static Room[] Resize(this Room[] rooms, int addWidth, int addHeight, int addLength)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].Resize(addWidth, addHeight, addLength);
        }
        return rooms;
    }

    public static Room[] ShiftSize(this Room[] rooms, int shiftWidth, int shiftHeight, int shiftLength)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].ShiftSize(shiftWidth, shiftHeight, shiftLength);
        }
        return rooms;
    }

    public static Room[] ShiftSize(this Room[] rooms, Func<int> shiftWidth, Func<int> shiftHeight, Func<int> shiftLength)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].ShiftSize(shiftWidth(), shiftHeight(), shiftLength());
        }
        return rooms;
    }

    public static Vector3Int Min(this Room[] rooms)
    {
        Vector3Int min = Vector3Int.one * int.MaxValue;
        for (int i = 0; i < rooms.Length; i++)
        {
            min.x = rooms[i].x < min.x ? rooms[i].x : min.x;
            min.y = rooms[i].y < min.y ? rooms[i].y : min.y;
            min.z = rooms[i].z < min.z ? rooms[i].z : min.z;
        }
        return min;
    }

    public static Vector3Int Max(this Room[] rooms)
    {
        Vector3Int max = Vector3Int.one * int.MinValue;
        for (int i = 0; i < rooms.Length; i++)
        {
            max.x = rooms[i].x + rooms[i].width > max.x ? rooms[i].x + rooms[i].width : max.x;
            max.y = rooms[i].y + rooms[i].height > max.y ? rooms[i].y + rooms[i].height : max.y;
            max.z = rooms[i].z + rooms[i].length > max.z ? rooms[i].z + rooms[i].length : max.z;
        }
        return max;
    }

    private static List<Room> Connect(this Room left, Room right)
    {
        var halls = new List<Room>();
        if(left.width > left.length)
        {
            var tmp = left;
            left = right;
            right = tmp;
        }
        else if(right.length > right.width)
        {
            var tmp = left;
            left = right;
            right = tmp;
        }
        Vector3Int point1 = left.Center;
        Vector3Int point2 = right.Center;
        Vector3Int delta = point2 - point1;
        if (delta.x > 0)
        {
            halls.Add(new Room(new Vector3Int(point1.x, point1.y, point1.z), new Vector3Int(delta.x + 1, 1, 1)));
        }
        else
        {
            halls.Add(new Room(new Vector3Int(point1.x + delta.x, point1.y, point1.z), new Vector3Int(-delta.x + 1, 1, 1)));
        }
        if (delta.z > 0)
        {
            halls.Add(new Room(new Vector3Int(point2.x, point2.y, point2.z - delta.z), new Vector3Int(1, 1, delta.z + 1)));
        }
        else
        {
            halls.Add(new Room(new Vector3Int(point2.x, point2.y, point2.z), new Vector3Int(1, 1, -delta.z + 1)));
        }
        return halls;
    }

    public static Room[] GetHalls(this Room[] rooms)
    {
        List<Room> remaining = rooms.ToList();
        List<Room> connected = new List<Room>();
        foreach (var item in remaining)
        {
            item.metaData.Add(new List<Room>());
        }
        connected.Add(remaining[remaining.Count - 1]);
        remaining.RemoveAt(remaining.Count - 1);
        for (int i = 0; i < connected.Count && remaining.Count > 0; i++)
        {
            var min = float.MaxValue;
            int index = 0;
            for (int j = 0; j < remaining.Count; j++)
            {
                var delta = (remaining[j].Center - connected[i].Center).magnitude;
                if (delta < min)
                {
                    min = delta;
                    index = j;
                }
            }
            var meta = connected[i].metaData.Get<List<Room>>();
            meta.Add(remaining[index]);
            connected.Add(remaining[index]);
            if(UnityEngine.Random.value > 0.5f) remaining.RemoveAt(index);
        }
        List<Room> halls = new List<Room>();
        foreach (var item in connected)
        {
            var connectedWith = item.metaData.Get<List<Room>>();
            foreach (var connectedTo in connectedWith)
            {
                halls.AddRange(item.Connect(connectedTo));
            }
        }
        AddDoors();
        connected.ToArray().RemoveMetaData<List<Room>>();
        return halls.ToArray();

        void AddDoors()
        {
            Vector3Int min = rooms.Min();
            Vector3Int max = rooms.Max();
            Vector3Int size = max - min;
            bool[,,] matrix = new bool[size.x + 1, size.y + 1, size.z + 1];
            for (int j = 0; j < size.x; j++)
            {
                for (int k = 0; k < size.y; k++)
                {
                    for (int l = 0; l < size.z; l++)
                    {
                        matrix[j, k, l] = false;
                    }
                }
            }
            for (int i = 0; i < halls.Count; i++)
            {
                for (int j = 0; j < halls[i].width; j++)
                {
                    for (int k = 0; k < halls[i].height; k++)
                    {
                        for (int l = 0; l < halls[i].length; l++)
                        {
                            Vector3Int pos = new Vector3Int(halls[i].x + j - min.x, halls[i].y + k - min.y, halls[i].z + l - min.z);
                            matrix[pos.x, pos.y, pos.z] = true;
                        }
                    }
                }
            }
            for (int i = 0; i < rooms.Length; i++)
            {
                rooms[i].metaData.Add(new RoomOption.Doors());
                for (int j = 0; j < rooms[i].width; j++)
                {
                    for (int k = 0; k < rooms[i].height; k++)
                    {
                        for (int l = 0; l < rooms[i].length; l++)
                        {
                            if (j == 0 || j == rooms[i].width - 1 || l == 0 || l == rooms[i].length - 1)
                            {
                                Vector3Int pos = new Vector3Int(rooms[i].x + j - min.x, rooms[i].y + k - min.y, rooms[i].z + l - min.z);
                                if (matrix[pos.x, pos.y, pos.z] == true)
                                {
                                    rooms[i].metaData.Get<RoomOption.Doors>().list.Add(new Vector3Int(j, k + 10, l));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private static RectRoom[] Move(RectRoom[] rects)
    {
        int iter = 0;
        bool overlaps = false;
        do
        {
            overlaps = false;
            for (int i = 0; i < rects.Length; i++)
            {
                overlaps = GetOverlapDelta(rects, in rects[i], out Vector2 overlapDelta) ? true : overlaps;
                overlapDelta = overlapDelta.normalized;
                rects[i].center += overlapDelta * stepSize;
            }
            iter++;
        } while (overlaps == true && iter < timeout);
        Debug.Log("Move rooms iterations: " + iter);
        return rects;
    }

    private static RectRoom[] ToRects(Room[] rooms, int sizeAdd = 0)
    {
        RectRoom[] rects = new RectRoom[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector2 center = new Vector2(rooms[i].x, rooms[i].z);
            Vector2 size = new Vector2(rooms[i].width + 1 + sizeAdd, rooms[i].length + 1 + sizeAdd);
            rects[i] = new RectRoom(center, size, rooms[i]);
        }
        return rects;
    }

    private static bool GetOverlapDelta(RectRoom[] rects, in RectRoom rect, out Vector2 overlapDelta)
    {
        overlapDelta = Vector2.zero;
        bool overlaps = false;
        for (int j = 0; j < rects.Length; j++)
        {
            if (rect.room != rects[j].room && rect.Overlaps(rects[j]))
            {
                if (rect.room.metaData.Get<RoomOption.AffectedByOthers>() == RoomOption.AffectedByOthers.NotAffected
                && rects[j].room.metaData.Get<RoomOption.AffectedByOthers>() == RoomOption.AffectedByOthers.Simple)
                {

                }
                else
                {
                    Vector2 step = (rect.center - rects[j].center);
                    if (step == Vector2.zero)
                        step /= step.magnitude;
                    overlapDelta += step;
                    overlaps = true;
                }
            }
        }
        return overlaps;
    }

    private static void RemoveMetaData<T>(this Room[] rooms)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].metaData.Remove<T>();
        }
    }

    public static void Resize(this Room room, int addWidth, int addHeight, int addLength)
    {
        room.width += addWidth;
        room.height += addHeight;
        room.length += addLength;
        room.x -= addWidth / 2;
        room.y -= addHeight / 2;
        room.z -= addLength / 2;
        Mathf.Clamp(room.width, 0, float.MaxValue);
        Mathf.Clamp(room.height, 0, float.MaxValue);
        Mathf.Clamp(room.length, 0, float.MaxValue);
    }

    public static void ShiftSize(this Room room, int shiftWidth, int shiftHeight, int shiftLength)
    {
        room.width -= Math.Abs(shiftWidth);
        room.height -= Math.Abs(shiftHeight);
        room.length -= Math.Abs(shiftLength);
        if (shiftWidth > 0)
            room.x += shiftWidth;
        if (shiftHeight > 0)
            room.y += shiftHeight;
        if (shiftLength > 0)
            room.z += shiftLength;
    }

    private static void AddDelta(this RectRoom[] rects)
    {
        for (int i = 0; i < rects.Length; i++)
        {
            rects[i].AddDelta();
        }
    }

    private class RectRoom
    {
        Rect rect;
        public Room room;

        public Vector2 center
        {
            get => rect.center;
            set
            {
                rect.center = value;
                room.x = Mathf.RoundToInt(center.x - size.x / 2);
                room.z = Mathf.RoundToInt(center.y - size.y / 2);
            }
        }

        public Vector2 size
        {
            get => rect.size;
            set
            {
                rect.size = value;
            }
        }

        public bool Overlaps(RectRoom rectRoom)
        {
            return rect.Overlaps(rectRoom.rect);
        }

        public RectRoom(Vector2 center, Vector2 size, Room room)
        {
            this.room = room;
            rect = new Rect(Vector2.zero, size);
            this.size = size;
            this.center = center;
        }

        public void AddDelta()
        {
            center += UnityEngine.Random.insideUnitCircle.normalized;
        }
    }
}
