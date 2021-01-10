using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFiller : IRoomFiller
{
    GameObject wall;
    public WallFiller(GameObject wall)
    {
        this.wall = wall;
    }

    public void FillRoom(Room room)
    {
        for (int i = 0; i < room.width; i++)
        {
            for (int j = 0; j < room.height; j++)
            {
                for (int k = 0; k < room.length; k++)
                if(i == 0 || i == room.width - 1 ||  k == 0 || k == room.length - 1)
                {
                    var w = Object.Instantiate(wall);
                    var pos = new Vector3(room.x + i, room.y + j, room.z + k);
                    //pos.z *= 99f/70f;
                    //pos.y *= 99f/70f;
                    w.transform.position = pos;
                }
            }
        }
    }
}
