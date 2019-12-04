using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIElement
{
    public float zoom;
    public Vector3 offset;
    public GameObject prefab;
    private Camera snapShotCamera;
    public RenderTexture prefabIcon;

    public void GenerateIcon(Camera snapCam, Transform instance)
    {
        prefabIcon = new RenderTexture(256, 256, 100, RenderTextureFormat.ARGB32);
        prefabIcon.Create();
        snapShotCamera = snapCam;
        snapShotCamera.transform.position = Vector3.zero;
        snapShotCamera.transform.parent = instance;

        OffsetCam(Vector3.zero);
        snapShotCamera.targetTexture = prefabIcon;
        snapShotCamera.Render();
        //ClearOutRenderTexture(prefabIcon);
    }

    void OffsetCam(Vector3 lookAtPos)
    {
        snapShotCamera.transform.position += offset;
        snapShotCamera.orthographicSize += zoom;
        snapShotCamera.transform.LookAt(lookAtPos);
    }
}
