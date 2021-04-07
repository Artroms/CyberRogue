using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CustomUV))]
[CanEditMultipleObjects]
public class CustomUVEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Mesh"))
        {
            var cuvs = serializedObject;
            foreach (CustomUV cuv in cuvs.targetObjects)
            {
                var mesh = cuv.GenerateMesh();
                var loaded = AssetDatabase.LoadAssetAtPath<Mesh>(@"Assets\GeneratedMeshes\" + UVToString(mesh.uv) + ".mesh");
                if (loaded == null)
                {
                    loaded = mesh;
                    AssetDatabase.CreateAsset(loaded, @"Assets\GeneratedMeshes\" + UVToString(loaded.uv) + ".mesh");
                    AssetDatabase.SaveAssets();
                }
                cuv.SetMesh(loaded);
                var prefabStage = PrefabStageUtility.GetPrefabStage(cuv.gameObject);
                if (prefabStage != null)
                {
                    EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                }
            }
        }
    }

    private string UVToString(Vector2[] uv)
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < uv.Length; i++)
        {
            sb.Append(uv[i].ToString("F"));
        }
        return sb.ToString();
    }
}
