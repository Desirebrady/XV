using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeController : MonoBehaviour
{
    public static BuildModeController instance;
    Transform CameraHandler;
    Camera SectionCamera;
    Transform UIElementsHolder;

    public float camForwardOffset;

    public UIElement[] UIElements;

    private float UIElementsLengthOffset;
    private float UIElementsRotationOffset;

    private int NumberOfElements;
    private bool nextItemBool = false;
    private bool nextSectionBool = false;
    private bool previousItemBool = false;
    private bool previousSectionBool = false;

    private Quaternion newRotation;
    private GameObject newInstance;

    private void Awake()
    {
        if (instance != this)
        {
            instance = null;
        }

        if (instance == null)
        {
            instance = this;
        }
    }


    void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        
        foreach (var child in children)
        {
            if (child.name == "UIElementsHolder")
            {
                UIElementsHolder = child;
            }

            if (child.name == "CameraHandler")
            {
                CameraHandler = child;
            }
        }
     

        
        if (UIElements.Length > 0)
        {
            NumberOfElements = (UIElements.Length % 2 == 0) ? UIElements.Length : UIElements.Length + 1;

            UIElementsRotationOffset = 360 / NumberOfElements;
            UIElementsLengthOffset = NumberOfElements/2;

            float currentRotationOffset = 0;

            //Placing Object in A Ring
            foreach (var element in UIElements)
            {
                currentRotationOffset += UIElementsRotationOffset;

                var instance = GameObject.Instantiate(element.elementTransform, UIElementsHolder);
                instance.Rotate(Vector3.up, currentRotationOffset);
                instance.localScale = element.elementScale;
                instance.localPosition += instance.forward * UIElementsLengthOffset;
            }
        }
        

        SectionCamera = GetComponentInChildren<Camera>();
        OffsetCam();

    }

    void Update()
    {
        if (newRotation != null)
            CameraHandler.rotation = Quaternion.Lerp(CameraHandler.rotation, newRotation, 0.2f);
        CreateInstance();
    }

    void OffsetCam()
    {
        SectionCamera.transform.localPosition = SectionCamera.transform.forward * camForwardOffset;
    }

    #region ROTATIONS
    private void RotateToNextItem()
    {
        if (nextItemBool)
        {
            newRotation = CameraHandler.rotation * Quaternion.Euler(0, UIElementsRotationOffset, 0);
            nextItemBool = false;
        }
    }

    private void RotateToNextSection()
    {
        if (nextSectionBool)
        {
            newRotation = CameraHandler.rotation * Quaternion.Euler(0, UIElementsRotationOffset * 3, 0);
            nextSectionBool = false;
        }
    }

    private void RotateToPreviousItem()
    {
        if (previousItemBool)
        {
            newRotation = CameraHandler.rotation * Quaternion.Euler(0, -UIElementsRotationOffset, 0);
            previousItemBool = false;
        }
    }

    private void RotateToPreviousSection()
    {
        if (previousSectionBool)
        {
            newRotation = CameraHandler.rotation * Quaternion.Euler(0, -UIElementsRotationOffset * 3, 0);
            previousSectionBool = false;
        }
    }

    public void RotateToTab(int interval)
    {
        newRotation = Quaternion.Euler(0, UIElementsRotationOffset * interval, 0);

    }
    #endregion

    #region FOR BUTTONS
    public void nextSection() { nextSectionBool = true; RotateToNextSection(); }
    public void nextItem() { nextItemBool = true; RotateToNextItem(); }
    public void previousSection() { previousSectionBool = true; RotateToPreviousSection(); }
    public void previousItem() { previousItemBool = true; RotateToPreviousItem(); }
    #endregion

    #region InstanceController
    void CreateInstance()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (newInstance != null)
            {
                GameObject.Destroy(newInstance);
                newInstance = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (newInstance != null)
            {
                newInstance = null;
            }

            Ray ray = SectionCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider != null)
                {
                    Debug.Log("Object: " + hit.collider.name);
                    newInstance = GameObject.Instantiate(hit.collider.gameObject);
                    newInstance.transform.rotation = Quaternion.identity;
                    newInstance.transform.localScale = new Vector3(1, 1, 1);
                    newInstance.GetComponent<UIElementController>().isDisplay = false;
                    newInstance.GetComponent<UIElementController>().isNewInstance = true;
                    newInstance.layer = LayerMask.GetMask("Default");
                }
            }
        }
    }

    #endregion
}

[System.Serializable]
public class UIElement
{
    public Transform elementTransform;
    public Vector3 elementScale;
}