using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwimState : PlayerBaseState
{

    public PlayerSwimState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatorController.SetBool("Swim", true);
        _ctx.GravityController.ToggleApplyGravity(false);

        _ctx.CameraMoveController.SetPosition(new Vector3(0, 0, -0.4f), 5);

        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, false, 5);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, true, 5);

    }
    public override void StateUpdate()
    {
        _ctx.MovementController.SwimMovement();
        _ctx.CameraController.RotatePlayerToCamera();

        if (_ctx.SwimController.CheckIsOnSurface() && _ctx.GravityController.GetIsGrounded() && _ctx.SwimController.CheckObjectInFront()) _ctx.SwitchController.SwitchTo.Walk();
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
        _ctx.GravityController.ToggleApplyGravity(true);

        _ctx.CameraMoveController.SetPosition(new Vector3(0, 0, 0), 5);

        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, true, 5);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, false, 5);
    }
}
