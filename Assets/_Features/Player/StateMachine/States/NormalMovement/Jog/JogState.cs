using System;

namespace Spread.Player.StateMachine
{
    using Movement;
    using Interactions;
    using Gravity;
    using Camera;
    using Ladder;

    public class JogState : PlayerBaseState
    {
        private PlayerMovementController _movementController;
        private PlayerInteractionsController _interactionsController;
        private PlayerGravityController _gravityController;
        private PlayerCameraController _cameraController;
        private PlayerLadderController _ladderController;
        private PlayerSlopeController _slopeController;
        private PlayerSlideController _slideController;

        protected override void OnSetup()
        {
            _movementController = _ctx.GetController<PlayerMovementController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _ladderController = _ctx.GetController<PlayerLadderController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
            _slideController = _ctx.GetController<PlayerSlideController>();
        }
        
        protected override void OnUpdate()
        {
            _cameraController.MoveCamera();
            _slopeController.SlopeSlide();
            _slideController.Slide(false);
            _interactionsController.CheckInteractables();
        }
        
        internal override Type GetNextState()
        {
            if (_ladderController.CurrentLadder != null)
            {
                return typeof(EnterLadderState);
            }

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