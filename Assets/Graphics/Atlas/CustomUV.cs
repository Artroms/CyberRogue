using UnityEngine;
using Exstensions;

public class CustomUV : MonoBehaviour
{
    [SerializeField] private Mesh serialQuad;

    public int x;
    public int y;

    public Mesh GenerateMesh()
    {
        var material = GetComponent<MeshRenderer>().sharedMaterial;
        var texture = material.GetTexture("_MainTex");
        var size = new Vector2(1f / texture.width * 32, 1f / texture.height * 32);
        var start = new Vector2(x * size.x, y * size.y);

        Vector2[] uvs = new Vector2[4];
        uvs[0] = start;
        uvs[1] = new Vector2(start.x + size.x, start.y);
        uvs[2] = new Vector2(start.x, start.y + size.y);
        uvs[3] = new Vector2(start.x + size.x, start.y + size.y);
        var mesh = serialQuad.Clone();

        mesh.SetUVs(0, uvs);
        return mesh;
    }


    public void SetMesh(Mesh mesh)
    {
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
