using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponInventoryState : RangeWeaponBaseState
{
    public RangeWeaponInventoryState(RangeWeaponStateMachine ctx, RangeWeaponStateFactory factory, string stateName) : base(ctx, factory, stateName) { }




    public override void StateEnter()
    {
        _ctx.Collider.enabled = false;
        _ctx.Rigidbody.isKinematic = true;
        _ctx.Outline.OutlineWidth = 0;

        _ctx.SetLayer(7);
    }
    public override void StateUpdate()
    {

    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsStateSwitch(RangeWeaponStateMachine.StateSwitchEnum.Ground)) StateChange(_factory.Ground());
        else if (_ctx.SwitchController.IsStateSwitch(RangeWeaponStateMachine.StateSwitchEnum.Equiped)) StateChange(_factory.Equiped());
    }
    public override void StateExit()
    {

    }
}
