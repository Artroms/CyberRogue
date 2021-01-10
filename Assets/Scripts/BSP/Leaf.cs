using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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

        public static System.Random rnd = new System.Random();

        public static int minSize = 6;
        public static int maxSize = 12;

        public static bool sizeDependent = false;

        private static float maxDelta = 0.1f;
        public static float MaxDelta
        {
            get => maxDelta;
            set => maxDelta = Mathf.Clamp(value, 0, 1);
        }

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

        public bool IAmLeaf => left == null && right == null;

        public bool SplitRandom()
        {
            if (!IAmLeaf)
            {
                return false;
            }

            if (Mathf.Min((float)length, (float)width) / 2 < minSize)
            {
                return false;
            }

            bool splitH;
            if ((float)width / (float)length >= 1.1)
            {
                splitH = false;
            }
            else if ((float)length / (float)width >= 1.1)
            {
                splitH = true;
            }
            else
            {
                splitH = rnd.NextDouble() > 0.5;
            }

            if (splitH)
            {
                int split = rnd.Next(minSize, (length - minSize));
                left = new Leaf(x, y, z, width, height, split);
                right = new Leaf(x, y, z + split - 1, width, height, length - split + 1);
            }
            else
            {
                int split = rnd.Next(minSize, (width - minSize));
                left = new Leaf(x, y, z, split, height, length);
                right = new Leaf(x + split - 1, y, z, width - split + 1, height, length);
            }
            return true;
        }

        public bool SplitSizeDependent()
        {
            if (!IAmLeaf)
            {
                return false;
            }

            if (Mathf.Min((float)length, (float)width) / 2 < minSize)
            {
                return false;
            }

            bool splitH;
            if ((float)width / (float)length >= 1.1)
            {
                splitH = false;
            }
            else if ((float)length / (float)width >= 1.1)
            {
                splitH = true;
            }
            else
            {
                splitH = rnd.NextDouble() > 0.5;
            }

            if (splitH)
            {
                int split = rnd.Next(minSize, (length - minSize));
                left = new Leaf(x, y, z, width, height, split);
                right = new Leaf(x, y, z + split - 1, width, height, length - split + 1);
            }
            else
            {
                int split = rnd.Next(minSize, (width - minSize));
                left = new Leaf(x, y, z, split, height, length);
                right = new Leaf(x + split - 1, y, z, width - split + 1, height, length);
            }

            if (left.height < left.width && left.height / left.width < MaxDelta)
                SplitRandom();
            if (right.height < right.width && right.height / right.width < MaxDelta)
                SplitRandom();

            return true;
        }


        public Room[] GetRooms()
        {
            List<Room> rooms = new List<Room>();
            if (sizeDependent)
                CreateBSP(this);
            else
                CreateBSPSizeDependent(this);
            GetRooms(this, rooms);
            return rooms.ToArray();
        }

        private void GetRooms(Leaf current, List<Room> rooms)
        {
            if (current.IAmLeaf)
            {
                Room r = new Room(current.width, current.length);
                r.x = current.x;
                r.y = current.y;
                r.z = current.z;
                rooms.Add(r);
            }
            if (current.left != null)
                GetRooms(current.left, rooms);
            if (current.right != null)
                GetRooms(current.right, rooms);
        }

        private void CreateBSP(Leaf leaf)
        {
            if (leaf.IAmLeaf)
            {
                if (leaf.width > maxSize || leaf.length > maxSize || rnd.NextDouble() > 0.25)
                {
                    if (leaf.SplitRandom())
                    {
                        var t1 = Task.Run(() => CreateBSP(leaf.left));
                        var t2 = Task.Run(() => CreateBSP(leaf.right));
                        Task.WaitAll(t1, t2);
                    }
                }
            }
        }

        private void CreateBSPSizeDependent(Leaf leaf)
        {
            if (leaf.IAmLeaf)
            {
                if (leaf.width > maxSize || leaf.length > maxSize || rnd.NextDouble() > 0.25)
                {
                    if (leaf.SplitSizeDependent())
                    {
                        var t1 = Task.Run(() => CreateBSP(leaf.left));
                        var t2 = Task.Run(() => CreateBSP(leaf.right));
                        Task.WaitAll(t1, t2);
                    }
                }
            }
        }
    }

}
