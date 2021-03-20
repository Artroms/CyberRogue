using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonePlayer : MonoBehaviour
{
    private static SingletonePlayer instance;
    public static SingletonePlayer Instance => instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
}
