using System;
using DG.Tweening;

namespace Spread.Player.StateMachine
{
    public class LadderState : PlayerBaseState
    {
        protected override void OnEnter()
        {

        }

        protected override void OnUpdate()
        {
            _ctx.LadderController.ClimbLadder();
        }

        protected override void OnExit()
        {
            //Exit by Input
            if (_ctx.LadderController.CurrentLadder == null)
            {
                ExitMidClimb();
                _ctx.Transform.DOMove(_ctx.Transform.position - _ctx.Transform.forward.normalized * 0.5f, 0.1f);
            }

            //Exit with Jump
            if (_ctx.GravityController.IsJump)
            {
                ExitMidClimb();
            }
        }

        internal override Type GetNextState()
        {
            if (_ctx.GravityController.IsJump)
            {
                return typeof(JumpState);
            }

            if (!_ctx.LadderController.UsingLadder)
            {
                return typeof(ExitLadderState);
            }

            if (_ctx.LadderController.CurrentLadder == null)
            {
                return typeof(IdleState);
            }

            return GetType();
        }

        private void ExitMidClimb()
        {
            _ctx.AnimatorController.ToggleFootIk(true);
            _ctx.AnimatorController.SetLadderRig(0);
            _ctx.AnimatorController.LadderExit(false);

            _ctx.GravityController.ToggleGravity(true);
            _ctx.GravityController.ToggleIkCrouch(true);

            _ctx.CameraController.ResetMinMax();
            _ctx.CameraController.ToggleWrap(true);

            _ctx.LadderController.ClearUp();

            _ctx.MovementController.RootMotionMove = true;
        }
    }
}