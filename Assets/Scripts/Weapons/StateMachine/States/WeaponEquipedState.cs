using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipedState : WeaponBaseState
{
    public WeaponEquipedState(WeaponStateMachine ctx, WeaponStateFactory factory, string stateName) : base(ctx, factory, stateName) { }




    public override void StateEnter()
    {
        _ctx.Collider.enabled = false;
        _ctx.Rigidbody.isKinematic = true;
        _ctx.Outline.OutlineWidth = 0;

        _ctx.SetLayer(8);
    }
    public override void StateUpdate()
    {

    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsStateSwitch(WeaponStateMachine.StateSwitchEnum.Inventory)) StateChange(_factory.Inventory());
        else if (_ctx.SwitchController.IsStateSwitch(WeaponStateMachine.StateSwitchEnum.Ground)) StateChange(_factory.Ground());
    }
    public override void StateExit()
    {

    }



}
