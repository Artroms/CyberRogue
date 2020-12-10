﻿using System.Collections;
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
    public Room[] rooms;

    void Start()
    {
        TestRoom();
    }

    void TestRoom()
    {
        rooms = new Room[rooms.Length];
        for (int i = 0; i < rooms.Length; i++)
        {
            int s = Random.Range(30,100);
            rooms[i] = new Room(50, 50);
            rooms[i].height = Random.Range(1, 200);
        }
        rooms[0].width = 15;
        rooms[0].length = 15;
        rooms[0].metaData.Add(RoomOption.AffectedByOthers.NotAffected);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        rooms = rooms.MoveRooms(5).Floors().BSP();
        sw.Stop();
        UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector3 pos = new Vector3(rooms[i].x + (float)(rooms[i].width - 1) / 2, rooms[i].y + (float)(rooms[i].height - 1) / 2, rooms[i].z + (float)(rooms[i].length - 1) / 2);
            pos.y *= 99f/70f;
            pos.z *= 99f/70f;
            Vector3 size = new Vector3(rooms[i].width, rooms[i].height, rooms[i].length);
            size.y *= 99f/70f;
            size.z *= 99f/70f;
            Gizmos.DrawWireCube(pos, size);
        }
    }
}
