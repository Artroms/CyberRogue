using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSeparator
{
    public static Room[] Separate(Room room)
    {
        Room[] rooms = new Room[room.height];
        for(int i = 0; i < room.height; i++)
        {
            Vector3Int min = new Vector3Int(room.x, room.y + i, room.z);
            Vector3Int size = new Vector3Int(room.width, 1, room.length);
            rooms[i] = new Room(min, size);
        }
        return rooms;
    }
}
