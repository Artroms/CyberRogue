using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private GameObject effect;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        effect = Instantiate(effect);
        effect.transform.position = gameObject.transform.position;
        Destroy(effect.gameObject, 1);
    }
}
