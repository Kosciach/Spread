using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _playerMainCamera;
    [SerializeField] Transform[] _weaponsHolders = new Transform[5]; public Transform[] WeaponsHolders { get { return _weaponsHolders; } }



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] List<WeaponStateMachine> _weapons = new List<WeaponStateMachine>(2); public List<WeaponStateMachine> Weapons { get { return _weapons; } }
    [SerializeField] List<WeaponData> _weaponsData = new List<WeaponData>(2); public List<WeaponData> WeaponsData { get { return _weaponsData; } }







    public void AddWeapon(WeaponStateMachine newWeapon, WeaponData newWeaponData)
    {
        Debug.Log("AddedWeapon: " + newWeaponData.WeaponName);

        int smallestEmptyIndex = FindSmallestEmptyIndex(); Debug.Log("SmallestIndex: " + smallestEmptyIndex);
        if (smallestEmptyIndex < 0) return;


        _weapons[smallestEmptyIndex] = newWeapon;
        _weaponsData[smallestEmptyIndex] = newWeaponData;

        HolsterWeapon(newWeapon, newWeaponData);
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
        _weapons[weaponToDropIndex].transform.parent = null;
        _weapons[weaponToDropIndex].transform.position = _playerMainCamera.position;
        _weapons[weaponToDropIndex].SwitchController.SwitchTo.Ground();
        _weapons[weaponToDropIndex].Rigidbody.AddForce(_playerMainCamera.forward * 10);

        _weapons[weaponToDropIndex] = null;
        _weaponsData[weaponToDropIndex] = null;
    }

    public WeaponStateMachine GetWeapon(int index)
    {
        return _weapons[index];
    }
    public WeaponData GetWeaponData(int index)
    {
        return _weaponsData[index];
    }
}
