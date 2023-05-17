using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataHolder : MonoBehaviour, IPickupable
{
    [Header("====References====")]
    [SerializeField] WeaponData _weaponData; public WeaponData WeaponData { get { return _weaponData; } }
    private PlayerInventory _playerInventory;



    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void Pickup()
    {
        _playerInventory.AddWeapon(gameObject.GetComponent<WeaponStateMachine>(), _weaponData);
    }
}
