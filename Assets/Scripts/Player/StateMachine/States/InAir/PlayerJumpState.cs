using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private bool _isClimb;
    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CoreControllers.Collider.SetColliderRadius(0.2f, 0.2f);

        PrepareAnimatorBools();

        _ctx.MovementControllers.VerticalVelocity.Jump.ToggleJumpReloaded(false);
        _ctx.MovementControllers.VerticalVelocity.Jump.ToggleIsJump(true);
        _ctx.MovementControllers.VerticalVelocity.Jump.Jump();

        _ctx.AnimatingControllers.Weapon.InAir.SetPosSpeed(5);
        _ctx.AnimatingControllers.Weapon.InAir.SetRotSpeed(5);
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
        _ctx.MovementControllers.Movement.InAir.Movement();

        if (_ctx.MovementControllers.VerticalVelocity.Gravity.CurrentGravityForce <= 0) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall))
        {
            _ctx.AnimatingControllers.Animator.SetBool("Fall", true);
            StateChange(_factory.Fall());
        }
    }
    public override void StateExit()
    {
        _ctx.MovementControllers.VerticalVelocity.Jump.ToggleIsJump(false);
        _ctx.AnimatingControllers.Animator.SetBool("Jump", false);
    }



    private void PrepareAnimatorBools()
    {
        _ctx.AnimatingControllers.Animator.SetBool("Jump", true);
        _ctx.AnimatingControllers.Animator.SetBool("Fall", false);
        _ctx.AnimatingControllers.Animator.SetBool("Land", false);
    }
}
