using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory_Throwables : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventoryController _inventory;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] List<ItemInventorySlot> _throwableInventorySlots; public List<ItemInventorySlot> ThrowableInventorySlots { get { return _throwableInventorySlots; } }



    private void Awake()
    {
        for (int i = 0; i < 2; i++)
            AddSlot();
    }




    public void AddSlot()
    {
        _throwableInventorySlots.Add(new ItemInventorySlot(CanvasController.Instance.PanelsControllers.Inventory.CreateUIThrowableSlot(_inventory)));
    }


    public void DropThrowable(int itemToDropIndex)
    {
        ItemInventorySlot itemInventorySlot = _throwableInventorySlots[itemToDropIndex];
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