using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FileDialogOpener : MonoBehaviour
{
    Texture2D texture;
    public void Find(string path)
    {
        byte[] data;
        if(System.IO.File.Exists(path))
        {
            data = System.IO.File.ReadAllBytes(path);
            texture.LoadImage(data);
            texture.requestedMipmapLevel = 0;
            texture.filterMode = FilterMode.Point;
        }
    }
}
