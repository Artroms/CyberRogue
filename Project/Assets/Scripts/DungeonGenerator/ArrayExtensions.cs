using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class ArrayExtensions
{

    private static int timeout = 100000;
    private static float stepSize = 1;

    public static void MoveRooms(this Room[] rooms)
    {
        RectRoom[] rects = ToRects(rooms);
        rects.AddDelta();
        rects = Move(rects);
    }

    public static void MoveRooms(this Room[] rooms, float customStepSize)
    {
        RectRoom[] rects = ToRects(rooms);
        rects.AddDelta();
        rects = Move(rects, customStepSize);
    }

    private static RectRoom[] Move(RectRoom[] rects)
    {
        int iter = 0;
        bool overlaps = false;
        do {
            overlaps = false;
            for (int i = 0; i < rects.Length; i++)
            {
                overlaps = GetOverlapDelta(rects, in rects[i], out Vector2 overlapDelta)? true: overlaps;
                overlapDelta = overlapDelta.normalized;
                rects[i].center += overlapDelta * stepSize;
            }
            iter++;
        } while (overlaps == true && iter < timeout);
        Debug.Log(iter);
        return rects;
    }

    private static RectRoom[] Move(RectRoom[] rects, float customStepSize)
    {
        int iter = 0;
        bool overlaps = false;
        do {
            overlaps = false;
            for (int i = 0; i < rects.Length; i++)
            {
                overlaps = GetOverlapDelta(rects, in rects[i], out Vector2 overlapDelta)? true: overlaps;
                overlapDelta = overlapDelta.normalized;
                rects[i].center += overlapDelta * customStepSize;
            }
            iter++;
        } while (overlaps == true && iter < timeout);
        Debug.Log(iter);
        return rects;
    }

    private static RectRoom[] ToRects(Room[] rooms)
    {
        RectRoom[] rects = new RectRoom[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector2 center = new Vector2(rooms[i].x, rooms[i].z);
            Vector2 size = new Vector2(rooms[i].width + 1, rooms[i].length + 1);
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
            if(rect.room != rects[j].room && rect.Overlaps(rects[j]))
            {
                if(rect.room.metaData.Get<RoomOption.AffectedByOthers>() == RoomOption.AffectedByOthers.NotAffected
                && rects[j].room.metaData.Get<RoomOption.AffectedByOthers>() == RoomOption.AffectedByOthers.Simple)
                {

                }
                else
                {
                    Vector2 step = (rect.center - rects[j].center);
                    if(step == Vector2.zero)
                    step /= step.magnitude;
                    overlapDelta += step;
                    overlaps = true;
                }
            }
        }
        return overlaps;
    }

    public static Room[] GetAllWithMeta<T>(this Room[] rooms, T type)
    {
        List<Room> meta = new List<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            if(rooms[i].metaData.Get<T>().Equals(type))
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

    private static void RemoveMetaData<T>(this Room[] rooms)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].metaData.Remove<T>();
        }
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
