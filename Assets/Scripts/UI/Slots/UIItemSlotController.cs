using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemSlotController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        UIItemController droppedUIItemController = eventData.pointerDrag.GetComponent<UIItemController>();
        UIItemController childUIItemController = transform.GetChild(0).GetComponent<UIItemController>();
        PlayerInventoryController playerInventoryController = childUIItemController.PlayerInventory;

        if(droppedUIItemController.HomeParent == null) return;


        childUIItemController.HomeParent = droppedUIItemController.HomeParent;
        droppedUIItemController.HomeParent = transform;
        childUIItemController.transform.parent = childUIItemController.HomeParent;

        childUIItemController.IndexInInventory = childUIItemController.HomeParent.GetSiblingIndex();
        droppedUIItemController.IndexInInventory = droppedUIItemController.HomeParent.GetSiblingIndex();

        ItemInventorySlot tempItemInventorySlot = playerInventoryController.Item.ItemInventorySlots[childUIItemController.IndexInInventory];
        playerInventoryController.Item.ItemInventorySlots[childUIItemController.IndexInInventory] = playerInventoryController.Item.ItemInventorySlots[droppedUIItemController.IndexInInventory];
        playerInventoryController.Item.ItemInventorySlots[droppedUIItemController.IndexInInventory] = tempItemInventorySlot;

        droppedUIItemController = null;
        childUIItemController = null;
        playerInventoryController = null;
    }
}
