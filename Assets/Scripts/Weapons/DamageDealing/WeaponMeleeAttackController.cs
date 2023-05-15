using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeAttackController : WeaponDamageDealingController
{


    public override void VirtualAwake()
    {

    }
    private void Start()
    {
        _inputs.Melee.Attack.performed += ctx => Attack();
    }


    private void Attack()
    {
        Debug.Log("BamBam");
    }
}
