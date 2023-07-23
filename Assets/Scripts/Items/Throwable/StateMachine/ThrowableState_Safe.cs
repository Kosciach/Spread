using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableState_Safe : ThrowableBaseState
{

    public ThrowableState_Safe(ThrowableStateMachine ctx) : base(ctx) { }

    public override void StateEnter()
    {
        _ctx.ThrowableController.OnSafe();


    }
    public override void StateExit()
    {

    }
}
