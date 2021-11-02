using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    public List<InventoryItem>[] inventoryLists;

    [HideInInspector] public int[] inventoryListCapacityArray;

    [SerializeField] private ItemList itemList = null;

    private int[] selectedInvenItem;


    protected override void Awake()
    {
        base.Awake();

        CreateInventoryList();

        CreateItemDetailsDictionary();

        selectedInvenItem = new int[(int)InventoryLocation.count];

        for (int i = 0; i<selectedInvenItem.Length; i++)
        {
            selectedInvenItem[i] = -1;
        }

    }

    public void CreateInventoryList()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i=0; i<(int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacityArray = new int[(int)InventoryLocation.count];
        inventoryListCapacityArray[(int)InventoryLocation.player] = Settings.InventoryCapacity;
    }

    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    public void AddItem(InventoryLocation inventoryLocation, Items item, GameObject gameObjectDestroy)
    {
        AddItem(inventoryLocation, item);

        Destroy(gameObjectDestroy);
    }

    public void AddItem(InventoryLocation inventoryLocation, Items item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        int itemPosition = FindInventoryItem(inventoryLocation, itemCode);

        if(itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }
        EventHandle.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = quantity;
        inventoryList[position] = inventoryItem;
    }

    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromtItem,int toItem)
    {
        if(fromtItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count && fromtItem!= toItem && fromtItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventory = inventoryLists[(int)inventoryLocation][fromtItem];
            InventoryItem toInventory = inventoryLists[(int)inventoryLocation][toItem];

            inventoryLists[(int)inventoryLocation][toItem] = fromInventory;
            inventoryLists[(int)inventoryLocation][fromtItem] = toInventory;

            EventHandle.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }

    public void ClearSelectItem(InventoryLocation inventoryLocation)
    {
        selectedInvenItem[(int)inventoryLocation] = -1;
    }

    public int FindInventoryItem(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for(int i = 0; i < inventoryList.Count; i++)
        {
            if(inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        }

        return -1;
    }

    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode,out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }
    }

    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation inventoryLocation)
    {
        int itemCode = GetSelectedInventoryItem(inventoryLocation);

        if(itemCode == -1)
        {
            return null;
        }else
        {
            return GetItemDetails(itemCode);
        }
    }
    
    private int GetSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        return selectedInvenItem[(int)inventoryLocation];
    }

    public string GetItemTypeDesc(ItemType itemType)
    {
        string itemTypeDesc;
        switch (itemType)
        {
            case ItemType.Hoeing_Tool:
                itemTypeDesc = Settings.HoeingTool;
                break;
            case ItemType.Watering_Tool:
                itemTypeDesc = Settings.WateringTool;
                break;
            case ItemType.Slaying_Tool:
                itemTypeDesc = Settings.SlayingTool;
                break;
            case ItemType.Pickup_Tool:
                itemTypeDesc = Settings.PickupTool;
                break;
            case ItemType.Choping_Tool:
                itemTypeDesc = Settings.ChopingTool;
                break;
            default:
                itemTypeDesc = itemType.ToString();
                break;
        }
        return itemTypeDesc;
    }

    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindInventoryItem(inventoryLocation, itemCode);

        if (itemPosition != -2)
        {
            RemoveItemAtPosition(inventoryList,itemCode,itemPosition);
        }

        EventHandle.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }


    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity - 1;

        if (quantity > 0)
        {
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[position] = inventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(position);
        }
    }

    public void SetSelectedInvenItem(InventoryLocation inventoryLocation, int itemCode)
    {
        selectedInvenItem[(int)inventoryLocation] = itemCode;
    }
}
