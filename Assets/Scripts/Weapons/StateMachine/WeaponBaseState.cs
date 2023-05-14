using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBaseState
{
    protected WeaponStateMachine _ctx;
    protected WeaponStateFactory _factory;

    public WeaponBaseState(WeaponStateMachine ctx, WeaponStateFactory factory, string stateName)
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


    protected void StateChange(WeaponBaseState newState)
    {
        StateExit();
        _ctx.CurrentState = newState;
        _ctx.CurrentState.StateEnter();
    }
}
