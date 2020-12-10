using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BSP
{
    public class Leaf
    {
    public Leaf left, right;

    public int x;
    public int y;
    public int z;
    public int width;
    public int height;
    public int length;

    public int debugId;

    public static int minSize = 6;
    public static int maxSize = 12;

    private static int debugCounter = 0;

    public Leaf(int x, int y, int z, int width, int height, int length)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.width = width;
      this.height = height;
      this.length = length;
      debugId = debugCounter;
      debugCounter++;
    }

    public Leaf(Room room)
    {
      this.x = room.x;
      this.y = room.y;
      this.z = room.z;
      this.width = room.width;
      this.height = room.height;
      this.length = room.length;
      debugId = debugCounter;
      debugCounter++;
    }

    public bool IAmLeaf()
    {
      return left == null && right == null;
    }

    public bool Split()
    {
      if (!IAmLeaf())
      {
        return false;
      }

      bool splitH;
      if ((float)width / (float)length >= 1.25f)
      {
        splitH = false;
      }
      else if ((float)length / (float)width >= 1.25f)
      {
        splitH = true;
      }
      else
      {
        splitH = Random.Range (0.0f, 1.0f) > 0.5;
      }

      if (Mathf.Min((float)length, (float)width) / 2 < minSize)
      {
        //Debug.Log ("Sub-dungeon " + debugId + " will be a leaf");
        return false;
      }

      if (splitH) {

        int split = Random.Range (minSize, (width - minSize));

        left = new Leaf (x, y, z, width, height, split);
        right = new Leaf (x, y, z + split, width, height, length - split);
      }
      else {
        int split = Random.Range (minSize, (length - minSize));

        left = new Leaf (x, y, z, split, height, length);
        right = new Leaf (x + split, y, z, width - split, height, length);
      }
      return true;
    }

    public void CreateBSP()
    {
        CreateBSP(this);
    }

    public Room[] GetRooms()
    {
        List<Room> rooms = new List<Room>();
        GetRooms(this, rooms);
        return rooms.ToArray();
    }

    public List<Room> GetRoomsList()
    {
        List<Room> rooms = new List<Room>();
        GetRooms(this, rooms);
        return rooms;
    }

    private void GetRooms(Leaf current, List<Room> rooms)
    {
        if(current.left == null && current.right == null)
        {
            Room r = new Room(current.width, current.length);
            r.x = current.x;
            r.y = current.y;
            r.z = current.z;
            rooms.Add(r);
        }
        if(current.left != null)
            GetRooms(current.left, rooms);
        if(current.right != null)
            GetRooms(current.right, rooms);
    }

    private void CreateBSP(Leaf leaf)
    {

      if (leaf.IAmLeaf()) {
        // if the sub-dungeon is too large
        if (leaf.width > maxSize
          || leaf.length > maxSize
          || Random.Range(0.0f,1.0f) > 0.25)
          {
          if (leaf.Split ())
          {

            CreateBSP(leaf.left);
            CreateBSP(leaf.right);
          }
        }
      }
  }


}
}
