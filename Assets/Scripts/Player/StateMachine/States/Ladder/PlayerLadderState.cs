using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerLadderState : PlayerBaseState
{

    public PlayerLadderState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip();
        _ctx.CameraControllers.Hands.Enable.ToggleHandsCamera(false);

        _ctx.StateControllers.Ladder.ResetBools();

        _ctx.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, false, 0.5f);
        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, false, 0.5f);
        _ctx.CameraControllers.Cine.Move.SetCameraPosition(PlayerCineCamera_Move.CameraPositionsEnum.Ladder, 0.2f);

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
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, true, 0.5f);
        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, true, 0.5f);
        _ctx.CameraControllers.Cine.Move.SetCameraPosition(PlayerCineCamera_Move.CameraPositionsEnum.OnGround, 0.2f);

        _ctx.CameraControllers.Hands.Enable.ToggleHandsCamera(true);

        _ctx.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
    }
}
