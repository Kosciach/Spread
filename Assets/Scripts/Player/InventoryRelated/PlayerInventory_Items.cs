using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory_Items : MonoBehaviour
{
    [Header("====Debugs====")]
    [SerializeField] List<ItemInventorySlot> _itemInventorySlots; public List<ItemInventorySlot> ItemInventorySlots { get { return _itemInventorySlots; } }


    private PlayerInventoryController _inventoryController;




    private void Awake()
    {
        _inventoryController = GetComponent<PlayerInventoryController>();

        for (int i = 0; i < 44; i++)
            AddSlot();
    }




    public void AddSlot()
    {
        _itemInventorySlots.Add(new ItemInventorySlot(CanvasController.Instance.PanelsControllers.Inventory.CreateUIItemSlot(_inventoryController)));
    }
    private int GetSmallestEmptyIndex()
    {
        for(int i=0; i< _itemInventorySlots.Count; i++)
            if (_itemInventorySlots[i].ItemData == null) return i;
        return -1;
    }

    public void AddItem(ItemData addedItemData, ItemDataHolder addedItemDataHolder)
    {
        int smallestIndex = GetSmallestEmptyIndex();
        if (smallestIndex < 0) return;

        _itemInventorySlots[smallestIndex].FillSlot(addedItemData);


        addedItemDataHolder.gameObject.layer = 0; 
        Destroy(addedItemDataHolder.gameObject);
    }
    public void DropItem(int itemToDropIndex)
    {
        Instantiate(_itemInventorySlots[itemToDropIndex].ItemData.ItemPrefab, _inventoryController.DropPoint.position, Quaternion.identity);
        _itemInventorySlots[itemToDropIndex].EmptySlot();
    }
}

[System.Serializable]
public class ItemInventorySlot
{
    public string Name;
    public bool Empty;
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

        ItemData = itemData;
        Empty = false;
    }
    public void EmptySlot()
    {
        Name = "EmptySlot";
        ItemIcon.color = new Color(1, 1, 1, 0);
        ItemIcon.sprite = null;

        ItemData = null;
        Empty = true;
    }
}