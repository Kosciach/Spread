using System;

namespace Spread.Player.StateMachine
{
    using Movement;
    using Animating;
    using Interactions;
    using Gravity;
    using Ladder;
    using Camera;

    public class FallState : PlayerBaseState
    {
        private PlayerMovementController _movementController;
        private PlayerAnimatorController _animatorController;
        private PlayerInteractionsController _interactionsController;
        private PlayerGravityController _gravityController;
        private PlayerLadderController _ladderController;
        private PlayerCameraController _cameraController;
        private PlayerSlideController _slideController;

        protected override void OnSetup()
        {
            _movementController = _ctx.GetController<PlayerMovementController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _ladderController = _ctx.GetController<PlayerLadderController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _slideController = _ctx.GetController<PlayerSlideController>();
        }

        protected override void OnEnter()
        {
            _slideController.ResetSlide();
            
            if (_ctx.LastState.GetType() != typeof(JumpState))
            {
                _animatorController.SetAnimatorLayerWeight(AnimatorLayer.InAir, 1f);
                _animatorController.Fall();
            }

            _interactionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _cameraController.MoveCamera();
            _movementController.InAirMovement();
            _interactionsController.CheckInteractables();
        }

        internal override Type GetNextState()
        {
            if (_ladderController.CurrentLadder != null)
            {
                return typeof(EnterLadderState);
            }

            if (!_gravityController.IsFalling)
            {
                return typeof(LandState);
            }

            return GetType();
        }
    }

}