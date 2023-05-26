using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{

    public PlayerDashState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatingControllers.Animator.SetFloat("DashX", _ctx.CoreControllers.Input.MovementInputVector.x);
        _ctx.AnimatingControllers.Animator.SetFloat("DashZ", _ctx.CoreControllers.Input.MovementInputVector.z);
        _ctx.AnimatingControllers.Animator.SetBool("Dash", true);

        _ctx.StateControllers.Dash.DashStart(_ctx.MovementControllers.Movement.OnGround.GetMovementDirection());
        _ctx.SwitchController.SwitchTo.Dash();
    }
    public override void StateUpdate()
    {
        _ctx.StateControllers.Dash.DashMove();
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
        _ctx.AnimatingControllers.Animator.SetBool("Dash", false);
    }
}
