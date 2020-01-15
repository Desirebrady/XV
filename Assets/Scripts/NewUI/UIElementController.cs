using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementController : MonoBehaviour
{
    public float RotationAngle = 10f;
    public bool isNewInstance = false;
    public LayerMask surfaceMask;
    [HideInInspector] public Texture myIcon;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isNewInstance)
        {
            followMouse();
            rotate();
            placeDown();
            destroyNewInstance();
        }
    }

    void rotate()
    {
        Vector3 newRot = Vector3.up * Input.GetAxis("Horizontal");
        transform.Rotate(newRot, (RotationAngle * 3) * Time.deltaTime);
    }

    void followMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, surfaceMask))
        {
            if (hit.collider != null)
            {
                transform.position = hit.point;
            }
        }
    }

    void placeDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isNewInstance = false;
        }
    }

    void destroyNewInstance()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (isNewInstance)
                Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (UIElementManager.Instance.currentMode == MenuMode.BuildMode)
        {
            if (!isNewInstance)
            {
                if (UIElementManager.Instance.currentMode == MenuMode.BuildMode)
                {
                    SceneObjectsManager.Instance.selectedInstance = this;
                    UIElementManager.Instance.ActivateOrDeactivateBuildEditMenu();
                }
            }
        }

        if (UIElementManager.Instance.currentMode == MenuMode.ActionsMode)
        {
            Person isPerson;
            if (TryGetComponent<Person>(out isPerson))
            {
                UIElementManager.Instance.ActivateOrDeactivateActionOptionsMenu();
            }
        }
    }
}
