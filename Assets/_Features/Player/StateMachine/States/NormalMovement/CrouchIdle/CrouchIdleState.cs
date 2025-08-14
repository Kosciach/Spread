using System;

namespace Spread.Player.StateMachine
{
    using Interactions;
    using Camera;
    using Movement;
    using Gravity;

    public class CrouchIdleState : PlayerBaseState
    {
        private PlayerInteractionsController _interactionsController;
        private PlayerCameraController _cameraController;
        private PlayerMovementController _movementController;
        private PlayerGravityController _gravityController;
        private PlayerSlopeController _slopeController;

        protected override void OnSetup()
        {
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
        }

        protected override void OnEnter()
        {
            _interactionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _cameraController.IdleCamera();
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