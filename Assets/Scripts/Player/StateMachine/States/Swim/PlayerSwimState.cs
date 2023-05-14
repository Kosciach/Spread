using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwimState : PlayerBaseState
{

    public PlayerSwimState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatorController.SetBool("Swim", true);

        ExitEnter(PlayerCineCameraMoveController.CameraPositionsEnum.Swim, false);
    }
    public override void StateUpdate()
    {
        _ctx.MovementController.Swim.Movement();
        _ctx.RotationController.RotateToCanera();

        bool isOutOfSwim = _ctx.SwimController.CheckIsOnSurface() && _ctx.VerticalVelocityController.GravityController.IsGrounded && _ctx.SwimController.CheckObjectInFront();
        if (isOutOfSwim) _ctx.SwitchController.SwitchTo.Walk();
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




    private void ExitEnter(PlayerCineCameraMoveController.CameraPositionsEnum cameraPosition, bool enable)
    {
        _ctx.VerticalVelocityController.GravityController.ToggleApplyGravity(enable);
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(enable);
        _ctx.CineCameraController.Move.SetCameraPosition(cameraPosition, 6);

        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, enable, 5);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, !enable, 5);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, enable, 5);
    }




    private void ExitToWalk()
    {
        _ctx.SwimController.ToggleCameraEffect(false);
        _ctx.AnimatorController.SetBool("Swim", false);

        ExitEnter(PlayerCineCameraMoveController.CameraPositionsEnum.OnGround, true);
        _ctx.CombatController.EquipWeapon(_ctx.CombatController.EquipedWeaponIndex);
    }
}
