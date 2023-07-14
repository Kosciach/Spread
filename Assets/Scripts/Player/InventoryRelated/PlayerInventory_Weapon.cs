using SimpleMan.CoroutineExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory_Weapon : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventoryController _inventory;
    [SerializeField] Transform _weaponDropPoint;
    [SerializeField] Transform[] _weaponsHolders = new Transform[5]; public Transform[] WeaponsHolders { get { return _weaponsHolders; } }


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] List<WeaponInventorySlot> _weaponInventorySlots; public List<WeaponInventorySlot> WeaponInventorySlots { get { return _weaponInventorySlots; } }



    private void Awake()
    {
        for (int i = 0; i < 2; i++)
            AddSlot();
    }



    public void AddSlot()
    {
        _weaponInventorySlots.Add(new WeaponInventorySlot(CanvasController.Instance.PanelsControllers.Inventory.CreateUIWeaponSlot()));
    }
    private int GetSmallestEmptyIndex()
    {
        for (int i = 0; i < _weaponInventorySlots.Count; i++)
            if (_weaponInventorySlots[i].WeaponData == null) return i;
        return -1;
    }

    public void AddWeapon(WeaponStateMachine addedWeapon, WeaponData addedWeaponData)
    {
        int smallestEmptyIndex = GetSmallestEmptyIndex();
        if (smallestEmptyIndex < 0)
        {
            //if (_inventory.StateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped)) ExchangeWeaponEquiped(addedWeapon, addedWeaponData);
            //else if (_inventory.StateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed)) ExchangeWeaponUnarmed(addedWeapon, addedWeaponData);

            return;
        }

        _weaponInventorySlots[smallestEmptyIndex].FillSlot(addedWeapon, addedWeaponData);
        HolsterWeapon(addedWeapon, addedWeaponData);
    }

    public void HolsterWeapon(WeaponStateMachine weapon, WeaponData weaponData)
    {
        weapon.transform.parent = _weaponsHolders[(int)weaponData.WeaponHolder];
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);

        weapon.SwitchController.SwitchTo.Inventory();
    }

    public void DropWeapon(int weaponToDropIndex)
    {
        CanvasController.Instance.HudControllers.Interaction.Pickup.SetArrowIcon(false);

        WeaponStateMachine weaponToDrop = _weaponInventorySlots[weaponToDropIndex].Weapon;
        weaponToDrop.transform.parent = null;
        weaponToDrop.transform.SetSiblingIndex(0);
        weaponToDrop.transform.position = _weaponDropPoint.position;
        weaponToDrop.SwitchController.SwitchTo.Ground();
        weaponToDrop.Rigidbody.AddForce(_weaponDropPoint.forward * 10);


        _weaponInventorySlots[weaponToDropIndex].EmptySlot();
    }





    private void ExchangeWeaponEquiped(WeaponStateMachine newWeapon, WeaponData newWeaponData)
    {
        _inventory.StateMachine.CombatControllers.Combat.DropWeapon();

        int smallestEmptyIndex = GetSmallestEmptyIndex();
        AddWeapon(newWeapon, newWeaponData);

        this.Delay(0.2f, () =>
        {
            _inventory.StateMachine.CombatControllers.Combat.EquipWeapon(smallestEmptyIndex);
        });
    }
    private void ExchangeWeaponUnarmed(WeaponStateMachine newWeapon, WeaponData newWeaponData)
    {
        DropWeapon(0);
        AddWeapon(newWeapon, newWeaponData);
    }
}

[System.Serializable]
public class WeaponInventorySlot
{
    public string Name;
    public WeaponStateMachine Weapon;
    public WeaponData WeaponData;
    public GameObject UISlot;
    public Image WeaponIcon;

    public WeaponInventorySlot(GameObject uiSlot)
    {
        UISlot = uiSlot;
        WeaponIcon = UISlot.transform.GetChild(0).GetComponent<Image>();

        EmptySlot();
    }


    public void FillSlot(WeaponStateMachine weapon, WeaponData weaponData)
    {
        Name = weaponData.WeaponName;
        WeaponIcon.sprite = weaponData.Icon;
        WeaponIcon.color = new Color(1, 1, 1, 1);

        Weapon = weapon;
        WeaponData = weaponData;
    }
    public void EmptySlot()
    {
        Name = "EmptySlot";
        WeaponIcon.color = new Color(1, 1, 1, 0);
        WeaponIcon.sprite = null;

        Weapon = null;
        WeaponData = null;
    }
}