using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{

    public PlayerFallState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CoreControllers.Collider.SetColliderRadius(0.09f);
        _ctx.AnimatingControllers.Animator.SetBool("Land", false);

        _ctx.CombatControllers.Combat.EquipedWeaponController.InAir.Fall();
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Rotation.RotateToCanera();
        _ctx.MovementControllers.Movement.InAir.Movement();

        CheckClimb();
        if (_ctx.StateControllers.Swim.CheckSwimEnter()) _ctx.SwitchController.SwitchTo.Swim();
        if (_ctx.MovementControllers.VerticalVelocity.GravityController.IsGrounded) _ctx.SwitchController.SwitchTo.Land();
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
            _ctx.CombatControllers.Combat.TemporaryUnEquip();
            StateChange(_factory.Swim());
        }
    }
    public override void StateExit()
    {
        _ctx.AnimatingControllers.Animator.SetBool("Fall", false);
        _ctx.AnimatingControllers.Animator.SetBool("FallFromGround", false);

        _ctx.CombatControllers.Combat.EquipedWeaponController.InAir.Land();
    }



    private void CheckClimb()
    {
        if (_ctx.StateControllers.Climb.CheckFallingClimb())
        {
            _ctx.SwitchController.SwitchTo.InAirClimb();
        }
    }
}
