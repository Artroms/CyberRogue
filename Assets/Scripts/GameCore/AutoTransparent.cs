using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTransparent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].gameObject.AddComponent<TransparentConverter>();
        }
    }
}
