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
        x = 0;
        y = 0;
        z = 0;
        height = 1;
        this.width = width;
        this.length = length;
        metaData.Add(option);
    }

    public Room(int width, int height, int length, RoomOption.AffectedByOthers option = RoomOption.AffectedByOthers.Simple)
    {
        x = 0;
        y = 0;
        z = 0;
        this.width = width;
        this.length = length;
        this.height = height;
        metaData.Add(option);
    }

    public Room(Vector3Int min, Vector3Int size, RoomOption.AffectedByOthers option = RoomOption.AffectedByOthers.Simple)
    {
        width = size.x;
        height = size.y;
        length = size.z;
        x = min.x;
        y = min.y;
        z = min.z;
        metaData.Add(option);
    }

    public Vector3Int Center
    {
        get
        {
            return new Vector3Int(x + width / 2, y + height / 2, z + length / 2);
        }
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
