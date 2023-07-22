using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelsControllers_Inventory : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Image _equipedWeaponIcon;
    [SerializeField] SlotCreateData _itemSlotCreateData;
    [SerializeField] SlotCreateData _throwableSlotCreateData;
    [SerializeField] SlotCreateData _weaponSlotCreateData;

    [System.Serializable]
    public struct SlotCreateData
    {
        public Transform UISlotsHolder;
        public GameObject UISlotPrefab;
    }



    public UIItemController CreateUIItemSlot(PlayerInventoryController playerInventory)
    {
        UIItemController uiItemController = Instantiate(_itemSlotCreateData.UISlotPrefab, _itemSlotCreateData.UISlotsHolder).transform.GetChild(0).GetComponent<UIItemController>();
        uiItemController.PlayerInventory = playerInventory;
        return uiItemController;
    }
    public UIItemController CreateUIThrowableSlot(PlayerInventoryController playerInventory)
    {
        UIItemController uiItemController = Instantiate(_throwableSlotCreateData.UISlotPrefab, _throwableSlotCreateData.UISlotsHolder).transform.GetChild(0).GetComponent<UIItemController>();
        uiItemController.PlayerInventory = playerInventory;
        return uiItemController;
    }
    public UIWeaponController CreateUIWeaponSlot(PlayerInventoryController playerInventory)
    {
        UIWeaponController uiWeaponController = Instantiate(_weaponSlotCreateData.UISlotPrefab, _weaponSlotCreateData.UISlotsHolder).transform.GetChild(0).GetComponent<UIWeaponController>();
        uiWeaponController.PlayerInventory = playerInventory;
        return uiWeaponController;
    }


    public void SetEquipedWeaponIcon(Sprite icon)
    {
        _equipedWeaponIcon.sprite = icon;
    }
    public void ToggleEquipedWeaponIcon(bool enable)
    {
        _equipedWeaponIcon.color = enable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
    }
}
