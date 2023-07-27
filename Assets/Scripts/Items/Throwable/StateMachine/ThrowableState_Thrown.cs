using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableState_Thrown : ThrowableBaseState
{

    public ThrowableState_Thrown(ThrowableStateMachine ctx) : base(ctx) { }

    public override void StateEnter()
    {
        _ctx.transform.SetParent(null);

        _ctx.ThrowableController.OnThrown();
    }
    public override void StateUpdate()
    {

    }
    public override void StateExit()
    {

    }
}
