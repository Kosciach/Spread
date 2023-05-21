using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerBaseState
{

    public PlayerLandState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        float forwardVelocity = Vector3.Dot(_ctx.MovementController.InAir.CurrentMovementVector, _ctx.transform.forward);


        CheckHardLanding();

        _ctx.ColliderController.SetColliderRadius(0.2f);
        _ctx.VerticalVelocityController.SlopeController.ToggleSlopeAngle(true);

        _ctx.AnimatorController.SetFloat("FallingTime", _ctx.VerticalVelocityController.GravityController.CurrentGravityForce);
        _ctx.AnimatorController.SetFloat("FallForwardVelocity", forwardVelocity, 0.1f);
        _ctx.AnimatorController.SetBool("Land", true);

        _ctx.FootStepAudioController.LandFootStep(_ctx.VerticalVelocityController.GravityController.CurrentGravityForce);

        _ctx.SwitchController.SwitchTo.Idle();
    }
    public override void StateUpdate()
    {
        _ctx.RotationController.RotateToCanera();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.WasHardLanding) return;

        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
    }
    public override void StateExit()
    {
        _ctx.AnimatorController.SetBool("Land", false);
    }


    private void CheckHardLanding()
    {
        _ctx.WasHardLanding = _ctx.VerticalVelocityController.GravityController.CurrentGravityForce <= -12;

        if (_ctx.WasHardLanding) _ctx.CombatController.TemporaryUnEquip();
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(!_ctx.WasHardLanding);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, !_ctx.WasHardLanding, 6);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, !_ctx.WasHardLanding, 6);
    }
}
