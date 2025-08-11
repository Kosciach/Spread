using System;

namespace Spread.Player.StateMachine
{
    using Animating;
    using Interactions;
    using Movement;
    using Camera;

    public class LandState : PlayerBaseState
    {
        private PlayerAnimatorController _animatorController;
        private PlayerInteractionsController _interactionsController;
        private PlayerMovementController _movementController;
        private PlayerCameraController _cameraController;

        protected override void OnSetup()
        {
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
        }

        protected override void OnEnter()
        {
            _animatorController.Land();
            _interactionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            if (_animatorController.CurrentStateName[2] != "Landing_Hard" &&
                _animatorController.PreviousStateName[2] != "Landing_Hard")
            {
                _cameraController.MoveCamera();
                _movementController.NormalMovement();
            }
        }

        protected override void OnExit()
        {
            _animatorController.SetAnimatorLayerWeight(AnimatorLayer.InAir, 0f);
        }

        internal override Type GetNextState()
        {
            if (_animatorController.CurrentStateName[2] == "Empty")
            {
                return typeof(IdleState);
            }

            return GetType();
        }
    }
}