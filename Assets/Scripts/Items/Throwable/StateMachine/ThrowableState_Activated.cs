using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableState_Activated : ThrowableBaseState
{

    public ThrowableState_Activated(ThrowableStateMachine ctx) : base(ctx) { }

    public override void StateEnter()
    {
        _ctx.ThrowableController.OnActivate();
        _ctx.gameObject.layer = 0;
        _ctx.Rigidbody.AddForce(_ctx.transform.forward * _ctx.ThrowableData.ThrowStrenght, ForceMode.Impulse);
    }
    public override void StateExit()
    {

    }
}
