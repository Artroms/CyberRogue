using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour
{
    public Camera cam;
    public Animator animator;
    private RenderTexture currentRT;

    public void Start()
    {
        StartCoroutine(Screenshoter());
    }
    // Take a "screenshot" of a camera's Render Texture.
    Texture2D RTImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGBA32, false, true);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }

    public IEnumerator Screenshoter()
    {
        currentRT = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        cam.targetTexture = currentRT;
        animator.speed = 0;
        animator.Play("New State", 0, 0);
        var step = 1f / 60f;
        for (float i = 0; i < 1; i+= step)
        {
            animator.Play("New State", 0, i);
            yield return new WaitForEndOfFrame();
            var im = RTImage(cam);
            byte[] bytes = im.EncodeToPNG();
            DestroyImmediate(im);
            var path = @"C:\Users\Artromskiy\Documents\GitHub\Cyber Rogue\Assets\TestFolder\Rendered\" + i.ToString("F") + ".png";
            System.IO.File.WriteAllBytes(path, bytes);
            Debug.Log(i);
        }
        cam.targetTexture = null;
        DestroyImmediate(currentRT);
    }
}
