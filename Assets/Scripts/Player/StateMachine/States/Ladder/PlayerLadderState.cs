using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using System;

public class PlayerLadderState : PlayerBaseState
{

    public PlayerLadderState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.StateControllers.Ladder.Enter();
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Movement.Ladder.Movement();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {

    }
    public override void StateExit()
    {

    }

}