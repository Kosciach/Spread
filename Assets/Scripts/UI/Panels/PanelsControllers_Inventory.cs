using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsControllers_Inventory : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] SlotCreateData _itemSlotCreateData;
    [SerializeField] SlotCreateData _weaponSlotCreateData;

    [System.Serializable]
    public struct SlotCreateData
    {
        public Transform UISlotsHolder;
        public GameObject UISlotPrefab;
    }



    public UIItemController CreateUIItemSlot(PlayerInventoryController playerInventory)
    {
        UIItemController itemSlotController = Instantiate(_itemSlotCreateData.UISlotPrefab, _itemSlotCreateData.UISlotsHolder).transform.GetChild(0).GetComponent<UIItemController>();
        itemSlotController.PlayerInventory = playerInventory;
        return itemSlotController;
    }
    public GameObject CreateUIWeaponSlot()
    {
        return Instantiate(_weaponSlotCreateData.UISlotPrefab, _weaponSlotCreateData.UISlotsHolder);
    }
}
