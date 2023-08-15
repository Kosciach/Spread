using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using System;

public class PlayerLandState : PlayerBaseState
{
    private bool _isHardLanding;
    public PlayerLandState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _isHardLanding = false;
        CheckLandingType();
        _ctx.AnimatingControllers.Animator.SetTrigger("Land", false);
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


    private void CheckLandingType()
    {
        _isHardLanding = (-_ctx.MovementControllers.VerticalVelocity.Gravity.CurrentGravityForce) >= 10.5f;
        _ctx.AnimatingControllers.Animator.SetBool("HardLanding", _isHardLanding);
        Action landingType = _isHardLanding ? HardLanding : NormalLanding;
        landingType();
    }
    private void NormalLanding()
    {
        float fallingForwardVelocity = Vector3.Dot(_ctx.MovementControllers.Velocity.Velocity, _ctx.transform.forward);

        _ctx.AnimatingControllers.Animator.SetFloat("FallingVelocity", -_ctx.MovementControllers.VerticalVelocity.Gravity.CurrentGravityForce);
        _ctx.AnimatingControllers.Animator.SetFloat("FallingForwardVelocity", fallingForwardVelocity);
        _ctx.SwitchController.SwitchTo.Idle();
    }
    private void HardLanding()
    {
        _ctx.AnimatingControllers.Animator.SetFloat("MovementX", 0);
        _ctx.AnimatingControllers.Animator.SetFloat("MovementZ", 0);

        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, false, 0.1f);
        _ctx.AnimatingControllers.Animator.ToggleLayer(LayersEnum.TopBodyStabilizer, false, 0.1f);
    }
}
