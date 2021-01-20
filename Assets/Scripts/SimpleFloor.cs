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
    public GameObject rndLight;
    public Room[] rooms = new Room[0];

    void Start()
    {
        TestRoom();
    }

    void TestRoom()
    {
        rooms = new Room[rooms.Length];
        rooms[0] = new Room(100, 1, 100);
        BSP.Leaf.maxSize = 10;
        BSP.Leaf.minSize = 8;
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
        System.Func<int> shift = () => Random.Range(-3, 3);
        rooms = rooms.MoveRooms(5).Floors().BSP().Resize(-2,0,-2).ShiftSize(shift, () => 0, shift);
        //rooms = rooms.MoveRooms(5).Floors();
        Vector3Int min = rooms.Min();
        Vector3Int max = rooms.Max();
        Vector3Int size = max - min;
        bool[,,] matrix = new bool[size.x + 1, size.y + 1, size.z + 1];
        for (int j = 0; j < size.x; j++)
        {
            for (int k = 0; k < size.y; k++)
            {
                for (int l = 0; l < size.z; l++)
                {
                    matrix[j, k, l] = false;
                }
            }
        }
        UnityEngine.Debug.Log(size);
        for (int i = 0; i < rooms.Length; i++)
        {
            for (int j = 0; j < rooms[i].width; j++)
            {
                for (int k = 0; k < rooms[i].height; k++)
                {
                    for (int l = 0; l < rooms[i].length; l++)
                    {
                        Vector3Int pos = new Vector3Int(rooms[i].x + j - min.x, rooms[i].y + k - min.y, rooms[i].z + l - min.z);
                        UnityEngine.Debug.Log(pos);
                        matrix[pos.x, pos.y, pos.z] = true;
                    }
                }
            }
        }
        var connection = rooms.GetHalls();
        var tmp = rooms;
        rooms = connection;
        for (int i = 0; i < rooms.Length; i++)
        {
            for (int j = 0; j < rooms[i].width; j++)
            {
                for (int k = 0; k < rooms[i].height; k++)
                {
                    for (int l = 0; l < rooms[i].length; l++)
                    {
                        Vector3Int pos = new Vector3Int(rooms[i].x + j - min.x, rooms[i].y + k - min.y, rooms[i].z + l - min.z);
                        matrix[pos.x, pos.y, pos.z] = true;
                    }
                }
            }
        }
        List<Room> Walls = new List<Room>();

        /*
        GameObject g = new GameObject();
        for (int j = 0; j < size.x; j++)
        {
            for (int k = 0; k < size.y; k++)
            {
                for (int l = 0; l < size.z; l++)
                {
                    if (matrix[j, k, l] == false)
                    {
                        var w = Instantiate(wall);
                        w.transform.position = new Vector3(j + min.x, k + min.y, l + min.z);
                        w.transform.SetParent(g.transform);
                        if(Random.Range(0,100) > 95)
                        {
                            var li = Instantiate(rndLight);
                            li.transform.position = new Vector3(j + min.x, k + min.y, l + min.z);
                            li.transform.SetParent(g.transform);
                        }
                    }
                    else
                    {
                        var f = Instantiate(floor);
                        f.transform.position = new Vector3(j + min.x, k + min.y, l + min.z);
                        f.transform.SetParent(g.transform);
                    }
                }
            }
        }
        */

        for (int j = 0; j < size.x; j++)
        {
            for (int k = 0; k < size.y; k++)
            {
                for (int l = 0; l < size.z; l++)
                {
                    if (matrix[j, k, l] == false)
                    {
                        Walls.Add(new Room(new Vector3Int(j + min.x, k + min.y, l + min.z), new Vector3Int(1, 1, 1)));
                    }
                }
            }
        }
        rooms = Walls.ToArray();

        sw.Stop();
        UnityEngine.Debug.Log("MoveRooms().Floors().BSP() miliseconds elapsed: " + sw.ElapsedMilliseconds);
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
