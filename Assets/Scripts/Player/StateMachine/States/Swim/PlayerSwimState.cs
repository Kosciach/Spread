using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwimState : PlayerBaseState
{

    public PlayerSwimState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatorController.SetBool("Swim", true);
        _ctx.VerticalVelocityController.GravityController.ToggleApplyGravity(false);

        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(false);

        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, false, 5);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, true, 5);

    }
    public override void StateUpdate()
    {
        _ctx.MovementController.Swim.Movement();
        _ctx.CineCameraController.RotatePlayerToCamera();

        if (_ctx.SwimController.CheckIsOnSurface() && _ctx.VerticalVelocityController.GravityController.IsGrounded && _ctx.SwimController.CheckObjectInFront()) _ctx.SwitchController.SwitchTo.Walk();
        else if(!_ctx.SwimController.CheckIsOnSurface()) _ctx.SwitchController.SwitchTo.UnderWater();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk))
        {
            ExitToWalk();
            StateChange(_factory.Walk());
        }
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.UnderWater)) StateChange(_factory.UnderWater());
    }
    public override void StateExit()
    {

    }









    private void ExitToWalk()
    {
        _ctx.SwimController.ToggleCameraEffect(false);
        _ctx.AnimatorController.SetBool("Swim", false);
        _ctx.VerticalVelocityController.GravityController.ToggleApplyGravity(true);

        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(true);

        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 5);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, false, 5);
    }
}
