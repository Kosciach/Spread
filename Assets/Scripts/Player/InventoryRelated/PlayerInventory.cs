using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerWeaponInventory _weapon; public PlayerWeaponInventory Weapon { get { return _weapon; } }
    [SerializeField] PlayerAmmoInventory _ammo; public PlayerAmmoInventory Ammo { get { return _ammo; } }
}
