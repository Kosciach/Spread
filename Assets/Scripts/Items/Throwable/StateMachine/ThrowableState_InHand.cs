using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableState_InHand : ThrowableBaseState
{

    public ThrowableState_InHand(ThrowableStateMachine ctx) : base(ctx) { }

    public override void StateEnter()
    {
        _ctx.ChangeLayer(_ctx.transform, 8);

        _ctx.Rigidbody.isKinematic = true;
        _ctx.Rigidbody.useGravity = false;
        _ctx.Rigidbody.freezeRotation = true;

        _ctx.ThrowableController.OnInHand();

        _ctx.transform.localPosition = _ctx.ThrowableData.InHand.Pos;
        _ctx.transform.localRotation = Quaternion.Euler(_ctx.ThrowableData.InHand.Rot);
    }
    public override void StateUpdate()
    {
        _ctx.transform.localPosition = _ctx.ThrowableData.InHand.Pos;
        _ctx.transform.localRotation = Quaternion.Euler(_ctx.ThrowableData.InHand.Rot);
    }
    public override void StateExit()
    {
        _ctx.Rigidbody.isKinematic = false;
        _ctx.Rigidbody.useGravity = true;
        _ctx.Rigidbody.freezeRotation = false;
    }
}
