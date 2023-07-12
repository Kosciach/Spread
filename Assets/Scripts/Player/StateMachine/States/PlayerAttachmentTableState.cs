using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachmentTableState : PlayerBaseState
{

    public PlayerAttachmentTableState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.AnimatingControllers.Animator.SetBool("Idle", true);
        _ctx.AnimatingControllers.Animator.SetBool("Walk", false);
        _ctx.AnimatingControllers.Animator.SetBool("Run", false);
        _ctx.AnimatingControllers.Animator.SetBool("Land", true);
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Movement.OnGround.AnimatorMovement();
        _ctx.MovementControllers.Rotation.RotateToCanera();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
    }
    public override void StateExit()
    {

    }
}
