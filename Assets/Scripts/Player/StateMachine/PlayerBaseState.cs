using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;

    public PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName)
    {
        _ctx = ctx;
        _factory = factory;
        _ctx.CurrentStateName = stateName;
    }

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
    public abstract void StateCheckChange();
    public abstract void StateExit();


    protected void StateChange(PlayerBaseState newState)
    {
        StateExit();
        _ctx.CurrentState = newState;
        _ctx.CurrentState.StateEnter();
    }
}
