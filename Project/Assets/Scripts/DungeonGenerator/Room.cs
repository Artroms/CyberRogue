using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public int x;
    public int y;
    public int z;
    public int width;
    public int length;
    public int height;
    public MetaData metaData = new MetaData();

    public Room(int width, int length, RoomOption.AffectedByOthers option = RoomOption.AffectedByOthers.Simple)
    {
        this.width = width;
        this.length = length;
        metaData.Add<RoomOption.AffectedByOthers>(option);
        height = 1;
    }

    public Room(int width, int length, int height, RoomOption.AffectedByOthers option = RoomOption.AffectedByOthers.Simple)
    {
        this.width = width;
        this.length = length;
        metaData.Add<RoomOption.AffectedByOthers>(option);
        this.height = height;
    }

    public Room(Vector3Int min, Vector3Int size, RoomOption.AffectedByOthers option = RoomOption.AffectedByOthers.Simple)
    {
        this.width = size.x;
        this.height = size.y;
        this.length = size.z;
        this.x = min.x;
        this.y = min.y;
        this.z = min.z;
        metaData.Add<RoomOption.AffectedByOthers>(option);
    }

    public Room[] BSP()
    {
        var l = new BSP.Leaf(this);
        l.CreateBSP();
        return l.GetRooms();
    }

    public Room[] Floors()
    {
        return FloorSeparator.Separate(this);
    }

    public static Room[] BSP(this Room[] rooms)
    {
        List<Room> newRooms = new List<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            return newRooms.AddRange(rooms[i].BSP());
        }
    }

    public static Room[] Floors(this Room[] rooms)
    {
        List<Room> newRooms = new List<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            return newRooms.AddRange(rooms[i].Floors());
        }
    }
}
