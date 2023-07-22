using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory_Items : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventoryController _inventory;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] List<ItemInventorySlot> _itemInventorySlots; public List<ItemInventorySlot> ItemInventorySlots { get { return _itemInventorySlots; } }



    private void Awake()
    {
        for (int i = 0; i < 2; i++)
            AddSlot();
    }




    public void AddSlot()
    {
        _itemInventorySlots.Add(new ItemInventorySlot(CanvasController.Instance.PanelsControllers.Inventory.CreateUIItemSlot(_inventory)));
    }
    private int GetSmallestSlotWithSpaceIndex(ItemData itemData)
    {
        for(int i=0; i< _itemInventorySlots.Count; i++)
            if (!_itemInventorySlots[i].MaxCountPerSlotReached && _itemInventorySlots[i].ItemData == itemData) return i;
        return -1;
    }


    public void AddItem(ItemData addedItemData, ItemDataHolder addedItemDataHolder)
    {
        Action<ItemData, ItemDataHolder> addItemMethod = addedItemData.Stackable ? AddStackable : AddNonStackable;
        addItemMethod(addedItemData, addedItemDataHolder);
    }
    private void AddStackable(ItemData addedItemData, ItemDataHolder addedItemDataHolder)
    {
        int smallestIndex = GetSmallestSlotWithSpaceIndex(addedItemData);
        if (smallestIndex < 0)
        {
            AddNonStackable(addedItemData, addedItemDataHolder);
            return;
        }

        ItemInventorySlot itemInventorySlot = _itemInventorySlots[smallestIndex];
        if (itemInventorySlot.Count + 1 > itemInventorySlot.ItemData.MaxCountPerSlot) return;
        itemInventorySlot.Count++;
        itemInventorySlot.MaxCountPerSlotReached = itemInventorySlot.Count == itemInventorySlot.ItemData.MaxCountPerSlot;
        itemInventorySlot.UIItemController.Count.UpdateCount(itemInventorySlot.Count);

        addedItemDataHolder.gameObject.layer = 0;
        Destroy(addedItemDataHolder.gameObject);
    }
    private void AddNonStackable(ItemData addedItemData, ItemDataHolder addedItemDataHolder)
    {
        int smallestIndex = GetSmallestSlotWithSpaceIndex(null);
        ItemInventorySlot itemInventorySlot = _itemInventorySlots[smallestIndex];
        if (smallestIndex < 0) return;

        itemInventorySlot.FillSlot(addedItemData);
        itemInventorySlot.MaxCountPerSlotReached = itemInventorySlot.ItemData.Stackable ? itemInventorySlot.MaxCountPerSlotReached : true;

        addedItemDataHolder.gameObject.layer = 0;
        Destroy(addedItemDataHolder.gameObject);
    }


    public void DropItem(int itemToDropIndex)
    {
        ItemInventorySlot itemInventorySlot = _itemInventorySlots[itemToDropIndex];
        if (itemInventorySlot.Empty) return;

        Instantiate(itemInventorySlot.ItemData.ItemPrefab, _inventory.DropPoint.position, Quaternion.identity);
        itemInventorySlot.MaxCountPerSlotReached = false;

        Action<ItemInventorySlot> dropItemMethod = itemInventorySlot.ItemData.Stackable ? DropStackable : DropNonStackable;
        dropItemMethod(itemInventorySlot);
    }
    private void DropStackable(ItemInventorySlot itemInventorySlot)
    {
        if (itemInventorySlot.Count > 1)
        {
            itemInventorySlot.Count--;
            itemInventorySlot.UIItemController.Count.UpdateCount(itemInventorySlot.Count);
        }
        else DropNonStackable(itemInventorySlot);
    }
    private void DropNonStackable(ItemInventorySlot itemInventorySlot)
    {
        itemInventorySlot.EmptySlot();
    }
}

[System.Serializable]
public class ItemInventorySlot
{
    public string Name;
    public bool Empty;
    public bool MaxCountPerSlotReached;
    public int Count;
    public ItemData ItemData;
    public UIItemController UIItemController;
    public Image ItemIcon;

    public ItemInventorySlot(UIItemController uiSlotController)
    {
        UIItemController = uiSlotController;
        ItemIcon = UIItemController.transform.GetComponent<Image>();

        EmptySlot();
    }


    public void FillSlot(ItemData itemData)
    {
        Name = itemData.ItemName;
        ItemIcon.sprite = itemData.Icon;
        ItemIcon.color = new Color(1, 1, 1, 1);
        Count = 1;

        ItemData = itemData;
        Empty = false;
    }
    public void EmptySlot()
    {
        Name = "EmptySlot";
        ItemIcon.color = new Color(1, 1, 1, 0);
        ItemIcon.sprite = null;
        Count = 0;

        ItemData = null;
        Empty = true;
    }
}