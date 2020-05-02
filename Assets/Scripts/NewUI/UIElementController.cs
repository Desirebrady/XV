using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIElementController : MonoBehaviour
{
    public float RotationAngle = 10f;
    public bool isNewInstance = false;
    public LayerMask surfaceMask;
    public RawImage myIcon;

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
        //Vector3 newRot = Vector3.up * Input.GetAxis("Horizontal");
        
        Vector3 newRot = Vector3.up;

        if (Input.GetKey(KeyCode.Q))
        {
            newRot = Vector3.up * -1;
            transform.Rotate(newRot, (RotationAngle * 3) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(newRot, (RotationAngle * 3) * Time.deltaTime);
        }
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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (GetComponent<Person>() != null)
                GetComponent<Person>().SetupStartingPos();

            isNewInstance = false;
        }
    }

    void destroyNewInstance()
    {
        if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (isNewInstance)
            {
                GameManager.Instance.moneySystem.AddMoney(gameObject.GetComponent<IBuyable>().GetPrice());
                Destroy(gameObject);
            }
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return ;

        if (UIElementManager.Instance.currentMode == MenuMode.BuildMode)
        {
            if (!isNewInstance)
            {
                if (UIElementManager.Instance.currentMode == MenuMode.BuildMode)
                {
                    SceneObjectsManager.Instance.selectedInstance = this;
                    
                    if (UIElementManager.Instance.isEditMenuActive("BuildEditMenu"))
                        return ;
                    
                    UIElementManager.Instance.ActivateOrDeactivateBuildEditMenu();
                }
            }
        }
        else
        {
            if (UIElementManager.Instance.isEditMenuActive("BuildEditMenu"))
                UIElementManager.Instance.ActivateOrDeactivateBuildEditMenu();
        }

        if (UIElementManager.Instance.currentMode == MenuMode.ActionsMode)
        {
            Person isPerson;
            if (TryGetComponent<Person>(out isPerson))
            {
                if (UIElementManager.Instance.isEditMenuActive("ActionsEditMenu"))
                    return ;
                UIElementManager.Instance.ActivateOrDeactivateActionOptionsMenu();
            }
            else
            {
                if (UIElementManager.Instance.isEditMenuActive("ActionsEditMenu"))
                    UIElementManager.Instance.ActivateOrDeactivateActionOptionsMenu();
            }
        }
        else
        {
            if (UIElementManager.Instance.isEditMenuActive("ActionsEditMenu"))
                UIElementManager.Instance.ActivateOrDeactivateActionOptionsMenu();
        }

    }
}
