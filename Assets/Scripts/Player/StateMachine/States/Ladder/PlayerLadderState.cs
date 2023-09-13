using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using System;

public class PlayerLadderState : PlayerBaseState
{

    public PlayerLadderState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.StateControllers.Ladder.IsExit = false;

        _ctx.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(false);
        _ctx.CoreControllers.Collider.ToggleCollider(false);

        _ctx.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip(false, 0.5f);

        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, false, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, false, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Head, false, 0.5f);

        _ctx.CameraControllers.Cine.Move.SetCameraPosition(PlayerCineCamera_Move.CameraPositionsEnum.Ladder, 0.5f);


        Action enterMethod = _ctx.StateControllers.Ladder.IsEnterTop ? EnterTop : EnterNormally;
        enterMethod();


        _ctx.AnimatingControllers.Animator.SetTrigger("LadderEnter", false);
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Movement.Ladder.Movement();
        if(!_ctx.StateControllers.Ladder.IsExit) _ctx.StateControllers.Ladder.CheckExit();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)) StateChange(_factory.Idle());
    }
    public override void StateExit()
    {
        _ctx.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(true);
        _ctx.CoreControllers.Collider.ToggleCollider(true);

        _ctx.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();

        _ctx.AnimatingControllers.Animator.ToggleLayer(LayersEnum.TopBodyStabilizer, true, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, true, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, true, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Head, true, 0.5f);

        _ctx.StateControllers.Ladder.RestoreCameraSettings();
    }


    private void EnterTop()
    {
        _ctx.StateControllers.Ladder.RotatePlayerToLadder(0.5f);
        _ctx.StateControllers.Ladder.MoveToLadderFromTop();
    }
    private void EnterNormally()
    {
        _ctx.StateControllers.Ladder.RotatePlayerToLadder(0.3f);
        _ctx.StateControllers.Ladder.MoveToLadderNormally();
    }
}