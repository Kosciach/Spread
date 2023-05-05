using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private bool _isClimb;
    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.ColliderController.SetColliderRadius(0.2f);

        PrepareAnimatorBools();

        _ctx.JumpController.ToggleJumpReloaded(false);
        _ctx.JumpController.ToggleIsJump(true);
        _ctx.JumpController.Jump();
    }
    public override void StateUpdate()
    {
        _ctx.CineCameraController.RotatePlayerToCamera();
        _ctx.MovementController.InAir.Movement();

        if (_ctx.GravityController.CurrentGravityForce <= 0) _ctx.SwitchController.SwitchTo.Fall();
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
        _ctx.JumpController.ToggleIsJump(false);
        _ctx.AnimatorController.SetBool("Jump", false);
    }



    private void PrepareAnimatorBools()
    {
        _ctx.AnimatorController.SetBool("Jump", true);
        _ctx.AnimatorController.SetBool("Fall", false);
        _ctx.AnimatorController.SetBool("Land", false);
    }
}
