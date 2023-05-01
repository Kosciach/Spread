using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataHolder : MonoBehaviour, IPickupableInterface
{
    [Header("====References====")]
    [SerializeField] WeaponData _weaponData; public WeaponData WeaponData { get { return _weaponData; } }
    [SerializeField] PlayerInventory _playerInventory;



    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void Pickup()
    {
        _playerInventory.AddWeapon(gameObject, _weaponData);
    }
}
