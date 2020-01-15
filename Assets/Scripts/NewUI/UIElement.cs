using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIElement
{
    public float scale;
    public Vector3 offset;
    public GameObject prefab;
    private Camera snapShotCamera;
    public RenderTexture prefabIcon;
    [HideInInspector]public bool isActive = true;

    public Category myCategory;

    public void GenerateIcon(Camera snapCam, Transform instance)
    {
        prefabIcon = new RenderTexture(256, 256, 100, RenderTextureFormat.ARGB32);
        prefabIcon.name = instance.name.Replace("(Clone)", "") + "_snapshot";
        prefabIcon.Create();
        snapShotCamera = snapCam;
        snapShotCamera.transform.position = Vector3.zero;
        snapShotCamera.transform.parent = instance;

        OffsetCam(Vector3.zero);
        instance.localScale *= scale;
        snapShotCamera.targetTexture = prefabIcon;
        snapShotCamera.Render();
        instance.localScale *= 1/scale;
        isActive = true;
    }

    void OffsetCam(Vector3 lookAtPos)
    {

        snapShotCamera.transform.position += offset;
        snapShotCamera.transform.LookAt(lookAtPos);
    }
}

public enum Category
{
    people,
    vehicles,
    equipment,
    materials,
    all
}