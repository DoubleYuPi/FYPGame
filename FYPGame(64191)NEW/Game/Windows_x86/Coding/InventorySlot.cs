using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    private Camera mainCamera;
    private Canvas parentCanvas;
    private Transform parentItem;
    private GameObject draggedItem;
    private GridCursor gridCursor;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshPro;

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    [SerializeField] private InventoryBar inventory = null;
    [SerializeField] private GameObject itemPrefab = null;

    [SerializeField] private int slotNumber = 0;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;

    [HideInInspector]public bool isSelected = false;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();    
    }

    private void OnEnable()
    {
        EventHandle.AfterSceneLoadEvent += SceneLoaded;
        EventHandle.RemoveSelectedItemFromIinventory += RemoveSelectedItem;
        EventHandle.DropSelectedItemEvent += DropSelectedItem;

    }

    private void OnDisable()
    {
        EventHandle.AfterSceneLoadEvent -= SceneLoaded;
        EventHandle.RemoveSelectedItemFromIinventory -= RemoveSelectedItem;
        EventHandle.DropSelectedItemEvent -= DropSelectedItem;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        gridCursor = FindObjectOfType<GridCursor>();
    }

    private void ClearCursor()
    {
        gridCursor.DisableCursor();

        gridCursor.SelectedItemType = ItemType.none;
    }

    private void SetSelectedItem()
    {
        inventory.ClearHighLight();

        isSelected = true;

        inventory.SetHighLight();

        InventoryManager.Instance.ClearSelectItem(InventoryLocation.player);

        gridCursor.ItemUseGridRadius = itemDetails.itemUseRadiusGrid;

        if (itemDetails.itemUseRadiusGrid > 0)
        {
            gridCursor.EnableCursor();
        }
        else
        {
            gridCursor.DisableCursor();
        }

        gridCursor.SelectedItemType = itemDetails.itemType;

        if(itemDetails.carried == true)
        {
            PlayerMovement.Instance.ShowCarriedItem(itemDetails.itemCode);
        }
        else
        {
            PlayerMovement.Instance.ClearCarriedItem();
        }
    }

    private void ClearSelectedItem()
    {
        inventory.ClearHighLight();

        isSelected = false;

        InventoryManager.Instance.ClearSelectItem(InventoryLocation.player);

        PlayerMovement.Instance.ClearCarriedItem();

        ClearCursor();
    }

    private void DropSelectedItem()
    {
        if(itemDetails != null && isSelected)
        {
            if (gridCursor.CursorPositionIsValid)
            {

                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

                //GameObject itemGameObject = Instantiate(itemPrefab, new Vector3 (worldPosition.x,worldPosition.y - Settings.gridCellSize/2f, worldPosition.z), Quaternion.identity, parentItem);
                GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
                Items items = itemGameObject.GetComponent<Items>();
                items.ItemCode = itemDetails.itemCode;

                    InventoryManager.Instance.RemoveItem(InventoryLocation.player,items.ItemCode);
            
                    if(InventoryManager.Instance.FindInventoryItem(InventoryLocation.player, items.ItemCode) == -1)
                    {
                        ClearSelectedItem();
                    }
            }

            

        }
    }

    private void RemoveSelectedItem()
    {
        if(itemDetails != null && isSelected)
        {
            int itemCode = itemDetails.itemCode;

            InventoryManager.Instance.RemoveItem(InventoryLocation.player, itemCode);

            if (InventoryManager.Instance.FindInventoryItem(InventoryLocation.player, itemCode) == -1)
            {
                ClearSelectedItem();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            draggedItem = Instantiate(inventory.inventoryBarDraggedItem, inventory.transform);

            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

            SetSelectedItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            if(eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null)
            {
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>().slotNumber;

                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                DestroyInventoryTextBox();

                ClearSelectedItem();
            }
            else
            {
                if (itemDetails.dropped)
                {
                    DropSelectedItem();
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (isSelected == true)
            {
                ClearSelectedItem();
            }
            else
            {
                if (itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemQuantity != 0)
        {
            inventory.inventoryTextBoxGameObject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventory.inventoryTextBoxGameObject.transform.SetParent(parentCanvas.transform, false);

            InventoryTextBox inventoryTextBox = inventory.inventoryTextBoxGameObject.GetComponent<InventoryTextBox>();

            string itemTypeDesc = InventoryManager.Instance.GetItemTypeDesc(itemDetails.itemType);

            inventoryTextBox.SetBoxText(itemDetails.itemName, itemTypeDesc, "", itemDetails.itemDesc, "", "");

            inventory.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.55f);
            inventory.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    public void DestroyInventoryTextBox()
    {
        if (inventory.inventoryTextBoxGameObject != null)
        {
            Destroy(inventory.inventoryTextBoxGameObject);
        }
    }

    public void SceneLoaded()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTrans).transform;
    }
}
