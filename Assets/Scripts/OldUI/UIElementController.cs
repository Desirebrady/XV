using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementController : MonoBehaviour
{
    public float RotationAngle = 10f;
    [HideInInspector] public bool isNewInstance = false;
    [HideInInspector] public bool isDisplay = true;
    public LayerMask surfaceMask;
    public string editMenuName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDisplay)
            transform.Rotate(Vector3.up, RotationAngle * Time.deltaTime);
        if (isNewInstance)
        {
            followMouse();
            rotate();
            placeDown();
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

    void OnMouseDown()
    {
        if (!isDisplay && !isNewInstance)
        {
            if (MenuManager.instance.buildMode)
            {
                MenuManager.instance.selected = this;
                MenuManager.instance.DeOrActivateSpecificMenu(editMenuName);
            }
        }
    }
}
