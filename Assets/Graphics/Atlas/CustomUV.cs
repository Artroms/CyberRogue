using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exstensions;

public class CustomUV : MonoBehaviour, ISerializationCallbackReceiver
{
    private MeshFilter meshFilter;
    private Material material;
    [SerializeField] private Mesh serialQuad;
    private static Mesh Quad { get; set; }
    [SerializeField] private static readonly Dictionary<UV, Mesh> UVToMesh = new Dictionary<UV, Mesh>();
    public int x;
    public int y;
    // Start is called before the first frame update

    // Update is called once per frame
    [ExecuteInEditMode, ContextMenu("OnValidate")]
    void OnValidate()
    {
        ResetMesh();
    }

    private void Reset()
    {
        UVToMesh.Clear();
        ResetMesh();
    }

    private void ResetMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        material = GetComponent<MeshRenderer>().sharedMaterial;
        var texture = material.GetTexture("_MainTex");
        if (meshFilter == null || texture == null)
            return;
        var size = new Vector2(1f / texture.width * 32, 1f / texture.height * 32);
        var start = new Vector2(x * size.x, y * size.y);

        Vector2[] uvs = new Vector2[4];
        uvs[0] = start;
        uvs[1] = new Vector2(start.x + size.x, start.y);
        uvs[2] = new Vector2(start.x, start.y + size.y);
        uvs[3] = new Vector2(start.x + size.x, start.y + size.y);

        var newUV = new UV();
        newUV.FromArray(uvs);

        var oldUV = new UV();
        oldUV.FromArray(meshFilter.sharedMesh.uv);

        if (!UVToMesh.ContainsKey(newUV))
        {
            var mesh = Quad.Clone();
            mesh.SetUVs(0, newUV.ToArray());
            UVToMesh.Add(newUV, mesh);
        }

        if (oldUV.Equals(newUV))
        {
            return;
        }
        meshFilter.sharedMesh = UVToMesh[newUV];
    }



    [ContextMenu("LogIndex")]
    private void LogIndex()
    {
        Debug.Log(meshFilter.sharedMesh.GetHashCode());
    }

    [ContextMenu("ClearMeshList")]
    private void ClearMeshList()
    {
        UVToMesh.Clear();
    }

    public void OnBeforeSerialize()
    {
        serialQuad = Quad;
    }

    public void OnAfterDeserialize()
    {
        Quad = serialQuad;
    }

    private struct UV
    {
        public Vector2 uv0;
        public Vector2 uv1;
        public Vector2 uv2;
        public Vector2 uv3;

        public Vector2[] ToArray()
        {
            Vector2[] array = new Vector2[4];
            array[0] = uv0;
            array[1] = uv1;
            array[2] = uv2;
            array[3] = uv3;
            return array;
        }

        public void FromArray(Vector2[] array)
        {
            var delta = new Vector2(Mathf.Floor(array[0].x), Mathf.Floor(array[0].y));
            uv0 = array[0] - delta;
            uv1 = array[1] - delta;
            uv2 = array[2] - delta;
            uv3 = array[3] - delta;
        }
    }

}
