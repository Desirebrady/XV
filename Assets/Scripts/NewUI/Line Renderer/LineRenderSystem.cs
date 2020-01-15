using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderSystem : MonoBehaviour
{
    #region Singleton Access
    private static LineRenderSystem instance;//Use of a singleton here, needs to be static in order for other scripts to access it.

    public static LineRenderSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LineRenderSystem>();
            }

            return instance;
        }
    }
    #endregion

    public LineRenderer lineRenderer;

    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
}
