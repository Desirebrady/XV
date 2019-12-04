using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementManager : MonoBehaviour
{
    public UIElement[] UIElements;
    public LayerMask UIElementMask;
    public GameObject UICamera;

    // Start is called before the first frame update
    void Start()
    {
        //UICamera = GetComponentInChildren<Camera>();
        foreach (var element in UIElements)
        {
            var instance = Instantiate(element.prefab, transform);
            var camInstance = Instantiate(UICamera, instance.transform);
            element.GenerateIcon(UICamera.GetComponent<Camera>(), instance.transform);
            //instance.transform.GetChild(0).SetParent(this.transform);
            Destroy(instance);
        }
        //Destroy(UICamera.gameObject);
    }
}
