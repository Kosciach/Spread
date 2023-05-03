using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CameraFovController.SetFov(0, 2);
        _ctx.CameraMoveController.SetHandsCameraPosition(PlayerCameraMoveController.HandsCameraPositionsEnum.Idle, 5);

        _ctx.JumpController.ToggleJumpReloaded(true);
        _ctx.ColliderController.SetColliderRadius(0.5f);
        _ctx.MovementController.SetAccelaration(0.9f);
        _ctx.AnimatorController.SetBool("Idle", true);
        _ctx.AnimatorController.SetBool("Land", true);
        _ctx.AnimatorController.SetInt("JumpType", 0);
    }
    public override void StateUpdate()
    {
        _ctx.CameraController.RotatePlayerToCamera();
        _ctx.MovementController.OnGroundMovement();

        _ctx.SwitchController.SetBaseMovementSwitch();
        if (!_ctx.GravityController.GetIsGrounded()) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)) StateChange(_factory.Walk());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) StateChange(_factory.Jump());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall)) StateChange(_factory.Fall());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Crouch)) StateChange(_factory.Crouch());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Climb)) StateChange(_factory.Climb());
    }
    public override void StateExit()
    {
        _ctx.AnimatorController.SetBool("Idle", false);
        _ctx.MovementController.SetAccelaration(1);
    }
}
