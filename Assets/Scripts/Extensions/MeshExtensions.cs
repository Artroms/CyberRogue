using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Exstensions
{
    public static class MeshExtensions
    {
        public static Mesh Clone(this Mesh mesh)
        {
            int bytes = 0;
            Mesh newmesh = new Mesh();
            newmesh.vertices = mesh.vertices;
            bytes += mesh.vertices.Length * 3 * 4;
            newmesh.triangles = mesh.triangles;
            bytes += mesh.triangles.Length * 4;
            newmesh.uv = mesh.uv;
            bytes += mesh.uv.Length * 2 * 4;
            newmesh.normals = mesh.normals;
            bytes += mesh.normals.Length * 3 * 4;
            newmesh.colors = mesh.colors;
            bytes += mesh.colors.Length * 4;
            newmesh.tangents = mesh.tangents;
            bytes += mesh.tangents.Length * 4 * 4;
            Debug.Log(bytes);
            return newmesh;
        }
    }
}
