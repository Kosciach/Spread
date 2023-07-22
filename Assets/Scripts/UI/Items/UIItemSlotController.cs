using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemSlotController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount <= 0) return;

        UIItemController droppedUIItemController = eventData.pointerDrag.GetComponent<UIItemController>();
        UIItemController childUIItemController = transform.GetChild(0).GetComponent<UIItemController>();
        PlayerInventoryController playerInventoryController = childUIItemController.PlayerInventory;

        if (droppedUIItemController == null) return;
        if (droppedUIItemController.HomeParent == null) return;


        MoveInUI(droppedUIItemController, childUIItemController, playerInventoryController);


        Action<UIItemController, UIItemController, PlayerInventoryController> movedFromMethod = droppedUIItemController.UIOrigin == UIItemController.UIOrigins.Items ? MovedFromTheSameSlotType : MovedFromDiffrentSlotType;
        movedFromMethod(droppedUIItemController, childUIItemController, playerInventoryController);

        droppedUIItemController = null;
        childUIItemController = null;
        playerInventoryController = null;
    }


    private void MovedFromTheSameSlotType(UIItemController droppedUIItemController, UIItemController childUIItemController, PlayerInventoryController playerInventoryController)
    {
        ItemInventorySlot tempItemInventorySlot = playerInventoryController.Item.ItemInventorySlots[childUIItemController.IndexInInventory];
        playerInventoryController.Item.ItemInventorySlots[childUIItemController.IndexInInventory] = playerInventoryController.Item.ItemInventorySlots[droppedUIItemController.IndexInInventory];
        playerInventoryController.Item.ItemInventorySlots[droppedUIItemController.IndexInInventory] = tempItemInventorySlot;
    }
    private void MovedFromDiffrentSlotType(UIItemController droppedUIItemController, UIItemController childUIItemController, PlayerInventoryController playerInventoryController)
    {
        ItemInventorySlot tempItemInventorySlot = playerInventoryController.Throwables.ThrowableInventorySlots[childUIItemController.IndexInInventory];
        playerInventoryController.Throwables.ThrowableInventorySlots[childUIItemController.IndexInInventory] = playerInventoryController.Item.ItemInventorySlots[droppedUIItemController.IndexInInventory];
        playerInventoryController.Item.ItemInventorySlots[droppedUIItemController.IndexInInventory] = tempItemInventorySlot;
    }


    private void MoveInUI(UIItemController droppedUIItemController, UIItemController childUIItemController, PlayerInventoryController playerInventoryController)
    {
        childUIItemController.HomeParent = droppedUIItemController.HomeParent;
        droppedUIItemController.HomeParent = transform;
        childUIItemController.transform.SetParent(childUIItemController.HomeParent);

        childUIItemController.IndexInInventory = childUIItemController.HomeParent.GetSiblingIndex();
        droppedUIItemController.IndexInInventory = droppedUIItemController.HomeParent.GetSiblingIndex();
    }
}