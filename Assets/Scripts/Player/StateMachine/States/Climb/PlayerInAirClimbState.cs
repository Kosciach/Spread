using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirClimbState : PlayerBaseState
{
    public PlayerInAirClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CameraRotateController.SetHandsCameraRotation(PlayerCameraRotateController.HandsCameraRotationsEnum.Climb, 5);

        _ctx.GravityController.ToggleApplyGravity(false);
        _ctx.ColliderController.ToggleCollider(false);
        _ctx.CameraController.ToggleLockCamera(true);

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
        _ctx.AnimatorController.SetBool("Climb", false);
        _ctx.ColliderController.ToggleCollider(true);
        _ctx.GravityController.ToggleApplyGravity(true);
        _ctx.CameraController.ToggleLockCamera(false);
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
