using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _playerMainCamera;
    [SerializeField] Transform[] _weaponsHolders = new Transform[5];



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] List<GameObject> _weapons = new List<GameObject>(3);
    [SerializeField] List<WeaponData> _weaponsData = new List<WeaponData>(3);







    public void AddWeapon(GameObject newWeapon, WeaponData newWeaponData)
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
    public void HolsterWeapon(GameObject weapon, WeaponData weaponData)
    {
        weapon.transform.parent = _weaponsHolders[(int)weaponData.WeaponHolder];
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);

        RangeWeaponStateMachine newWeaponStateMachine = weapon.GetComponent<RangeWeaponStateMachine>();
        newWeaponStateMachine.SwitchController.SwitchTo.Inventory();
    }

    public void DropWeapon(int weaponToDropIndex)
    {
        _weapons[weaponToDropIndex].transform.parent = null;
        _weapons[weaponToDropIndex].transform.position = _playerMainCamera.position;
        _weapons[weaponToDropIndex].GetComponent<RangeWeaponStateMachine>().SwitchController.SwitchTo.Ground();
        _weapons[weaponToDropIndex].GetComponent<Rigidbody>().AddForce(_playerMainCamera.forward * 10);

        _weapons[weaponToDropIndex] = null;
        _weaponsData[weaponToDropIndex] = null;
    }

    public GameObject GetWeapon(int index)
    {
        return _weapons[index];
    }
    public WeaponData GetWeaponData(int index)
    {
        return _weaponsData[index];
    }
}
