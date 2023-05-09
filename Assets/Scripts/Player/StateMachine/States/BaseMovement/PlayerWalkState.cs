using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CineCameraController.Fov.SetFov(5, 2);
        _ctx.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Walk, 5);

        _ctx.VerticalVelocityController.JumpController.ToggleJumpReloaded(true);
        _ctx.ColliderController.SetColliderRadius(0.8f);
        _ctx.AnimatorController.SetBool("Walk", true);
        _ctx.AnimatorController.SetBool("Land", true);
        _ctx.AnimatorController.SetInt("JumpType", 1);
        _ctx.AnimatorController.SetBool("FallFromGround", false);
        _ctx.MovementController.OnGround.SetWalkSpeed();
    }
    public override void StateUpdate()
    {
        _ctx.RotationController.RotateToCanera();
        _ctx.MovementController.OnGround.Movement();
        _ctx.MovementController.OnGround.CheckMovementType();

        if (_ctx.SwimController.CheckSwimEnter()) _ctx.SwitchController.SwitchTo.Swim();
        if (!_ctx.VerticalVelocityController.GravityController.IsGrounded) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)) StateChange(_factory.Run());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder)) StateChange(_factory.Ladder());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) StateChange(_factory.Jump());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall))
        {
            _ctx.AnimatorController.SetBool("FallFromGround", true);
            StateChange(_factory.Fall());
        }
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Crouch)) StateChange(_factory.Crouch());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Climb)) StateChange(_factory.Climb());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Swim)) StateChange(_factory.Swim());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Dash)) StateChange(_factory.Dash());
    }
    public override void StateExit()
    {
        _ctx.AnimatorController.SetBool("Walk", false);
    }
}