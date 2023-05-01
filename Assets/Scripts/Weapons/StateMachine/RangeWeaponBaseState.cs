using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeWeaponBaseState
{
    protected RangeWeaponStateMachine _ctx;
    protected RangeWeaponStateFactory _factory;

    public RangeWeaponBaseState(RangeWeaponStateMachine ctx, RangeWeaponStateFactory factory, string stateName)
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


    protected void StateChange(RangeWeaponBaseState newState)
    {
        StateExit();
        _ctx.CurrentState = newState;
        _ctx.CurrentState.StateEnter();
    }
}
