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
        metaData.Add(option);
        height = 1;
    }

    public Room(int width, int length, int height, RoomOption.AffectedByOthers option = RoomOption.AffectedByOthers.Simple)
    {
        this.width = width;
        this.length = length;
        metaData.Add(option);
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
        metaData.Add(option);
    }

    public Room[] BSP()
    {
        var l = new BSP.Leaf(this);
        return l.GetRooms();
    }

    public Room[] Floors()
    {
        return FloorSeparator.Separate(this);
    }
}
