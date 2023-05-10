using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnderWaterState : PlayerBaseState
{

    public PlayerUnderWaterState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.SwimController.ToggleCameraEffect(true);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, false, 5);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.UnderWater, true, 5);
    }
    public override void StateUpdate()
    {
        _ctx.MovementController.Swim.Movement();
        _ctx.RotationController.RotateToCanera();

        if (_ctx.SwimController.CheckIsOnSurface()) _ctx.SwitchController.SwitchTo.Swim();
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
        _ctx.SwimController.ToggleCameraEffect(false);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, true, 5);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.UnderWater, false, 5);
    }
}
