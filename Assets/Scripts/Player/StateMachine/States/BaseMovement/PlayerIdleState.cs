using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatController.CheckCombatMovement(true, 5);

        _ctx.CineCameraController.Fov.SetFov(0, 2);
        if(_ctx.CombatController.IsState(PlayerCombatController.CombatStateEnum.Unarmed))
            _ctx.HandsCameraController.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);

        _ctx.VerticalVelocityController.JumpController.ToggleJumpReloaded(true);
        _ctx.ColliderController.SetColliderRadius(0.8f);
        _ctx.AnimatorController.SetBool("Idle", true);
        _ctx.AnimatorController.SetBool("Land", true);
        _ctx.AnimatorController.SetInt("JumpType", 0);
        _ctx.AnimatorController.SetBool("FallFromGround", false);
    }
    public override void StateUpdate()
    {
        _ctx.RotationController.RotateToCanera();
        _ctx.RotationController.RotateAnimation();
        _ctx.MovementController.OnGround.Movement();
        _ctx.MovementController.OnGround.CheckMovementType();

        if (!_ctx.VerticalVelocityController.GravityController.IsGrounded) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)) StateChange(_factory.Walk());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) StateChange(_factory.Jump());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall))
        {
            _ctx.AnimatorController.SetBool("FallFromGround", true);
            StateChange(_factory.Fall());
        }
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Crouch)) StateChange(_factory.Crouch());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Climb)) StateChange(_factory.Climb());
    }
    public override void StateExit()
    {
        _ctx.AnimatorController.SetBool("Idle", false);

        _ctx.AnimatorController.SetFloat("RotateDirection", 0);
        _ctx.AnimatorController.SetFloat("RotationSpeed", 0);
        _ctx.AnimatorController.SetBool("IsRotating", false);
    }
}
