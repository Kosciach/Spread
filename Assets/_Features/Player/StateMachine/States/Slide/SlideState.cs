using System;

namespace Spread.Player.StateMachine
{
    using Movement;
    using Animating;
    using Interactions;
    using Gravity;
    
    public class SlideState : PlayerBaseState
    {
        private PlayerSlideController _slideController;
        private PlayerAnimatorController _animatorController;
        private PlayerInteractionsController _interactionsController;
        private PlayerGravityController _gravityController;

        protected override void OnSetup()
        {
            _slideController = _ctx.GetController<PlayerSlideController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
        }

        protected override void OnEnter()
        {
            _slideController.StartSlide();
            _animatorController.SetAnimatorLayerWeight(AnimatorLayer.Slide, 1f);
             _animatorController.ToggleFootIk(false);
            _gravityController.ToggleIkCrouch(false);
            _interactionsController.SetInteractable(null);
        }
        
        protected override void OnExit()
        {
            _animatorController.SetAnimatorLayerWeight(AnimatorLayer.Slide, 0f);
            _animatorController.ToggleFootIk(true);
            _gravityController.ToggleIkCrouch(true);
        }

        protected override void OnUpdate()
        {
            _slideController.Slide(true);
        }

        internal override Type GetNextState()
        {
            if (_gravityController.IsFalling)
            {
                return typeof(FallState);
            }

            if (_gravityController.IsJump)
            {
                return typeof(JumpState);
            }

            if (!_slideController.IsSlide)
            {
                return typeof(IdleState);
            }

            return GetType();
        }
    }
}