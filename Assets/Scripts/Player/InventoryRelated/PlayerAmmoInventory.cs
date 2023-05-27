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
    [SerializeField] int[] _ammoTypesAmmount;

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
    }



    public void RemoveAmmo(Ammo newAmmo, int ammount)
    {
        int index = (int)newAmmo.AmmoType;

        if (_ammoTypesAmmount[index] < newAmmo.SizeInInventory * ammount) return;


        _ammoTypesAmmount[index] -= newAmmo.SizeInInventory * ammount;
        _spaceForAmmoTaken -= ammount;

        _spaceForAmmoLeft = _maxSpaceForAmmo - _spaceForAmmoTaken;
    }



    public bool IsAmmoForShoot(Ammo newAmmo)
    {
        int index = (int)newAmmo.AmmoType;

        return _ammoTypesAmmount[index] > 0;
    }
}
