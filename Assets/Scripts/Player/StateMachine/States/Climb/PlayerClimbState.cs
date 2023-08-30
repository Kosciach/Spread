using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerClimbState : PlayerBaseState
{
    public PlayerClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip(false, 0.5f);
        ClimbEnterExit(false);

        CheckClimbType(_ctx.StateControllers.Climb.DetectedWallHeight);
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
        _ctx.AnimatingControllers.Animator.ToggleLayer(LayersEnum.TopBodyStabilizer, enable, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, enable, 0.1f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, enable, 0.1f);

        _ctx.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(enable);
        _ctx.CoreControllers.Collider.ToggleCollider(enable);

        _ctx.CameraControllers.Cine.ToggleCineInput(enable);
    }

    private void CheckClimbType(float climbHeight)
    {
        if(_ctx.StateControllers.Climb.IsVault)
        {
            Vault(_ctx.StateControllers.Climb.FinalClimbPosition, _ctx.StateControllers.Climb.StartClimbPosition);
            return;
        }

        if (climbHeight >= 0.2f && climbHeight <= 1.1f) ClimbSmall(_ctx.StateControllers.Climb.FinalClimbPosition, _ctx.StateControllers.Climb.StartClimbPosition);
        else if (climbHeight > 1.1f && climbHeight <= 2) ClimbMid(_ctx.StateControllers.Climb.FinalClimbPosition, _ctx.StateControllers.Climb.StartClimbPosition);
        else if (climbHeight > 2 && climbHeight <= 3.5f) ClimbHigh(_ctx.StateControllers.Climb.FinalClimbPosition, _ctx.StateControllers.Climb.StartClimbPosition);
        else _ctx.SwitchController.SwitchTo.Idle();
    }

    private void Vault(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatingControllers.Animator.SetInt("ClimbType", 1);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition - _ctx.transform.forward / 2, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatingControllers.Animator.SetBool("Climb", true);
            _ctx.transform.LeanMove(finalClimbPosition - _ctx.transform.forward / 2, 0.3f).setOnComplete(() =>
            {
                _ctx.AnimatingControllers.Animator.SetBool("Climb", false);
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
    private void ClimbSmall(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatingControllers.Animator.SetInt("ClimbType", 0);
        _ctx.AnimatingControllers.Animator.SetBool("Climb", true);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatingControllers.Animator.SetBool("Climb", false);
            _ctx.transform.LeanMove(finalClimbPosition, 0.5f).setOnComplete(() =>
            {
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
    private void ClimbMid(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatingControllers.Animator.SetInt("ClimbType", 1);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition - _ctx.transform.forward/2, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatingControllers.Animator.SetBool("Climb", true);
            _ctx.transform.LeanMove(finalClimbPosition, 0.5f).setOnComplete(() =>
            {
                _ctx.AnimatingControllers.Animator.SetBool("Climb", false);
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
    private void ClimbHigh(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatingControllers.Animator.SetInt("ClimbType", 2);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition - _ctx.transform.forward / 2, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatingControllers.Animator.SetBool("Climb", true);
            _ctx.transform.LeanMoveY(finalClimbPosition.y / 2, 0.33f).setOnComplete(() =>
            {
                _ctx.AnimatingControllers.Animator.SetBool("Climb", false);
                _ctx.transform.LeanMove(finalClimbPosition, 0.66f).setOnComplete(() =>
                {
                    _ctx.SwitchController.SwitchTo.Idle();
                });
            });
        });
    }
}
