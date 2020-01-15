using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public LayerMask floor;
    public float zoomLevel = 0.0f;
    public float cameraMoveSpeed = 10.0f;
    public float cameraRotSpeed = 10.0f;
    Quaternion rot;
    // Start is called before the first frame update
    void Start()
    {
        rot = transform.rotation;
        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        EdgeScroll();
        ZoomAndRotate();
        FocusCamera();
        KeyChangedEvents();
        KeyHeldEvents();
    }

    private void OnDrawGizmos()
    {
    }

    void EdgeScroll()
    {
        if (/*Input.mousePosition.x <= 0.0f ||*/ Input.GetKey(KeyCode.A))
        {
            transform.position += (transform.rotation * Vector3.left) * Time.deltaTime * cameraMoveSpeed * (zoomLevel + 1);
        }
        if (/*Input.mousePosition.y <= 0.0f ||*/ Input.GetKey(KeyCode.S))
        {
            transform.position += (transform.rotation * Vector3.back) * Time.deltaTime * cameraMoveSpeed * (zoomLevel + 1);
        }
        if (/*Input.mousePosition.x >= Screen.width - 1 ||*/ Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.rotation * Vector3.right) * Time.deltaTime * cameraMoveSpeed * (zoomLevel + 1);
        }
        if (/*Input.mousePosition.y >= Screen.height - 1 ||*/ Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.rotation * Vector3.forward) * Time.deltaTime * cameraMoveSpeed * (zoomLevel + 1);
        }
    }
    void ZoomAndRotate()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rot = rot * Quaternion.Euler(0, 20 * Input.mouseScrollDelta.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * cameraRotSpeed);
        }
        else
        {
            zoomLevel -= Input.mouseScrollDelta.y / 10.0f;
            zoomLevel = Mathf.Clamp(zoomLevel, 0.0f, 1.0f);
        }
    }
    void FocusCamera()
    {
        //if (transform.position.x < 100f)
        //    transform.position = new Vector3(100f, transform.position.y, transform.position.z);
        //if (transform.position.x > 924f)
        //    transform.position = new Vector3(924f, transform.position.y, transform.position.z);
        //if (transform.position.z < 100f)
        //    transform.position = new Vector3(transform.position.x, transform.position.y, 100f);
        //if (transform.position.z > 924f)
        //    transform.position = new Vector3(transform.position.x, transform.position.y, 924f);
        Camera.main.transform.LookAt(transform);
        Camera.main.transform.localPosition = Vector3.Slerp(Camera.main.transform.localPosition, new Vector3(0.0f, zoomLevel * 23.0f + 2.0f, -zoomLevel * 13.0f - 2.0f), Time.deltaTime * 2.0f);
        transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
    }
    void KeyChangedEvents()
    {
    }
    void KeyHeldEvents()
    {
    }
}