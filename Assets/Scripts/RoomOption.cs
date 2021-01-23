using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOption
{
    public enum AffectedByOthers
    {
        Simple,
        NotAffected
    };

    public class Doors
    {
        public HashSet<Vector3Int> list = new HashSet<Vector3Int>();
    }

}
