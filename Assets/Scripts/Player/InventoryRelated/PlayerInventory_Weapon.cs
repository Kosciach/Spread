using SimpleMan.CoroutineExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory_Weapon : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventoryController _inventory;
    [SerializeField] Transform _weaponDropPoint;
    [SerializeField] Transform[] _weaponsHolders = new Transform[5]; public Transform[] WeaponsHolders { get { return _weaponsHolders; } }


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] List<WeaponStateMachine> _weapons = new List<WeaponStateMachine>(2); public List<WeaponStateMachine> Weapons { get { return _weapons; } }
    [SerializeField] List<WeaponData> _weaponsData = new List<WeaponData>(2); public List<WeaponData> WeaponsData { get { return _weaponsData; } }




    public void AddWeapon(WeaponStateMachine newWeapon, WeaponData newWeaponData)
    {
        int smallestEmptyIndex = FindSmallestEmptyIndex();
        if (smallestEmptyIndex < 0)
        {
            if (_inventory.StateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped)) ExchangeWeaponEquiped(newWeapon, newWeaponData);
            else if (_inventory.StateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed)) ExchangeWeaponUnarmed(newWeapon, newWeaponData);

            return;
        }


        _weapons[smallestEmptyIndex] = newWeapon;
        _weaponsData[smallestEmptyIndex] = newWeaponData;
        HolsterWeapon(newWeapon, newWeaponData);

        CanvasController.Instance.HudControllers.Interaction.Pickup.SetArrowIcon(_weapons[0] != null && _weapons[1] != null);
    }
    private int FindSmallestEmptyIndex()
    {
        for (int i = 0; i < _weapons.Count; i++)
            if (_weapons[i] == null) return i;
        return -1;
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

        _weapons[weaponToDropIndex].transform.parent = null;
        _weapons[weaponToDropIndex].transform.SetSiblingIndex(0);
        _weapons[weaponToDropIndex].transform.position = _weaponDropPoint.position;
        _weapons[weaponToDropIndex].SwitchController.SwitchTo.Ground();
        _weapons[weaponToDropIndex].Rigidbody.AddForce(_weaponDropPoint.forward * 10);

        _weapons[weaponToDropIndex] = null;
        _weaponsData[weaponToDropIndex] = null;
    }





    private void ExchangeWeaponEquiped(WeaponStateMachine newWeapon, WeaponData newWeaponData)
    {
        _inventory.StateMachine.CombatControllers.Combat.DropWeapon();  int smallestEmptyIndex = FindSmallestEmptyIndex();
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
