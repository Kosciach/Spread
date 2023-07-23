using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableState_Activated : ThrowableBaseState
{

    public ThrowableState_Activated(ThrowableStateMachine ctx) : base(ctx) { }

    public override void StateEnter()
    {
        _ctx.ThrowableController.OnActivate();


    }
    public override void StateExit()
    {

    }
}
