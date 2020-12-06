using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartRoomMover : IRoomArranger
{
    public int timeout;
    public SizeOptions sizeOptions;
    public float stepSize;
    private int largeSize;

    public SmartRoomMover(int stepSize = 1, SizeOptions sizeOptions = SizeOptions.NotAffected, int largeSize = 25)
    {
        this.stepSize = stepSize;
        this.sizeOptions = sizeOptions;
        this.timeout = 100000;
        this.largeSize = largeSize;
    }

    public void ArrangeRooms(Room[] rooms)
    {
        rooms.MoveRooms();
    }



    public enum SizeOptions
    {
        NotAffected,
        BiggerOut,
        BiggerCenter
    };

    private enum RoomType
    {
        None,
        Heavy,
        Light,
        NotAffected
    };
}
