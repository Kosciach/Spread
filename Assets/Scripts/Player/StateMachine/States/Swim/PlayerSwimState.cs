using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerSwimState : PlayerBaseState
{

    public PlayerSwimState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatingControllers.Animator.SetBool("Swim", true);

        ExitEnter(PlayerCineCameraMoveController.CameraPositionsEnum.Swim, false);
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Movement.Swim.Movement();
        _ctx.MovementControllers.Rotation.RotateToCanera();

        bool isOutOfSwim = _ctx.StateControllers.Swim.CheckIsOnSurface() && _ctx.MovementControllers.VerticalVelocity.GravityController.IsGrounded && _ctx.StateControllers.Swim.CheckObjectInFront();
        if (isOutOfSwim) _ctx.SwitchController.SwitchTo.Walk();
        else if(!_ctx.StateControllers.Swim.CheckIsOnSurface()) _ctx.SwitchController.SwitchTo.UnderWater();
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
        _ctx.MovementControllers.VerticalVelocity.GravityController.ToggleApplyGravity(enable);
        _ctx.CameraControllers.Hands.EnableController.ToggleHandsCamera(enable);
        _ctx.CameraControllers.Cine.Move.SetCameraPosition(cameraPosition, 6);

        _ctx.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, enable, 0.3f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Swim, !enable, 0.3f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.SpineLock, enable, 0.3f);
    }




    private void ExitToWalk()
    {
        _ctx.StateControllers.Swim.ToggleCameraEffect(false);
        _ctx.AnimatingControllers.Animator.SetBool("Swim", false);

        ExitEnter(PlayerCineCameraMoveController.CameraPositionsEnum.OnGround, true);

        _ctx.CombatControllers.Combat.RecoverFromTemporaryUnEquip();
    }
}
