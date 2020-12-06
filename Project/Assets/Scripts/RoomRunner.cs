using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRunner : IRoomArranger
{
    public void ArrangeRooms(Room[] rooms)
    {
        Rect[] rects = new Rect[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector2 pos = Random.insideUnitCircle;
            Vector2 size = new Vector2(rooms[i].width + 1, rooms[i].length + 1);
            rects[i] = new Rect(pos, size);
        }
        int iter = 0;
        bool overlaps = false;
        do {
            overlaps = false;
            for (int i = 0; i < rooms.Length; i++)
            {
                for (int j = 0; j < rooms.Length; j++)
                {
                    if(rects[i] != rects[j] && rects[i].Overlaps(rects[j]))
                    {
                        rects[i].center += (rects[i].center - rects[j].center).normalized * 2;
                        overlaps = true;
                    }
                }
            }
            iter++;
        } while (overlaps == true && iter < 100000);
        if(iter == 100000)
            throw new System.TimeoutException();
        Debug.Log(iter);
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].x = Mathf.RoundToInt(rects[i].center.x - rooms[i].width / 2);
            rooms[i].z = Mathf.RoundToInt(rects[i].center.y - rooms[i].length / 2);
        }
    }
}
