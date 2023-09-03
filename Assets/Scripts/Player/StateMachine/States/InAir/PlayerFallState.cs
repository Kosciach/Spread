using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{

    public PlayerFallState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatingControllers.Animator.SetTrigger("Fall", false);
        ChangeColliderRadius();
        SetWeaponInAirSmooth();
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
        _ctx.MovementControllers.Movement.InAir.Movement();

        CheckSwitches();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Land)) StateChange(_factory.Land());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.InAirClimb)) StateChange(_factory.InAirClimb());
        else if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Swim))
        {
            _ctx.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip(false, 0.5f);
            StateChange(_factory.Swim());
        }
    }
    public override void StateExit()
    {
    }


    private void ChangeColliderRadius()
    {
        _ctx.CoreControllers.Collider.SetColliderRadius(0.2f, 0.2f);
    }
    private void SetWeaponInAirSmooth()
    {
        _ctx.AnimatingControllers.Weapon.InAir.SetPosSpeed(5);
        _ctx.AnimatingControllers.Weapon.InAir.SetRotSpeed(5);
    }
    private void CheckSwitches()
    {
        if (_ctx.MovementControllers.VerticalVelocity.Gravity.IsGrounded) _ctx.SwitchController.SwitchTo.Land();
        if (_ctx.StateControllers.Climb.CheckFallClimb()) _ctx.SwitchController.SwitchTo.InAirClimb();
        if (_ctx.StateControllers.Swim.CheckSwimEnter()) _ctx.SwitchController.SwitchTo.Swim();
    }
}
