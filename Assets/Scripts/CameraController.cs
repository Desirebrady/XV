using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    Vector2 rotation = new Vector2(0, 0);

    public float rotateSpeed = 1f;
    public float panSpeed = 1f;
    public float scrollSpeed = 10f;
    public Vector3 offset = Vector3.zero;
    public Vector3 rotationOffset = Vector3.zero;

    public float defaultFOV = 60;
    public float minFOV = 20;
    public float maxFOV = 100;

    private PanDirection pan = PanDirection.NONE;
    private PanDirection previousPan = PanDirection.NONE;
    private float panLerp = 0;
    private Camera cam;

    void Awake()
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
        cam = Camera.main;
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

    public void OffsetCamera()
    {
        if (cam == null)
            cam = Camera.main;

        cam.transform.localPosition += offset;
        cam.transform.localRotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
    }

    void ZoomCamera()
    {
        var cam = GetComponentInChildren<Camera>();
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cam.fieldOfView++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cam.fieldOfView--;
        }

        if (Input.GetKeyDown(KeyCode.R))
            cam.fieldOfView = defaultFOV;

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFOV, maxFOV);
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
        previousPan = pan;

        if (Input.mousePosition.x <= 0)
        {
            //offset.x -= panSpeed * Time.deltaTime;
            offset -= transform.right * (panSpeed * panLerp) * Time.deltaTime;
            pan = PanDirection.LEFT;
        }
        else if (Input.mousePosition.x >= Screen.width)
        {
            //offset.x += panSpeed * Time.deltaTime;
            offset += transform.right * (panSpeed * panLerp) * Time.deltaTime;
            pan = PanDirection.RIGHT;
        }
        else if (Input.mousePosition.y <= 0)
        {
            //offset.z -= panSpeed * Time.deltaTime;
            offset -= transform.forward * (panSpeed * panLerp) * Time.deltaTime;
            pan = PanDirection.FORWARD;
        }
        else if (Input.mousePosition.y >= Screen.height)
        {
            //offset.z += panSpeed * Time.deltaTime;
            offset += transform.forward * (panSpeed * panLerp) * Time.deltaTime;
            pan = PanDirection.BACKWARD;
        }

        if (previousPan != pan)
        {
            panLerp = 0.01f;
        }

        panLerp += 0.01f;
        panLerp = Mathf.Clamp(panLerp, 0, 1);

        transform.position = offset;    
    }
}

public enum PanDirection
{
    NONE,
    RIGHT,
    LEFT,
    FORWARD,
    BACKWARD
}
