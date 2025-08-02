using System;
using DG.Tweening;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    using Animating;
    using Interactions;
    using Movement;
    using Camera;

    public class CrawlState : PlayerBaseState
    {
        private PlayerAnimatorController _animatorController;
        private PlayerInteractionsController _interactionsController;
        private PlayerMovementController _movementController;
        private PlayerCameraController _cameraController;
        private PlayerCrouchController _crouchController;

        protected override void OnSetup()
        {
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _crouchController = _ctx.GetController<PlayerCrouchController>();
        }

        protected override void OnEnter()
        {
            Vector3 inputNormalized = _movementController.MoveInputVector.normalized;
            Vector3 dir = (_ctx.Transform.forward * inputNormalized.z) + (_ctx.Transform.right * inputNormalized.x);
            _ctx.Transform.DOMove(_ctx.Transform.position + dir, 0.5f);

            _animatorController.SetCrawlLayer(1);
            _animatorController.ToggleRootMotion(false);
            _animatorController.ToggleFootIk(false);

            _interactionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _cameraController.MoveCamera();
            _crouchController.CrawlMovement();
        }

        protected override void OnExit()
        {
            Vector3 inputNormalized = _movementController.MoveInputVector.normalized;
            Vector3 dir = (_ctx.Transform.forward * inputNormalized.z) + (_ctx.Transform.right * inputNormalized.x);
            _ctx.Transform.DOMove(_ctx.Transform.position + dir, 0.5f);

            _animatorController.SetCrawlLayer(0);
            _animatorController.ToggleRootMotion(true);
            _animatorController.ToggleFootIk(true);
        }

        internal override Type GetNextState()
        {
            if (!_crouchController.IsCrawlArea)
            {
                return typeof(CrouchWalkState);
            }

            return GetType();
        }
    }
}