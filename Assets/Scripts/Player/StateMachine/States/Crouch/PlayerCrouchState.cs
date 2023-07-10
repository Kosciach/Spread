using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerCrouchState : PlayerBaseState
{

    public PlayerCrouchState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatingControllers.Weapon.Crouch.Toggle(true);

        if (_ctx.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed))
        {
            _ctx.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.Crouch, 5);
        }

        if (_ctx.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped) && _ctx.CombatControllers.Combat.EquipedWeapon != null)
        {
            _ctx.CombatControllers.Combat.EquipedWeapon.OnPlayerCrouch();
        }

        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.Crouch, true, 0.3f);
        _ctx.MovementControllers.Movement.OnGround.SetCrouchSpeed();
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
        _ctx.MovementControllers.Movement.OnGround.Movement();

        if (_ctx.StateControllers.Swim.CheckSwimEnter()) _ctx.SwitchController.SwitchTo.Swim();
        if (!_ctx.MovementControllers.VerticalVelocity.Gravity.IsGrounded) _ctx.SwitchController.SwitchTo.Fall();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)) StateChange(_factory.Walk());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)) StateChange(_factory.Run());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump)) StateChange(_factory.Jump());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Fall)) StateChange(_factory.Fall());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Climb)) StateChange(_factory.Climb());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder)) StateChange(_factory.Ladder());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Swim)) StateChange(_factory.Swim());
    }
    public override void StateExit()
    {
        if (_ctx.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed))
            _ctx.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.IdleWalkRun, 5);

        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.Crouch, false, 0.3f);

        _ctx.AnimatingControllers.Weapon.Crouch.Toggle(false);
    }
}
