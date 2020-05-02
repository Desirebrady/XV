using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Nothing,
    PackedDocuments,
    Document,
    Iron,
    Aluminium,
    Copper,
    Gold,
    Bullets,
    Guns,
    MetalSheets,
    AmmoBox
}

[System.Serializable]
public class Item : MonoBehaviour, IBuyable
{
    public int ItemID;
    public ItemType type = ItemType.Nothing;
    public float SellingPrice = 100.0f;

    public float GetPrice()
    {
        return SellingPrice;
    }

    public ItemType GetItemType()
    {
        return type;
    }
}
