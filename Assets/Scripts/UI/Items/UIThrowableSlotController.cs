using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIThrowableSlotController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount <= 0) return;

        UIItemController droppedUIItemController = eventData.pointerDrag.GetComponent<UIItemController>();
        UIItemController childUIItemController = transform.GetChild(0).GetComponent<UIItemController>();
        PlayerInventoryController playerInventoryController = childUIItemController.PlayerInventory;

        if (droppedUIItemController == null) return;
        if (droppedUIItemController.HomeParent == null) return;

        Action<UIItemController, UIItemController, PlayerInventoryController> movedFromMethod = droppedUIItemController.UIOrigin == UIItemController.UIOrigins.Throwables ? MovedFromTheSameSlotType : MovedFromDiffrentSlotType;
        movedFromMethod(droppedUIItemController, childUIItemController, playerInventoryController);

        droppedUIItemController = null;
        childUIItemController = null;
        playerInventoryController = null;
    }


    private void MovedFromTheSameSlotType(UIItemController droppedUIItemController, UIItemController childUIItemController, PlayerInventoryController playerInventoryController)
    {
        ItemInventorySlot droppedInventorySlot = playerInventoryController.Throwables.ThrowableInventorySlots[droppedUIItemController.IndexInInventory];
        ItemInventorySlot childrenInventorySlot = playerInventoryController.Throwables.ThrowableInventorySlots[childUIItemController.IndexInInventory];

        if (droppedInventorySlot.ItemData == childrenInventorySlot.ItemData && droppedInventorySlot.ItemData.Stackable && !childrenInventorySlot.MaxCountPerSlotReached)
        {
            int spaceLeftInSlot = droppedInventorySlot.ItemData.MaxCountPerSlot - childrenInventorySlot.Count;
            bool shouldEmptyDroppedSlot = spaceLeftInSlot >= droppedInventorySlot.Count;

            int itemCountToAdd = droppedInventorySlot.Count;
            itemCountToAdd = Mathf.Clamp(itemCountToAdd, itemCountToAdd, spaceLeftInSlot);

            childrenInventorySlot.Count += itemCountToAdd;
            droppedInventorySlot.Count -= itemCountToAdd;

            childrenInventorySlot.UIItemController.Count.UpdateCount(childrenInventorySlot.Count);
            droppedInventorySlot.UIItemController.Count.UpdateCount(droppedInventorySlot.Count);

            childrenInventorySlot.MaxCountPerSlotReached = childrenInventorySlot.Count == childrenInventorySlot.ItemData.MaxCountPerSlot;
            droppedInventorySlot.MaxCountPerSlotReached = false;

            if (!shouldEmptyDroppedSlot) return;

            droppedInventorySlot.EmptySlot();
            return;
        }

        MoveInUI(droppedUIItemController, childUIItemController, playerInventoryController);

        ItemInventorySlot tempItemInventorySlot = playerInventoryController.Throwables.ThrowableInventorySlots[childUIItemController.IndexInInventory];
        playerInventoryController.Throwables.ThrowableInventorySlots[childUIItemController.IndexInInventory] = playerInventoryController.Throwables.ThrowableInventorySlots[droppedUIItemController.IndexInInventory];
        playerInventoryController.Throwables.ThrowableInventorySlots[droppedUIItemController.IndexInInventory] = tempItemInventorySlot;
    }
    private void MovedFromDiffrentSlotType(UIItemController droppedUIItemController, UIItemController childUIItemController, PlayerInventoryController playerInventoryController)
    {
        if (playerInventoryController.Item.ItemInventorySlots[droppedUIItemController.IndexInInventory].ItemData is not ThrowableData) return;

        MoveInUI(droppedUIItemController, childUIItemController, playerInventoryController);

        ItemInventorySlot tempItemInventorySlot = playerInventoryController.Item.ItemInventorySlots[childUIItemController.IndexInInventory];
        playerInventoryController.Item.ItemInventorySlots[childUIItemController.IndexInInventory] = playerInventoryController.Throwables.ThrowableInventorySlots[droppedUIItemController.IndexInInventory];
        playerInventoryController.Throwables.ThrowableInventorySlots[droppedUIItemController.IndexInInventory] = tempItemInventorySlot;
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