using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private bool _isClimb;
    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(false);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, false, 6);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, false, 6);

        _ctx.ColliderController.SetColliderRadius(0.2f);

        PrepareAnimatorBools();

        _ctx.VerticalVelocityController.JumpController.ToggleJumpReloaded(false);
        _ctx.VerticalVelocityController.JumpController.ToggleIsJump(true);
        _ctx.VerticalVelocityController.JumpController.Jump();
    }
    public override void StateUpdate()
    {
        _ctx.CineCameraController.RotatePlayerToCamera();
        _ctx.MovementController.InAir.Movement();

        if (_ctx.VerticalVelocityController.GravityController.CurrentGravityForce <= 0) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall))
        {
            _ctx.AnimatorController.SetBool("Fall", true);
            StateChange(_factory.Fall());
        }
    }
    public override void StateExit()
    {
        _ctx.VerticalVelocityController.JumpController.ToggleIsJump(false);
        _ctx.AnimatorController.SetBool("Jump", false);
    }



    private void PrepareAnimatorBools()
    {
        _ctx.AnimatorController.SetBool("Jump", true);
        _ctx.AnimatorController.SetBool("Fall", false);
        _ctx.AnimatorController.SetBool("Land", false);
    }
}
