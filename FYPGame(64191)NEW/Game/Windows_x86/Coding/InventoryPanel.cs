using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] ItemContainer inventory;
    [SerializeField] List<InventoryButton> button;

    private void Start()
    {
        SetIndex();
        Show();
    }

    private void SetIndex()
    {
        for(int i = 0; i < inventory.slots.Count; i++)
        {
            button[i].SetIndex(i);
        }
    }

    private void Show()
    {
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            if(inventory.slots[i].item == null)
            {
                button[i].Clean();
            }
            else
            {
                button[i].Set(inventory.slots[i]);
            }
        }
    }
}
