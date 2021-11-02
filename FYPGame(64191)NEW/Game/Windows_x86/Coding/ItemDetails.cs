using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemDetails
{
    public int itemCode;
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public string itemDesc;
    public short itemUseRadiusGrid;
    public float itemUseRadius;
    public bool isStarting;
    public bool pickedUp;
    public bool dropped;
    public bool eatable;
    public bool carried;
}
