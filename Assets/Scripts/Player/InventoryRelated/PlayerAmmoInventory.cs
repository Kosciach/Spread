using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoInventory : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventory _inventory;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] int _spaceForAmmoTaken;
    [SerializeField] int _spaceForAmmoLeft;
    [SerializeField] int[] _ammoTypesAmmount; public int[] AmmoTypesAmmount { get { return _ammoTypesAmmount; } }

    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] int _maxSpaceForAmmo;





    private void Awake()
    {
        _spaceForAmmoLeft = _maxSpaceForAmmo - _spaceForAmmoTaken;
    }




    public void AddAmmo(Ammo newAmmo, int ammount)
    {
        if (_spaceForAmmoTaken + newAmmo.SizeInInventory * ammount > _maxSpaceForAmmo) return;

        int index = (int)newAmmo.AmmoType;

        _ammoTypesAmmount[index] += ammount;
        _spaceForAmmoTaken += newAmmo.SizeInInventory * ammount;

        _spaceForAmmoLeft = _maxSpaceForAmmo - _spaceForAmmoTaken;


        //Makes sure to update ammo HUD correctly
        if (!_inventory.StateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;
        RangeWeaponData rangeWeaponData = (RangeWeaponData)_inventory.StateMachine.CombatControllers.Combat.EquipedWeapon.DataHolder.WeaponData;
        if(rangeWeaponData.AmmoSettings.AmmoType.AmmoType != newAmmo.AmmoType) return;
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(_ammoTypesAmmount[index]);
    }



    public void RemoveAmmo(Ammo newAmmo, int ammount)
    {
        int index = (int)newAmmo.AmmoType;

        if (_ammoTypesAmmount[index] < ammount) return;


        _ammoTypesAmmount[index] -= ammount;
        _spaceForAmmoTaken -= newAmmo.SizeInInventory * ammount;

        _spaceForAmmoLeft = _maxSpaceForAmmo - _spaceForAmmoTaken;

        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(_ammoTypesAmmount[index]);
    }
}
