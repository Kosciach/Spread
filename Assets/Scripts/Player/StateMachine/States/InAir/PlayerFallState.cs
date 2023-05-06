using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{

    public PlayerFallState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(false);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, false, 6);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, false, 6);

        _ctx.ColliderController.SetColliderRadius(0.09f);
        _ctx.AnimatorController.SetBool("Land", false);
    }
    public override void StateUpdate()
    {
        _ctx.CineCameraController.RotatePlayerToCamera();
        _ctx.MovementController.InAir.Movement();

        CheckClimb();
        if (_ctx.SwimController.CheckSwimEnter()) _ctx.SwitchController.SwitchTo.Swim();
        if (_ctx.VerticalVelocityController.GravityController.IsGrounded) _ctx.SwitchController.SwitchTo.Land();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Land)) StateChange(_factory.Land());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.InAirClimb)) StateChange(_factory.InAirClimb());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Swim)) StateChange(_factory.Swim());
    }
    public override void StateExit()
    {
        _ctx.AnimatorController.SetBool("Fall", false);
    }



    private void CheckClimb()
    {
        if (_ctx.ClimbController.CheckFallingClimb())
        {
            _ctx.SwitchController.SwitchTo.InAirClimb();
        }
    }
}
