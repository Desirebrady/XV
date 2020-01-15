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
public class Item : MonoBehaviour
{
    public ItemType type = ItemType.Nothing;

    public ItemType GetItemType()
    {
        return type;
    }
}
