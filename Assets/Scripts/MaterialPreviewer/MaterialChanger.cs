using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public List<Mesh> meshes;
    public Material material;
    public MeshRenderer meshRenderer;

    public void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.sharedMaterial;
    }

    public void SetMainTexture(string path)
    {
        byte[] data;
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        if (System.IO.File.Exists(path))
        {
            data = System.IO.File.ReadAllBytes(path);
            texture.LoadImage(data);
            texture.requestedMipmapLevel = 0;
            texture.filterMode = FilterMode.Point;
        }
        meshRenderer.sharedMaterial.SetTexture("_BaseMap", texture);
        //meshRenderer.sharedMaterial = material;
    }
    public void SetSpecularMap(Texture texture)
    {
        material.SetTexture("MainTex", texture);
    }

    public void SetNormalMap(Texture texture)
    {
        material.SetTexture("MainTex", texture);
    }

    public void SetOcclusionMap(Texture texture)
    {
        material.SetTexture("MainTex", texture);
    }

    public void SetEmissionMap(Texture texture)
    {
        material.SetTexture("MainTex", texture);
    }
}
