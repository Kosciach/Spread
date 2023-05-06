using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirClimbState : PlayerBaseState
{
    public PlayerInAirClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(false);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, false, 6);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, false, 6);

        _ctx.VerticalVelocityController.GravityController.ToggleApplyGravity(false);
        _ctx.ColliderController.ToggleCollider(false);
        _ctx.CineCameraController.ToggleCineInput(false);
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
        _ctx.HandsCameraController.EnableController.ToggleHandsCamera(true);
        _ctx.AnimatorController.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, true, 6);
        _ctx.IkLayerController.SetLayerWeight(PlayerIkLayerController.LayerEnum.HeadBody, true, 6);

        _ctx.AnimatorController.SetBool("Climb", false);
        _ctx.ColliderController.ToggleCollider(true);
        _ctx.VerticalVelocityController.GravityController.ToggleApplyGravity(true);
        _ctx.CineCameraController.ToggleCineInput(true);
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
