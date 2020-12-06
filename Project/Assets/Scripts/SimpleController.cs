using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(dir != Vector3.zero)
            transform.Translate(dir * Time.deltaTime * 5);
        if(Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up * Time.deltaTime * 5);

    }
}
