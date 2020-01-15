using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItems : MonoBehaviour, IPut
{
    public int maxStorage = 32;
    public List<Item> sellingItems = new List<Item>();
    [HideInInspector] public Outline outline;

    public void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public int sell(Item target)
    {
        int startlen = sellingItems.Count;
        for (int i = startlen - 1; i >= 0; i--)
        {
            if (sellingItems[i] == null)
                continue;
                
            if (sellingItems[i].type == target.type)
            {
                transform.GetChild(0).GetChild(sellingItems.Count - 1).gameObject.SetActive(false);
                sellingItems.RemoveAt(i);
            }
        }
        
        int endlen = sellingItems.Count;
        return startlen - endlen;
    }

    public bool put(Item item)
    {
        if (item == null)
            return false;

        sellingItems.Add(item);

        if (sellingItems.Count - 1 < transform.GetChild(0).childCount)
            transform.GetChild(0).GetChild(sellingItems.Count - 1).gameObject.SetActive(true);
        return true;
    }

    public bool CanTake(Item item)
    {
        return true;
    }

    public List<Ingredient> GetInputs()
    {
        return new List<Ingredient>();
    }
}