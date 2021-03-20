using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentConverter : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Transform player;
    void Start()
    {
        player = SingletonePlayer.Instance.transform;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SingletonePlayer.Instance.transform.hasChanged)
        {
            if(transform.position.y - player.position.y > 3 && transform.position.z < player.position.z)
            {
                meshRenderer.enabled = false;
            }
            else
            {
                meshRenderer.enabled = true;
            }
        }
    }
}
