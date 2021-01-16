using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeMover : MonoBehaviour
{
    public Vector3 moveTo;
    public float size;
    // Start is called before the first frame update
    void Start()
    {
        size = Camera.main.orthographicSize;
        moveTo = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime);
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, size, Time.deltaTime * 0.5f);
    }
}
