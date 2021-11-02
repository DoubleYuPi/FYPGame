using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    [SerializeField] private Sprite blankSprite = null;
    [SerializeField] private InventorySlot[] inventorySlots = null;
    [HideInInspector] public GameObject inventoryTextBoxGameObject;


    public GameObject inventoryBarDraggedItem;
    private void OnEnable()
    {
        EventHandle.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void OnDisable()
    {
        EventHandle.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (inventoryLocation == InventoryLocation.player)
        {
            ClearInventorySlots();

            if (inventorySlots.Length > 0 && inventoryList.Count > 0)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (i < inventoryList.Count)
                    {
                        int itemCode = inventoryList[i].itemCode;

                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if (itemDetails != null)
                        {
                            inventorySlots[i].inventorySlotImage.sprite = itemDetails.itemIcon;
                            inventorySlots[i].textMeshPro.text = inventoryList[i].itemQuantity.ToString();
                            inventorySlots[i].itemDetails = itemDetails;
                            inventorySlots[i].itemQuantity = inventoryList[i].itemQuantity;
                            SetHighLight(i);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void SetHighLight()
    {
        if (inventorySlots.Length > 0)
        {
            for(int i = 0; i < inventorySlots.Length; i++)
            {
                SetHighLight(i);
            }
        }
    }

    public void SetHighLight(int itemPosition)
    {
        if(inventorySlots.Length>0 &&inventorySlots[itemPosition].itemDetails != null)
        {
            if (inventorySlots[itemPosition].isSelected)
            {
                inventorySlots[itemPosition].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f);

                InventoryManager.Instance.SetSelectedInvenItem(InventoryLocation.player, inventorySlots[itemPosition].itemDetails.itemCode);
            }
        } 
    }

    public void ClearHighLight()
    {
        if(inventorySlots.Length > 0)
        {
            for(int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].isSelected)
                {
                    inventorySlots[i].isSelected = false;
                    inventorySlots[i].inventorySlotHighlight.color = new Color(0f, 0f, 0f, 0f);

                    InventoryManager.Instance.ClearSelectItem(InventoryLocation.player);
                }
            }
        }
    }

    private void ClearInventorySlots()
    {
        if (inventorySlots.Length > 0)
        {
            for(int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].inventorySlotImage.sprite = blankSprite;
                inventorySlots[i].textMeshPro.text = "";
                inventorySlots[i].itemDetails = null;
                inventorySlots[i].itemQuantity = 0;
                SetHighLight(i);
            }
        }
    }
}
