using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFiller : IRoomFiller
{
    public GameObject floor;

    public FloorFiller(GameObject floor)
    {
        this.floor = floor;
    }

    public void FillRoom(Room room)
    {
        var g = new GameObject();
        for (int i = 0; i < room.width; i++)
        {
            for (int j = 0; j < room.height; j++)
            {
                for (int k = 0; k < room.length; k++)
                {
                    Vector3 pos = new Vector3(room.x + i, room.y + j, room.z + k);
                    //pos.z *= 99f/70f;
                    //pos.y *= 99f/70f;
                    var o = Object.Instantiate(floor, pos, floor.transform.rotation);
                    o.isStatic = true;
                    o.transform.SetParent(g.transform);
                }
            }
        }
    }
}
