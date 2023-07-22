using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerInventory_Weapon _weapon;            public PlayerInventory_Weapon Weapon { get { return _weapon; } }
    [SerializeField] PlayerInventory_Ammo _ammo;                public PlayerInventory_Ammo Ammo { get { return _ammo; } }
    [SerializeField] PlayerInventory_Items _item;               public PlayerInventory_Items Item { get { return _item; } }
    [SerializeField] PlayerInventory_Throwables _throwables;    public PlayerInventory_Throwables Throwables { get { return _throwables; } }
    [Space(5)]
    [SerializeField] PlayerStateMachine _stateMachine;          public PlayerStateMachine StateMachine { get { return _stateMachine; } }
    [SerializeField] Transform _dropPoint;                      public Transform DropPoint { get { return _dropPoint; } }
}
