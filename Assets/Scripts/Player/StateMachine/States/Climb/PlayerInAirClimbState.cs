using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerInAirClimbState : PlayerBaseState
{
    private float _capturedFallingTime = 0;
    public PlayerInAirClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        GetFallingTime();
        _ctx.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip(false, 0.5f);
        ClimbEnterExit(false);
        CheckClimbType();
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



    private void GetFallingTime()
    {
        _capturedFallingTime = _ctx.MovementControllers.VerticalVelocity.Gravity.FallingTime;
        Debug.Log(_capturedFallingTime);
    }
    private void ClimbEnterExit(bool enable)
    {
        _ctx.AnimatingControllers.Animator.ToggleLayer(LayersEnum.TopBodyStabilizer, enable, 0.5f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, enable, 0.1f);
        _ctx.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, enable, 0.1f);

        _ctx.CoreControllers.Collider.ToggleCollider(enable);
        _ctx.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(enable);
        _ctx.CameraControllers.Cine.ToggleCineInput(enable);
    }


    private void CheckClimbType()
    {
        //if (_capturedFallingTime <= 1.1f) ClimbNormal(_ctx.StateControllers.Climb.FinalClimbPosition, _ctx.StateControllers.Climb.StartClimbPosition);
        ClimbNormal(_ctx.StateControllers.Climb.FinalClimbPosition, _ctx.StateControllers.Climb.StartClimbPosition);
    }
    private void ClimbNormal(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatingControllers.Animator.SetInt("ClimbType", 2);
        _ctx.AnimatingControllers.Animator.SetBool("Climb", true);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMoveY(finalClimbPosition.y - 1.3f, 0.5f).setOnComplete(() =>
        {
            _ctx.transform.LeanMove(finalClimbPosition, 0.5f).setOnComplete(() =>
            {
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
        _ctx.AnimatingControllers.Animator.SetInt("ClimbType", 2);
    }
}
