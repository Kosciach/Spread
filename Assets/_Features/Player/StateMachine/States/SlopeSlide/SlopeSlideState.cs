using System;
using Spread.Player.Animating;
using Spread.Player.Camera;
using Spread.Player.Gravity;
using Spread.Player.Interactions;
using Spread.Player.Movement;

namespace Spread.Player.StateMachine
{
    public class SlopeSlideState : PlayerBaseState
    {
        private PlayerAnimatorController _animatorController;
        private PlayerInteractionsController _interactionsController;
        private PlayerCameraController _cameraController;
        private PlayerGravityController _gravityController;
        private PlayerSlopeController _slopeController;
        private PlayerMovementController _movementController;
        
        protected override void OnSetup()
        {
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
        }
        
        protected override void OnEnter()
        {
            _animatorController.SlopeSlide(true);
            _interactionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _cameraController.MoveCamera();
        }

        protected override void OnExit()
        {
            _animatorController.SlopeSlide(false);
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

            return _movementController.NextMovementState;
        }
    }
}