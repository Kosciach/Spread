using System;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    using Gravity;
    using Animating;
    using Interactions;
    using Movement;
    using Camera;
    using Ladder;

    public class JumpState : PlayerBaseState
    {
        [SerializeField] private float _xzLadderJumpForce = 10;
        [SerializeField] private float _yLadderJumpForce = 1;

        private PlayerGravityController _gravityController;
        private PlayerAnimatorController _animatorController;
        private PlayerInteractionsController _interactionsController;
        private PlayerMovementController _movementController;
        private PlayerCameraController _cameraController;
        private PlayerLadderController _ladderController;

        protected override void OnSetup()
        {
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _ladderController = _ctx.GetController<PlayerLadderController>();
        }

        protected override void OnEnter()
        {
            _gravityController.AddJumpForce();
            _animatorController.SetInAirLayer(1);
            _animatorController.Jump();
            _interactionsController.SetInteractable(null);

            TryJumpFromLadder();
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

            if (_gravityController.IsFalling || !_gravityController.IsJump)
            {
                return typeof(FallState);
            }

            return GetType();
        }

        private void TryJumpFromLadder()
        {
            if (_ctx.LastState is not LadderState ladderState)
                return;

            // Reset ladder behavior
            ladderState.QuickLadderExit(0.1f);

            // Calculate jump direction
            Transform cameraTransform = _cameraController.Main.transform;
            Vector3 xzVelocity = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
            _movementController.PushInAir(xzVelocity * _xzLadderJumpForce);

            float gravityForce = Mathf.Max(0, cameraTransform.forward.y);
            _gravityController.AddGravity(gravityForce * _yLadderJumpForce);
        }
    }
}