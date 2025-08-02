using System;

namespace Spread.Player.StateMachine
{
    using Camera;
    using Movement;
    using Interactions;
    using Gravity;

    public class RunState : PlayerBaseState
    {
        private PlayerCameraController _cameraController;
        private PlayerMovementController _movementController;
        private PlayerInteractionsController _interactionsController;
        private PlayerGravityController _gravityController;
        private PlayerSlideController _slideController;
        private PlayerSlopeController _slopeController;

        protected override void OnSetup()
        {
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slideController = _ctx.GetController<PlayerSlideController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
        }
        
        protected override void OnUpdate()
        {
            _cameraController.MoveCamera();
            _movementController.NormalMovement();
            _interactionsController.CheckInteractables();
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

            if (_slideController.IsSlide)
            {
                return typeof(SlideState);
            }

            if (_slopeController.IsSlopeSlide)
            {
                return typeof(SlopeSlideState);
            }

            switch (_movementController.MovementType)
            {
                case MovementTypes.Idle:
                    return _movementController.IdleType == IdleTypes.Normal
                        ? typeof(IdleState)
                        : typeof(CrouchIdleState);
                case MovementTypes.Crouch:
                    return typeof(CrouchWalkState);
                case MovementTypes.Walk:
                    return typeof(WalkState);
                case MovementTypes.Jog:
                    return typeof(JogState);
                case MovementTypes.Run:
                    return typeof(RunState);
            }

            return GetType();
        }
    }
}