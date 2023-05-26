using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirClimbState : PlayerBaseState
{
    public PlayerInAirClimbState(PlayerStateMachine ctx, PlayerStateFactory factory, string stateName) : base(ctx, factory, stateName) { }


    public override void StateEnter()
    {
        _ctx.CombatControllers.Combat.TemporaryUnEquip();
        ClimbEnterExit(false);
        _ctx.CameraControllers.Cine.VerticalController.RotateToAngle(0, 0.3f);

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

        _ctx.CombatControllers.Combat.RecoverFromTemporaryUnEquip();
    }


    private void ClimbEnterExit(bool enable)
    {
        _ctx.CameraControllers.Hands.EnableController.ToggleHandsCamera(enable);
        _ctx.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.TopBodyStabilizer, enable, 6);
        _ctx.AnimatingControllers.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.Body, enable, 6);
        _ctx.AnimatingControllers.IkController.Layers.SetLayerWeight(PlayerIkLayerController.LayerEnum.SpineLock, enable, 6);

        _ctx.CoreControllers.Collider.ToggleCollider(enable);
        _ctx.MovementControllers.VerticalVelocity.GravityController.ToggleApplyGravity(enable);
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
