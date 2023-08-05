using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipedWeaponController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;        public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] PlayerEquipedWeapon_Hold _hold;                public PlayerEquipedWeapon_Hold Hold { get { return _hold; } }
    [SerializeField] PlayerEquipedWeapon_Aim _aim;                  public PlayerEquipedWeapon_Aim Aim { get { return _aim; } }
    [SerializeField] PlayerEquipedWeapon_Block _block;              public PlayerEquipedWeapon_Block Block { get { return _block; } }
    [SerializeField] PlayerEquipedWeapon_Run _run;                  public PlayerEquipedWeapon_Run Run { get { return _run; } }
    [SerializeField] PlayerEquipedWeapon_Wall _wall;                public PlayerEquipedWeapon_Wall Wall { get { return _wall; } }
}
