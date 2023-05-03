using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponGroundState : RangeWeaponBaseState
{
    public RangeWeaponGroundState(RangeWeaponStateMachine ctx, RangeWeaponStateFactory factory, string stateName) : base(ctx, factory, stateName) { }




    public override void StateEnter()
    {
        _ctx.Collider.enabled = true;
        _ctx.Rigidbody.isKinematic = false;

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
        if (_ctx.SwitchController.IsStateSwitch(RangeWeaponStateMachine.StateSwitchEnum.Inventory)) StateChange(_factory.Inventory());
    }
    public override void StateExit()
    {

    }
}
