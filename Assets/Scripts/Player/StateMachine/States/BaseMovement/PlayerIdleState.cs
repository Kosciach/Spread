using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CameraControllers.Cine.Fov.SetFov(0, 0.5f);
        if (_ctx.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed))
        {
            _ctx.CameraControllers.Hands.MoveController.SetCameraPosition(PlayerHandsCameraMoveController.CameraPositionsEnum.Idle, 5);
        }

        if (_ctx.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped))
        {
            _ctx.CombatControllers.Combat.EquipedWeapon.OnPlayerIdle();
        }

        _ctx.MovementControllers.VerticalVelocity.JumpController.ToggleJumpReloaded(true);
        _ctx.CoreControllers.Collider.SetColliderRadius(0.8f);



        _ctx.AnimatingControllers.Animator.SetBool("Idle", true);
        _ctx.AnimatingControllers.Animator.SetBool("Land", true);
        _ctx.AnimatingControllers.Animator.SetInt("JumpType", 0);
        _ctx.AnimatingControllers.Animator.SetBool("FallFromGround", false);
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
        _ctx.MovementControllers.Rotation.RotateAnimation();
        _ctx.MovementControllers.Movement.OnGround.Movement();
        _ctx.MovementControllers.Movement.OnGround.CheckMovementType();

        float forwardVelocity = Vector3.Dot(_ctx.MovementControllers.Movement.InAir.CurrentMovementVector, _ctx.transform.forward);
        _ctx.AnimatingControllers.Animator.SetFloat("FallForwardVelocity", forwardVelocity, 0.1f);

        if (!_ctx.MovementControllers.VerticalVelocity.GravityController.IsGrounded) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)) StateChange(_factory.Walk());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run) && !_ctx.CombatControllers.EquipedWeapon.Wall.IsWall) StateChange(_factory.Run());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) StateChange(_factory.Jump());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall))
        {
            _ctx.AnimatingControllers.Animator.SetBool("FallFromGround", true);
            StateChange(_factory.Fall());
        }
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Crouch)) StateChange(_factory.Crouch());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Climb)) StateChange(_factory.Climb());
    }
    public override void StateExit()
    {
        _ctx.AnimatingControllers.Animator.SetBool("Idle", false);

        _ctx.AnimatingControllers.Animator.SetFloat("RotateDirection", 0);
        _ctx.AnimatingControllers.Animator.SetFloat("RotationSpeed", 0);
        _ctx.AnimatingControllers.Animator.SetBool("IsRotating", false);
    }
}
