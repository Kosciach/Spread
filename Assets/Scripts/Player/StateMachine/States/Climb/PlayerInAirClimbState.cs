using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerInAirClimbState : PlayerBaseState
{
    public PlayerInAirClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip(false);
        ClimbEnterExit(false);

        Climb(_ctx.StateControllers.Climb.GetFinalClimbPosition(), _ctx.StateControllers.Climb.GetStartClimbPosition());
    }
    public override void StateUpdate()
    {

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
        ClimbEnterExit(true);

        _ctx.AnimatingControllers.Animator.SetBool("Climb", false);
        _ctx.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
    }


    private void ClimbEnterExit(bool enable)
    {
        _ctx.CameraControllers.Hands.Enable.ToggleHandsCamera(enable);
        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, enable, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, enable, 0.1f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.SpineLock, enable, 0.1f);

        _ctx.CoreControllers.Collider.ToggleCollider(enable);
        _ctx.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(enable);
        _ctx.CameraControllers.Cine.ToggleCineInput(enable);
    }




    private void Climb(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatingControllers.Animator.SetInt("ClimbType", 2);

        LeanTween.cancel(_ctx.gameObject);

        _ctx.AnimatingControllers.Animator.SetBool("Climb", true);
        _ctx.transform.LeanMoveY(finalClimbPosition.y - 1.3f, 0.5f).setOnComplete(() =>
        {
            _ctx.transform.LeanMove(finalClimbPosition, 0.5f).setOnComplete(() =>
            {
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
}
