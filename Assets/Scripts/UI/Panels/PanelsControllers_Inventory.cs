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



    public GameObject CreateUIItemSlot()
    {
        return Instantiate(_itemSlotCreateData.UISlotPrefab, _itemSlotCreateData.UISlotsHolder);
    }
    public GameObject CreateUIWeaponSlot()
    {
        return Instantiate(_weaponSlotCreateData.UISlotPrefab, _weaponSlotCreateData.UISlotsHolder);
    }
}
