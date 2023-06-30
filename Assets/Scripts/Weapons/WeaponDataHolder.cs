using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataHolder : MonoBehaviour, IPickupable
{
    [Header("====References====")]
    [SerializeField] WeaponData _weaponData; public WeaponData WeaponData { get { return _weaponData; } }
    private PlayerInventoryController _playerInventory;



    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventoryController>();
    }

    public void Pickup()
    {
        _playerInventory.Weapon.AddWeapon(gameObject.GetComponent<WeaponStateMachine>(), _weaponData);
    }
}
