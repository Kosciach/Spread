using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerInventory_Items : MonoBehaviour
{
    [Header("====Debugs====")]
    [SerializeField] List<ItemInventorySlot> _itemInventorySlot;



    private void Awake()
    {
        for (int i = 0; i < 10; i++)
            AddSlot();
    }



    public void AddSlot()
    {
        _itemInventorySlot.Add(new ItemInventorySlot(CanvasController.Instance.PanelsControllers.Inventory.CreateUISlot()));
    }
    private int GetSmallestIndex()
    {
        for(int i=0; i< _itemInventorySlot.Count; i++)
        {
            if (_itemInventorySlot[i].ItemData == null) return i;
        }
        return -1;
    }

    public void AddItem(Item addedItemData, ItemDataHolder addedItemDataHolder)
    {
        int smallestIndex = GetSmallestIndex();
        if (smallestIndex < 0) return;

        _itemInventorySlot[smallestIndex].SetItemData(addedItemData);


        addedItemDataHolder.gameObject.layer = 0; 
        Destroy(addedItemDataHolder.gameObject);
    }
}

[System.Serializable]
public class ItemInventorySlot
{
    public string Name;
    public Item ItemData;
    public GameObject UISlot;

    public ItemInventorySlot(GameObject uiSlot)
    {
        Name = "EmptySlot";
        UISlot = uiSlot;
    }


    public void SetItemData(Item itemData)
    {
        Name = itemData.Name;
        ItemData = itemData;
    }
}