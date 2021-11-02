using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : PowerUpItem
{
    [ItemsCodeDescAtr]
    [SerializeField]
    private int _itemCode;

    private SpriteRenderer spriterender;

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    private void Awake()
    {
        spriterender = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if(ItemCode != 0)
        {
            Init(ItemCode);
        }
    }

    public void Init (int itemCodeParam)
    {
        if(itemCodeParam != 0)
        {
            ItemCode = itemCodeParam;

            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);

            spriterender.sprite = itemDetails.itemIcon;

            if (itemDetails.itemType == ItemType.Destroyable_object)
            {
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }
}
