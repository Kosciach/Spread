using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHolder : MonoBehaviour, IPickupable
{
    [Header("====Settings====")]
    [SerializeField] Ammo _ammo;
    [SerializeField] int _ammoCount;


    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }
    public void Pickup()
    {
        _playerStateMachine.InventoryControllers.Inventory.Ammo.AddAmmo(_ammo, _ammoCount);
    }
}
