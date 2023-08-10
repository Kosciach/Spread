using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableState_Safe : ThrowableBaseState
{

    public ThrowableState_Safe(ThrowableStateMachine ctx) : base(ctx) { }

    public override void StateEnter()
    {
        _ctx.ChangeLayer(_ctx.transform, 7);
        _ctx.ThrowableController.OnSafe();
    }
    public override void StateUpdate()
    {
        
    }
    public override void StateExit()
    {

    }
}
