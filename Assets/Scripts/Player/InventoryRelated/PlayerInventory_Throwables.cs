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
        for (int i = 0; i < 3; i++)
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



    private int GetFirstNotEmptySlot()
    {
        for (int i = 0; i < _throwableInventorySlots.Count; i++)
            if (!_throwableInventorySlots[i].Empty) return i;
        return -1;
    }
    public void Throw()
    {
        int notEmptySlotIndex = GetFirstNotEmptySlot();
        if (notEmptySlotIndex < 0) return;


        Vector3 itemPosition = _inventory.DropPoint.position;
        Quaternion itemRotation = _inventory.StateMachine.CameraControllers.Cine.MainCamera.transform.rotation;
        ThrowableStateMachine throwableStateMachine = Instantiate(_throwableInventorySlots[notEmptySlotIndex].ItemData.ItemPrefab, itemPosition, itemRotation).GetComponent<ThrowableStateMachine>();
        throwableStateMachine.ChangeState(ThrowableStateMachine.StateLabels.Activated);

        //Disabled for testing
        //_throwableInventorySlots[notEmptySlotIndex].EmptySlot();
    }
}