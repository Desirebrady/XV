﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public string buildModeMenuName = "BUILD MODE";
    public MenuOptionComponents[] menuComponents;
    [HideInInspector] public UIElementController selected;

    [HideInInspector] public bool buildMode = false;

    void Awake()
    {
        if (instance != this)
        {
            GameObject.Destroy(instance);
            instance = null;
        }

        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeOrActivateMenuComponents(string name)
    {
        foreach (var mc in menuComponents)
        {
            if (mc.name != name)
                mc.DeactivateComponents();
        }

        foreach (var mc in menuComponents)
        {
            if (mc.name == name)
            {
                if (mc.name.Contains(buildModeMenuName))
                    buildMode = !buildMode;

                foreach (GameObject c in mc.components)
                {
                    bool set = !c.activeSelf;
                    c.SetActive(set);
                }
                mc.isActive = true;
                break;
            }
        }
    }

    public void DeOrActivateSpecificMenu(string name)
    {
        foreach (var mc in menuComponents)
        {
            if (mc.name == name)
            {
                if (mc.name.Contains(buildModeMenuName))
                    buildMode = !buildMode;

                foreach (GameObject c in mc.components)
                {
                    bool set = !c.activeSelf;
                    c.SetActive(set);
                }
                mc.isActive = !mc.isActive;
                break;
            }
        }
    }

    public void RemoveSelected()
    {
        if (selected != null)
        {
            Destroy(selected.gameObject);
        }
    }

    public void TransformSelected()
    {
        if (selected != null)
        {
            selected.isNewInstance = true;
        }
    }
}

[System.Serializable]
public class MenuOptionComponents
{
    public string name;
    [HideInInspector]public bool isActive = false;
    public GameObject[] components;

    public void DeactivateComponents()
    {
        foreach (GameObject c in components)
        {
            c.SetActive(false);
        }
        isActive = false;
    }
}
