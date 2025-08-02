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
        private PlayerMovementController _movementController;
        private PlayerGravityController _gravityController;

        protected override void OnSetup()
        {
            _slideController = _ctx.GetController<PlayerSlideController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();;
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();;
            _movementController = _ctx.GetController<PlayerMovementController>();;
            _gravityController = _ctx.GetController<PlayerGravityController>();;
        }

        protected override void OnEnter()
        {
            _slideController.StartSlide();
            _animatorController.SetSlideLayer(1);
            _interactionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _movementController.RestrainNormalMovement();
        }

        protected override void OnExit()
        {
            _slideController.StopSlide();
            _animatorController.SetSlideLayer(0);
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