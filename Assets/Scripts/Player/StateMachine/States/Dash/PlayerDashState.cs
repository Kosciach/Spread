using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{

    public PlayerDashState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatorController.SetFloat("DashX", _ctx.InputController.MovementInputVector.x);
        _ctx.AnimatorController.SetFloat("DashZ", _ctx.InputController.MovementInputVector.z);
        _ctx.AnimatorController.SetBool("Dash", true);

        _ctx.DashController.DashStart(_ctx.MovementController.GetOnGrroundMovementDirection());
        _ctx.SwitchController.SwitchTo.Dash();
        Debug.Log(_ctx.MovementController.GetOnGrroundMovementDirectionString());
    }
    public override void StateUpdate()
    {
        _ctx.DashController.DashMove();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) StateChange(_factory.Jump());
    }
    public override void StateExit()
    {
        _ctx.AnimatorController.SetBool("Dash", false);
    }
}
