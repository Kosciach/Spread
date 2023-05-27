using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHolder : MonoBehaviour, IPickupable
{
    [SerializeField] Ammo _ammo; public Ammo Ammo { get { return _ammo; } }
    [SerializeField] int _ammount;

    private PlayerInventory _playerInventory;



    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void Pickup()
    {
        _playerInventory.Ammo.AddAmmo(_ammo, _ammount);
    }
}
