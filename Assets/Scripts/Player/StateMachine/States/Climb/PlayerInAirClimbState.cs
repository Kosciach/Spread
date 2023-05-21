using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirClimbState : PlayerBaseState
{
    public PlayerInAirClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatController.TemporaryUnEquip();
        ClimbEnterExit(false);
        _ctx.CineCameraController.VerticalController.RotateToAngle(0, 0.3f);

        Climb(_ctx.ClimbController.GetFinalClimbPosition(), _ctx.ClimbController.GetStartClimbPosition());
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
        _ctx.AnimatorController.SetBool("Climb", false);

        _ctx.CombatController.RecoverFromTemporaryUnEquip();
    }


    private void ClimbEnterExit(bool enable)
    {
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(enable);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, enable, 6);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, enable, 6);
        _ctx.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, enable, 6);

        _ctx.ColliderController.ToggleCollider(enable);
        _ctx.VerticalVelocityController.GravityController.ToggleApplyGravity(enable);
        _ctx.CineCameraController.ToggleCineInput(enable);
    }




    private void Climb(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatorController.SetInt("ClimbType", 2);

        LeanTween.cancel(_ctx.gameObject);

        _ctx.AnimatorController.SetBool("Climb", true);
        _ctx.transform.LeanMoveY(finalClimbPosition.y - 1.3f, 0.5f).setOnComplete(() =>
        {
            _ctx.transform.LeanMove(finalClimbPosition, 0.5f).setOnComplete(() =>
            {
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
}
