using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowableBaseState
{
    protected ThrowableStateMachine _ctx;

    public ThrowableBaseState(ThrowableStateMachine ctx)
    {
        _ctx = ctx;
    }


    public abstract void StateEnter();
    public abstract void StateExit();
}