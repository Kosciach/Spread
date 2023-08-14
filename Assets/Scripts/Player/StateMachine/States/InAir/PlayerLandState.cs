using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerLandState : PlayerBaseState
{

    public PlayerLandState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        Debug.Log("Land");
        Debug.Log(_ctx.MovementControllers.VerticalVelocity.Gravity.CurrentGravityForce);
        _ctx.AnimatingControllers.Animator.SetTrigger("Land", false);


        _ctx.SwitchController.SwitchTo.Idle();
    }
    public override void StateUpdate()
    {
    }
    public override void StateFixedUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)) StateChange(_factory.Walk());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)) StateChange(_factory.Run());
    }
    public override void StateExit()
    {
        _ctx.AnimatingControllers.Animator.SetTrigger("Fall", true);
    }
}
