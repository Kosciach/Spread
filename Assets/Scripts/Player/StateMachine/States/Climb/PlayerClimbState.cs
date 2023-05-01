using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
    public PlayerClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.GravityController.ToggleApplyGravity(false);
        _ctx.ColliderController.ToggleCollider(false);


        CheckClimbType(_ctx.ClimbController.GetClimbHeight());
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





    private void CheckClimbType(float climbHeight)
    {
        bool isVault = _ctx.ClimbController.GetIsVault();
        if(isVault)
        {
            Vault(_ctx.ClimbController.GetFinalClimbPosition(), _ctx.ClimbController.GetStartClimbPosition());
            return;
        }

        if (climbHeight >= 0.2f && climbHeight <= 1.1f)
        {
            _ctx.CameraController.ToggleLockCamera(true);
            ClimbSmall(_ctx.ClimbController.GetFinalClimbPosition(), _ctx.ClimbController.GetStartClimbPosition());
        }
        else if (climbHeight > 1.1f && climbHeight <= 2)
        {
            _ctx.CameraController.ToggleLockCamera(true);
            ClimbMid(_ctx.ClimbController.GetFinalClimbPosition(), _ctx.ClimbController.GetStartClimbPosition());
        }
        else if (climbHeight > 2 && climbHeight <= 3.5f)
        {
            _ctx.CameraController.ToggleLockCamera(true);
            ClimbHigh(_ctx.ClimbController.GetFinalClimbPosition(), _ctx.ClimbController.GetStartClimbPosition());
        }
        else
        {
            _ctx.SwitchController.SwitchTo.Idle();
        }
    }



    private void ClimbSmall(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatorController.SetInt("ClimbType", 0);
        _ctx.AnimatorController.SetBool("Climb", true);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatorController.SetBool("Climb", false);
            _ctx.transform.LeanMove(finalClimbPosition, 0.5f).setOnComplete(() =>
            {
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
    private void ClimbMid(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatorController.SetInt("ClimbType", 1);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition - _ctx.transform.forward/2, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatorController.SetBool("Climb", true);
            _ctx.transform.LeanMove(finalClimbPosition, 0.5f).setOnComplete(() =>
            {
                _ctx.AnimatorController.SetBool("Climb", false);
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
    private void ClimbHigh(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatorController.SetInt("ClimbType", 2);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition - _ctx.transform.forward / 2, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatorController.SetBool("Climb", true);
            _ctx.transform.LeanMoveY(finalClimbPosition.y / 2, 0.33f).setOnComplete(() =>
            {
                _ctx.AnimatorController.SetBool("Climb", false);
                _ctx.transform.LeanMove(finalClimbPosition, 0.66f).setOnComplete(() =>
                {
                    _ctx.SwitchController.SwitchTo.Idle();
                });
            });
        });
    }



    private void Vault(Vector3 finalClimbPosition, Vector3 startClimbPosition)
    {
        _ctx.AnimatorController.SetInt("ClimbType", 1);

        LeanTween.cancel(_ctx.gameObject);
        _ctx.transform.LeanMove(startClimbPosition - _ctx.transform.forward / 2, 0.2f).setOnComplete(() =>
        {
            _ctx.AnimatorController.SetBool("Climb", true);
            _ctx.transform.LeanMove(finalClimbPosition - _ctx.transform.forward / 2, 0.3f).setOnComplete(() =>
            {
                _ctx.AnimatorController.SetBool("Climb", false);
                _ctx.SwitchController.SwitchTo.Idle();
            });
        });
    }
}
