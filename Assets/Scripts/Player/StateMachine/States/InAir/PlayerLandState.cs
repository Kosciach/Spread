using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerBaseState
{

    public PlayerLandState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        CheckHardLanding();

        _ctx.ColliderController.SetColliderRadius(0.2f);
        _ctx.VerticalVelocityController.SlopeController.ToggleSlopeAngle(true);

        _ctx.AnimatorController.SetFloat("FallingTime", _ctx.VerticalVelocityController.GravityController.CurrentGravityForce);
        _ctx.AnimatorController.SetFloat("FallForwardVelocity", _ctx.MovementController.InAir.CurrentMovementVector.z);
        _ctx.AnimatorController.SetBool("Land", true);

        _ctx.FootStepAudioController.LandFootStep(_ctx.VerticalVelocityController.GravityController.CurrentGravityForce);


        _ctx.SwitchController.SwitchTo.Idle();
    }
    public override void StateUpdate()
    {
        _ctx.CineCameraController.RotatePlayerToCamera();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if(!_ctx.WasHardLanding)
        {
            if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
            else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)) StateChange(_factory.Walk());
            else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)) StateChange(_factory.Run());
        }
    }
    public override void StateExit()
    {
        _ctx.AnimatorController.SetBool("Land", false);
    }


    private void CheckHardLanding()
    {
        _ctx.WasHardLanding = _ctx.VerticalVelocityController.GravityController.CurrentGravityForce <= -16;

        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(!_ctx.WasHardLanding);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, !_ctx.WasHardLanding, 6);
    }
}
