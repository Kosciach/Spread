using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerBaseState
{

    public PlayerLandState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(false);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, false, 6);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, false, 6);

        _ctx.ColliderController.SetColliderRadius(0.2f);
        _ctx.SlopeController.ToggleSlopeAngle(true);

        _ctx.AnimatorController.SetFloat("FallVelocity", _ctx.GravityController.CurrentGravityForce);
        _ctx.AnimatorController.SetFloat("FallForwardVelocity", _ctx.MovementController.InAir.CurrentMovementVector.z);
        _ctx.AnimatorController.SetBool("Land", true);

        _ctx.FootStepAudioController.LandFootStep(_ctx.GravityController.CurrentGravityForce);

        CheckSlowDown();
        _ctx.SwitchController.SwitchTo.Idle();
        //_ctx.RecoverFromLanding();
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
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(true);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, true, 6);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, true, 6);

        _ctx.AnimatorController.SetBool("Land", false);
    }


    private void CheckSlowDown()
    {
        if (_ctx.GravityController.CurrentGravityForce <= -0.3f)
            _ctx.WasHardLanding = true;
    }
}
