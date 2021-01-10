using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraParallax : MonoBehaviour {

    public Camera mainCamera;
    public Camera nearCamera;

    void LateUpdate()
    {
        UpdateCameras();
    }

    public void UpdateCameras()
    {
        if(mainCamera == null || nearCamera == null)
            return;

        //change clipping planes based on main camera z-position
        nearCamera.nearClipPlane = mainCamera.farClipPlane;

        nearCamera.aspect = mainCamera.aspect;
        nearCamera.fieldOfView = Mathf.Atan(mainCamera.orthographicSize / nearCamera.nearClipPlane) / Mathf.Deg2Rad * 2;
    }

}