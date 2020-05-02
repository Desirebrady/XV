using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSetup : MonoBehaviour
{
    private RawImage image;
    [HideInInspector] public RenderTexture rendTexture;
    [HideInInspector] public Texture2D myTexture;
    public LayerMask surfaceMask;

    [HideInInspector]public int radius = 2;
    [HideInInspector]public int textureWidth = 256;
    [HideInInspector]public int textureHeight = 256;
    [HideInInspector]public GameObject prefab;

    public void SetIcon()
    {
        image = GetComponent<RawImage>();

        if (myTexture == null && rendTexture != null)
        {
            myTexture = TextureTools.toTexture2D(rendTexture, textureWidth, textureHeight);
            Texture2D temp = TextureTools.CalculateTexture(  myTexture.height, myTexture.width, 
                                                myTexture.height / radius,
                                                myTexture.height / radius,
                                                myTexture.width / radius, myTexture 
                                            );
        }

        image.texture = myTexture;
    }
 
 




    public void onClick()
    {
        //Create Instance
        var instance = Instantiate(prefab, UIElementManager.Instance.newInstanceParent);
        instance.gameObject.layer =  LayermaskToLayer(UIElementManager.Instance.SceneElementMask);

        UIElementController test;

        if (!instance.TryGetComponent<UIElementController>(out test))
        {
            var controller = instance.AddComponent<UIElementController>();
            controller.isNewInstance = true;
            controller.myIcon = image;
            controller.surfaceMask = surfaceMask;
        }
        else
        {
            test.isNewInstance = true;
            test.myIcon = image;
            test.surfaceMask = surfaceMask;
        }

        GameManager.Instance.moneySystem.RemoveMoney(instance.GetComponent<IBuyable>().GetPrice());
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

public static class TextureTools
{
    public static Texture2D toTexture2D(RenderTexture rTex, int textureWidth, int textureHeight)
    {
        Texture2D tex = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public static Texture2D CalculateTexture(int h, int w, float r, float cx, float cy, Texture2D sourceTex)
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
}
