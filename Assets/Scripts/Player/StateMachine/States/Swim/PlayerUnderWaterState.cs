using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerUnderWaterState : PlayerBaseState
{

    public PlayerUnderWaterState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.StateControllers.Swim.ToggleCameraEffect(true);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Swim, false, 1);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.UnderWater, true, 1);
    }
    public override void StateUpdate()
    {
        _ctx.MovementControllers.Movement.Swim.Movement();
        _ctx.MovementControllers.Rotation.RotateToCanera();

        if (_ctx.StateControllers.Swim.CheckIsOnSurface()) _ctx.SwitchController.SwitchTo.Swim();
    }
    public override void StateFixedUpdate()
    {

    }
    public override void StateCheckChange()
    {
        if (_ctx.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Swim)) StateChange(_factory.Swim());
    }
    public override void StateExit()
    {
        _ctx.StateControllers.Swim.ToggleCameraEffect(false);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Swim, true, 1);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.UnderWater, false, 1);
    }
}
