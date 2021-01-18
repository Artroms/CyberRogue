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
            rooms[i] = new Room(Random.Range(5,15),Random.Range(3,30),Random.Range(5,15));
        }
        BSP.Leaf.maxSize = 15;
        BSP.Leaf.minSize = 7;
        Leaf.sizeDependent = true;
        Leaf.threading = false;
        UnityEngine.Debug.Log(Leaf.threading);
        /*
        rooms[0].width = 15;
        rooms[0].length = 15;
        rooms[0].metaData.Add(RoomOption.AffectedByOthers.NotAffected);
        */

        Stopwatch sw = new Stopwatch();
        sw.Start();
        System.Func<int> shift = () => Random.Range(-2, 2);
        //rooms = rooms.MoveRooms(5).Floors().BSP().Resize(-2,0,-2).ShiftSize(shift, () => 0, shift);
	rooms = rooms.MoveRooms(5).Floors();
	/*
        var connection = rooms.GetHalls();
        var all = new Room[rooms.Length + connection.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            all[i] = rooms[i];
        }
        for (int i = 0; i < connection.Length; i++)
        {
            all[i + rooms.Length] = connection[i];
        }
        rooms = all;
	*/
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
