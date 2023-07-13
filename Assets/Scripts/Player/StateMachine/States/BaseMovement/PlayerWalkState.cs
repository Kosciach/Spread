using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CameraControllers.Cine.Fov.SetFov(5, 0.5f);
        if (_ctx.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed))
        {
            _ctx.CameraControllers.Hands.Move.SetCameraPosition(PlayerHandsCamera_Move.CameraPositionsEnum.Walk, 5);
        }

        if (_ctx.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped) && _ctx.CombatControllers.Combat.EquipedWeapon != null)
        {
            _ctx.CombatControllers.Combat.EquipedWeapon.OnPlayerWalk();
        }

        _ctx.MovementControllers.VerticalVelocity.Jump.ToggleJumpReloaded(true);
        _ctx.CoreControllers.Collider.SetColliderRadius(0.8f, 0.2f);



        _ctx.AnimatingControllers.Animator.SetBool("Walk", true);
        _ctx.AnimatingControllers.Animator.SetBool("Land", true);
        _ctx.AnimatingControllers.Animator.SetInt("JumpType", 1);
        _ctx.AnimatingControllers.Animator.SetBool("FallFromGround", false);



        _ctx.MovementControllers.Movement.OnGround.SetWalkSpeed();
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
        _ctx.MovementControllers.Movement.OnGround.Movement();
        _ctx.MovementControllers.Movement.OnGround.CheckMovementType();

        float forwardVelocity = Vector3.Dot(_ctx.MovementControllers.Movement.InAir.CurrentMovementVector, _ctx.transform.forward);
        _ctx.AnimatingControllers.Animator.SetFloat("FallForwardVelocity", forwardVelocity, 0.1f);

        if (_ctx.StateControllers.Swim.CheckSwimEnter()) _ctx.SwitchController.SwitchTo.Swim();
        if (!_ctx.MovementControllers.VerticalVelocity.Gravity.IsGrounded) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run) && !_ctx.CombatControllers.EquipedWeapon.Wall.IsWall) StateChange(_factory.Run());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder)) StateChange(_factory.Ladder());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) StateChange(_factory.Jump());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall))
        {
            _ctx.AnimatingControllers.Animator.SetBool("FallFromGround", true);
            StateChange(_factory.Fall());
        }
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Crouch)) StateChange(_factory.Crouch());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Climb)) StateChange(_factory.Climb());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Swim))
        {
            _ctx.CombatControllers.Combat.TemporaryUnEquip();
            StateChange(_factory.Swim());
        }
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Dash)) StateChange(_factory.Dash());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.AttachmentTable)) StateChange(_factory.AttachmentTable());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Inventory)) StateChange(_factory.Inventory());
    }
    public override void StateExit()
    {
        _ctx.AnimatingControllers.Animator.SetBool("Walk", false);
    }
}