using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnderWaterState : PlayerBaseState
{

    public PlayerUnderWaterState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.StateControllers.Swim.ToggleCameraEffect(true);
        _ctx.AnimatingControllers.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, false, 5);
        _ctx.AnimatingControllers.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.UnderWater, true, 5);
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
        _ctx.AnimatingControllers.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Swim, true, 5);
        _ctx.AnimatingControllers.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.UnderWater, false, 5);
    }
}
