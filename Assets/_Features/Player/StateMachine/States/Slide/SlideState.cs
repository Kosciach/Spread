using System;

namespace Spread.Player.StateMachine
{
    public class SlideState : PlayerBaseState
    {
        protected override void OnEnter()
        {
            _ctx.SlideController.StartSlide();
            _ctx.AnimatorController.SetSlideLayer(1);
            _ctx.InteractionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _ctx.MovementController.RestrainNormalMovement();
        }

        protected override void OnExit()
        {
            _ctx.SlideController.StopSlide();
            _ctx.AnimatorController.SetSlideLayer(0);
        }

        internal override Type GetNextState()
        {

            if (_ctx.GravityController.IsFalling)
            {
                return typeof(FallState);
            }

            if (_ctx.GravityController.IsJump)
            {
                return typeof(JumpState);
            }
            
            if (!_ctx.SlideController.IsSlide)
            {
                return typeof(IdleState);
            }

            return GetType();
        }
    }
}