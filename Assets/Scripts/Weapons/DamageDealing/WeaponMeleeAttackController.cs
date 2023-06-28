using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeAttackController : WeaponDamageDealingController
{


    private Action[] _attackType = new Action[2];


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
        if (!_mainToggle) return;

        int attackType = _stateMachine.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim ? 1 : 0;
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





    public override void ToggleOn()
    {

    }
    public override void ToggleOff()
    {

    }


    public override void OnWeaponEquip()
    {
        Debug.Log("Melee E");
    }
    public override void OnWeaponUnEquip()
    {
        Debug.Log("Melee UnE");
    }

    public override void OnPlayerIdle()
    {
        Debug.Log("Melee Idle");
    }
    public override void OnPlayerWalk()
    {
        Debug.Log("Melee Walk");
    }
    public override void OnPlayerCrouch()
    {
        Debug.Log("Melee Run");
    }

    public override void OnWeaponAim(bool isAim)
    {
        Debug.Log("Melee Aim " + isAim);
    }
}
