using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory_Items : MonoBehaviour
{
    [Header("====Debugs====")]
    [SerializeField] List<ItemInventorySlot> _itemInventorySlots;



    private void Awake()
    {
        for (int i = 0; i < 10; i++)
            AddSlot();
    }



    public void AddSlot()
    {
        _itemInventorySlots.Add(new ItemInventorySlot(CanvasController.Instance.PanelsControllers.Inventory.CreateUIItemSlot()));
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
    public void DropItem()
    {

    }
}

[System.Serializable]
public class ItemInventorySlot
{
    public string Name;
    public ItemData ItemData;
    public GameObject UISlot;
    public Image ItemIcon;

    public ItemInventorySlot(GameObject uiSlot)
    {
        UISlot = uiSlot;
        ItemIcon = UISlot.transform.GetChild(0).GetComponent<Image>();

        EmptySlot();
    }


    public void FillSlot(ItemData itemData)
    {
        Name = itemData.ItemName;
        ItemIcon.sprite = itemData.Icon;
        ItemIcon.color = new Color(1, 1, 1, 1);

        ItemData = itemData;
    }
    public void EmptySlot()
    {
        Name = "EmptySlot";
        ItemIcon.color = new Color(1, 1, 1, 0);
        ItemIcon.sprite = null;

        ItemData = null;
    }
}