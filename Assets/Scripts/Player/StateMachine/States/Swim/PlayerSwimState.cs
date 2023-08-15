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

        ExitEnter(PlayerCineCamera_Move.CameraPositionsEnum.Swim, false);
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Movement.Swim.Movement();
        _ctx.MovementControllers.Rotation.RotateToCanera();

        bool isOutOfSwim = _ctx.StateControllers.Swim.CheckIsOnSurface() && _ctx.MovementControllers.VerticalVelocity.Gravity.IsGrounded && _ctx.StateControllers.Swim.CheckObjectInFront();
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




    private void ExitEnter(PlayerCineCamera_Move.CameraPositionsEnum cameraPosition, bool enable)
    {
        _ctx.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(enable);
        _ctx.CameraControllers.Cine.Move.SetCameraPosition(cameraPosition, 0.2f);

        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, enable, 0.3f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Swim, !enable, 0.3f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, enable, 0.3f);
    }




    private void ExitToWalk()
    {
        _ctx.StateControllers.Swim.ToggleCameraEffect(false);
        _ctx.AnimatingControllers.Animator.SetBool("Swim", false);

        ExitEnter(PlayerCineCamera_Move.CameraPositionsEnum.OnGround, true);

        _ctx.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
    }
}
