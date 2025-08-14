using System;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    using Camera;
    using Movement;
    using Interactions;
    using Gravity;
    using Ladder;

    public class IdleState : PlayerBaseState
    {
        private PlayerCameraController _cameraController;
        private PlayerMovementController _movementController;
        private PlayerInteractionsController _interactionsController;
        private PlayerGravityController _gravityController;
        private PlayerSlopeController _slopeController;
        private PlayerLadderController _ladderController;

        protected override void OnSetup()
        {
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
            _ladderController = _ctx.GetController<PlayerLadderController>();
        }
        
        protected override void OnUpdate()
        {
            _cameraController.IdleCamera();
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