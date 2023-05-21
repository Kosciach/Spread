using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderState : PlayerBaseState
{

    public PlayerLadderState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatController.TemporaryUnEquip();
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(false);

        _ctx.LadderController.ResetBools();

        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, false, 5);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, false, 5);
        _ctx.CineCameraController.Move.SetCameraPosition(PlayerCineCameraMoveController.CameraPositionsEnum.Ladder, 4);

        if (_ctx.LadderController.GetLadderType() == PlayerLadderController.LadderEnum.LowerEnter) _ctx.LadderController.LowerLadderEnter();
        else if (_ctx.LadderController.GetLadderType() == PlayerLadderController.LadderEnum.HigherEnter) _ctx.LadderController.HigherLadderEnter();
    }
    public override void StateUpdate()
    {
        _ctx.LadderController.CheckLadderLowerExit();

        _ctx.MovementController.Ladder.Movement();
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
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 5);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, true, 5);
        _ctx.CineCameraController.Move.SetCameraPosition(PlayerCineCameraMoveController.CameraPositionsEnum.OnGround, 4);

        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(true);

        _ctx.CombatController.RecoverFromTemporaryUnEquip();
    }
}
