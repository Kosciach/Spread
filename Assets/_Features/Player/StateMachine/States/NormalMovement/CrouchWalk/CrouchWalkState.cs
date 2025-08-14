using System;

namespace Spread.Player.StateMachine
{
    using Animating;
    using Interactions;
    using Movement;
    using Gravity;
    using Camera;

    public class CrouchWalkState : PlayerBaseState
    {
        private PlayerAnimatorController _animatorController;
        private PlayerInteractionsController _interactionsController;
        private PlayerMovementController _movementController;
        private PlayerGravityController _gravityController;
        private PlayerSlopeController _slopeController;
        private PlayerCrouchController _crouchController;
        private PlayerCameraController _cameraController;

        protected override void OnSetup()
        {
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
            _crouchController = _ctx.GetController<PlayerCrouchController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
        }

        protected override void OnEnter()
        {
            _interactionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _cameraController.MoveCamera();
            _slopeController.SlopeSlide();
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

            if (_slopeController.IsSlopeSlide)
            {
                return typeof(SlopeSlideState);
            }

            if (_crouchController.IsCrawlArea && !_animatorController.TransitioningToCrawl)
            {
                return typeof(CrawlState);
            }

            return _movementController.NextMovementState;
        }
    }
}