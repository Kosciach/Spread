using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerBaseState
{

    public PlayerLandState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        float forwardVelocity = Vector3.Dot(_ctx.MovementControllers.Movement.InAir.CurrentMovementVector, _ctx.transform.forward);


        CheckHardLanding();

        _ctx.CoreControllers.Collider.SetColliderRadius(0.2f);
        _ctx.MovementControllers.VerticalVelocity.SlopeController.ToggleSlopeAngle(true);

        _ctx.AnimatingControllers.Animator.SetFloat("FallingTime", _ctx.MovementControllers.VerticalVelocity.GravityController.CurrentGravityForce);
        _ctx.AnimatingControllers.Animator.SetFloat("FallForwardVelocity", forwardVelocity, 0.1f);
        _ctx.AnimatingControllers.Animator.SetBool("Land", true);

        _ctx.AudioControllers.FootStep.LandFootStep(_ctx.MovementControllers.VerticalVelocity.GravityController.CurrentGravityForce);

        _ctx.SwitchController.SwitchTo.Idle();
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
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
        _ctx.AnimatingControllers.Animator.SetBool("Land", false);
    }


    private void CheckHardLanding()
    {
        _ctx.WasHardLanding = _ctx.MovementControllers.VerticalVelocity.GravityController.CurrentGravityForce <= -12;

        if (_ctx.WasHardLanding) _ctx.CombatControllers.Combat.TemporaryUnEquip();
        _ctx.CameraControllers.Hands.EnableController.ToggleHandsCamera(!_ctx.WasHardLanding);
        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, !_ctx.WasHardLanding, 6);
        _ctx.AnimatingControllers.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, !_ctx.WasHardLanding, 6);
    }
}
