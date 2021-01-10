using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using BSP;

public class SimpleFloor : MonoBehaviour
{
    public Vector3 offset;
    public int width;
    public int height;
    public IRoomArranger runner = new SmartRoomMover(3);
    public IRoomFiller filler;
    public FloorFiller floorFill;
    public GameObject wall;
    public GameObject floor;
    public Room[] rooms = new Room[0];

    void Start()
    {
        TestRoom();
    }

    void TestRoom()
    {
        rooms = new Room[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            int s = Random.Range(10000,10000);
            rooms[i] = new Room(s, s);
            rooms[i].height = 1;
        }
        BSP.Leaf.maxSize = 15;
        BSP.Leaf.minSize = 5;
        Leaf.sizeDependent = false;
        /*
        rooms[0].width = 15;
        rooms[0].length = 15;
        rooms[0].metaData.Add(RoomOption.AffectedByOthers.NotAffected);
        */

        Stopwatch sw = new Stopwatch();
        sw.Start();
        rooms = rooms.MoveRooms(5).Floors().BSP();
        sw.Stop();
        UnityEngine.Debug.Log("MoveRooms().Floors().BSP() miliseconds elapsed: " + sw.ElapsedMilliseconds);
        /*
        FloorFiller filler = new FloorFiller(floor);
        WallFiller filler2 = new WallFiller(wall);
        for (int i = 0; i < rooms.Length; i++)
        {
            filler.FillRoom(rooms[i]);
            filler2.FillRoom(rooms[i]);
        }
        */
    }

    public void OnDrawGizmos()
    {
        CubeDraw();
    }

    private void CubeDraw()
    {
        Gizmos.color = Color.green;
        //Gizmos.matrix = transform.localToWorldMatrix;
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector3 pos = new Vector3(rooms[i].x + (float)(rooms[i].width - 1) / 2, rooms[i].y + (float)(rooms[i].height - 1) / 2, rooms[i].z + (float)(rooms[i].length - 1) / 2);
            Vector3 size = new Vector3(rooms[i].width, rooms[i].height, rooms[i].length);
            Gizmos.DrawWireCube(pos, size);
        }
    }

    private void ResizedDraw()
    {
        Gizmos.color = Color.green;
        //Gizmos.matrix = transform.localToWorldMatrix;
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector3 pos = new Vector3(rooms[i].x + (float)(rooms[i].width - 1) / 2, rooms[i].y + (float)(rooms[i].height - 1) / 2, rooms[i].z + (float)(rooms[i].length - 1) / 2);
            pos.y *= 99f / 70f;
            pos.z *= 99f / 70f;
            Vector3 size = new Vector3(rooms[i].width, rooms[i].height, rooms[i].length);
            size.y *= 99f / 70f;
            size.z *= 99f / 70f;
            Gizmos.DrawWireCube(pos, size);
        }
    }
}
