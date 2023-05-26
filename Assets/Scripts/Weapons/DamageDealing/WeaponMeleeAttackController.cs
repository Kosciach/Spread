using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeAttackController : WeaponDamageDealingController
{


    private delegate void AttackType();
    private AttackType[] _attackType = new AttackType[2];


    public override void VirtualAwake()
    {

    }
    private void Start()
    {
        _attackType[0] = Swing;
        _attackType[1] = Throw;

        _inputs.Melee.Attack.performed += ctx => Attack();
    }



    private void Attack()
    {
        int attackType = _stateMachine.PlayerStateMachine.CombatControllers.Combat.EquipedWeaponController.Aim.IsAim ? 1 : 0;
        _attackType[attackType]();
    }


    private void Swing()
    {
        Debug.Log("Swing");
    }
    private void Throw()
    {
        Debug.Log("Throw");
    }
}
