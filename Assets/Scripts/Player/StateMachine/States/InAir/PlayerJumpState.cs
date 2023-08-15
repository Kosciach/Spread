using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        Jump();
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
        _ctx.MovementControllers.Movement.InAir.Movement();

        CheckFall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall)) StateChange(_factory.Fall());
    }
    public override void StateExit()
    {
        _ctx.MovementControllers.VerticalVelocity.Jump.IsJump = false;
    }


    private void Jump()
    {
        _ctx.AnimatingControllers.Animator.SetTrigger("Land", true);
        _ctx.AnimatingControllers.Animator.SetTrigger("Jump", false);
        _ctx.MovementControllers.VerticalVelocity.Jump.Jump();
    }

    private void CheckFall()
    {
        if (_ctx.MovementControllers.VerticalVelocity.Gravity.CurrentGravityForce <= 0) _ctx.SwitchController.SwitchTo.Fall();
    }
}
