using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 rotation = new Vector2(0, 0);

    public float rotateSpeed = 1f;
    public float panSpeed = 1f;
    public float scrollSpeed = 10f;
    public Vector3 offset = Vector3.zero;

    public Vector2 CameraZoomMinMax = new Vector2(2, 15);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetMouseButton(2))
            RotateCamera();
        else
        {
            PanCamera();
            ZoomCamera();
        }
    }

    void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Camera.main.transform.position.y >= CameraZoomMinMax.x && Camera.main.transform.position.y <= CameraZoomMinMax.y)
                offset += Camera.main.transform.forward * (Input.mouseScrollDelta.y * scrollSpeed) * Time.deltaTime;
        }

        transform.localPosition = offset;    
    }

    void RotateCamera()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");

        //Clamp
        rotation.x = Mathf.Clamp(rotation.x, 20f, 75f);

        //transform.eulerAngles = new Vector2(0, rotation.y) * rotateSpeed;
        Quaternion rot = Quaternion.Euler(rotation.x * rotateSpeed, rotation.y * rotateSpeed, 0);

        Camera.main.transform.localRotation = Quaternion.Euler(rotation.x * rotateSpeed, 0, 0);
        transform.localRotation = Quaternion.Euler(0, rotation.y * rotateSpeed, 0);
    }

    void PanCamera()
    {
        Vector3 cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position);

        if (Input.mousePosition.x <= 0)
        {
            //offset.x -= panSpeed * Time.deltaTime;
            offset -= transform.right * panSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x >= Screen.width)
        {
            //offset.x += panSpeed * Time.deltaTime;
            offset += transform.right * panSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= 0)
        {
            //offset.z -= panSpeed * Time.deltaTime;
            offset -= transform.forward * panSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.y >= Screen.height)
        {
            //offset.z += panSpeed * Time.deltaTime;
            offset += transform.forward * panSpeed * Time.deltaTime;
        }

        transform.position = offset;    
    }
}
