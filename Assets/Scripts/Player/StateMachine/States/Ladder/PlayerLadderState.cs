using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderState : PlayerBaseState
{

    public PlayerLadderState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatControllers.Combat.TemporaryUnEquip();
        _ctx.CameraControllers.Hands.EnableController.ToggleHandsCamera(false);

        _ctx.StateControllers.Ladder.ResetBools();

        _ctx.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, false, 5);
        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, false, 5);
        _ctx.CameraControllers.Cine.Move.SetCameraPosition(PlayerCineCameraMoveController.CameraPositionsEnum.Ladder, 4);

        if (_ctx.StateControllers.Ladder.GetLadderType() == PlayerLadderController.LadderEnum.LowerEnter) _ctx.StateControllers.Ladder.LowerLadderEnter();
        else if (_ctx.StateControllers.Ladder.GetLadderType() == PlayerLadderController.LadderEnum.HigherEnter) _ctx.StateControllers.Ladder.HigherLadderEnter();
    }
    public override void StateUpdate()
    {
        _ctx.StateControllers.Ladder.CheckLadderLowerExit();

        _ctx.MovementControllers.Movement.Ladder.Movement();
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
        _ctx.AnimatingControllers.IkLayers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, true, 5);
        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, true, 5);
        _ctx.CameraControllers.Cine.Move.SetCameraPosition(PlayerCineCameraMoveController.CameraPositionsEnum.OnGround, 4);

        _ctx.CameraControllers.Hands.EnableController.ToggleHandsCamera(true);

        _ctx.CombatControllers.Combat.RecoverFromTemporaryUnEquip();
    }
}
