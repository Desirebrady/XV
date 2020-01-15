using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSetup : MonoBehaviour
{
    private RawImage image;
    [HideInInspector]public RenderTexture rendTexture;
    private Texture2D myTexture;
    public LayerMask surfaceMask;

    [HideInInspector]public int radius = 2;
    [HideInInspector]public int textureWidth = 256;
    [HideInInspector]public int textureHeight = 256;
    [HideInInspector]public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetIcon()
    {
        image = GetComponent<RawImage>();
        myTexture = toTexture2D(rendTexture);
        Texture2D temp = CalculateTexture(  myTexture.height, myTexture.width, 
                                            myTexture.height / radius,
                                            myTexture.height / radius,
                                            myTexture.width / radius, myTexture 
                                          );
        image.texture = temp;
    }
 
 
    Texture2D CalculateTexture(int h, int w, float r, float cx, float cy, Texture2D sourceTex)
    {
        Color[] c = sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
        Texture2D b = new Texture2D(h, w);
        for (int i = 0; i < (h * w); i++)
        {
            int y = Mathf.FloorToInt(((float)i) / ((float)w));
            int x = Mathf.FloorToInt(((float)i - ((float)(y * w))));
            if (r * r >= (x - cx) * (x - cx) + (y - cy) * (y - cy))
            {
                b.SetPixel(x, y, c[i]);
            }
            else
            {
                b.SetPixel(x, y, Color.clear);
            }
        }
        b.Apply();
        return b;
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public void onClick()
    {
        var instance = Instantiate(prefab, UIElementManager.Instance.newInstanceParent);
        instance.gameObject.layer =  LayermaskToLayer(UIElementManager.Instance.SceneElementMask);
        var controller = instance.AddComponent<UIElementController>();
        controller.isNewInstance = true;
        controller.myIcon = image.texture;
        controller.surfaceMask = surfaceMask;
    }

    public int LayermaskToLayer(LayerMask layerMask) 
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while(layer > 0) 
        {
            layer = layer >> 1;
            layerNumber++;
        }
        
        return layerNumber - 1;
     }
}
