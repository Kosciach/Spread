using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderState : PlayerBaseState
{

    public PlayerLadderState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.LadderController.ResetBools();

        _ctx.HandsCameraEnableController.ToggleHandsCamera(false);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, false, 5);
        _ctx.AnimatorController.ToggleLayer(false, 5, PlayerAnimatorController.LayersEnum.TopBodyStabilizer);

        if (_ctx.LadderController.GetLadderType() == PlayerLadderController.LadderEnum.LowerEnter) _ctx.LadderController.LowerLadderEnter();
        else if (_ctx.LadderController.GetLadderType() == PlayerLadderController.LadderEnum.HigherEnter) _ctx.LadderController.HigherLadderEnter();
    }
    public override void StateUpdate()
    {
        _ctx.LadderController.CheckLadderLowerExit();

        _ctx.MovementController.LadderMovement();
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
        _ctx.HandsCameraEnableController.ToggleHandsCamera(true);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, true, 5);
        _ctx.AnimatorController.ToggleLayer(true, 5, PlayerAnimatorController.LayersEnum.TopBodyStabilizer);
    }
}
