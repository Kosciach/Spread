using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventory_Weapon _weapon;        public PlayerInventory_Weapon Weapon { get { return _weapon; } }
    [SerializeField] PlayerInventory_Ammo _ammo;            public PlayerInventory_Ammo Ammo { get { return _ammo; } }
    [Space(5)]
    [SerializeField] PlayerStateMachine _stateMachine;      public PlayerStateMachine StateMachine { get { return _stateMachine; } }
}
