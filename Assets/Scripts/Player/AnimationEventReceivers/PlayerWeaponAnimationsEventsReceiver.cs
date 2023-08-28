using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAnimationsEventsReceiver : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;


    public void WeaponEquipAnimEnd()
    {
        _playerStateMachine.CombatControllers.Combat.Equip.OnEquipAnimationEnd();
    }

    public void WeaponUnEquipAnimEnd()
    {
        _playerStateMachine.CombatControllers.Combat.UnEquip.OnUnEquipAnimationEnd();
    }

    public void WeaponPutInHand()
    {
        _playerStateMachine.CombatControllers.Combat.Equip.PutWeaponIntoRightHand();
    }
}
