using System;
using DG.Tweening;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    public class CrawlState : PlayerBaseState
    {
        protected override void OnEnter()
        {
            Vector3 inputNormalized = _ctx.MovementController.MoveInputVector.normalized;
            Vector3 dir = (_ctx.Transform.forward * inputNormalized.z) + (_ctx.Transform.right * inputNormalized.x);
            _ctx.Transform.DOMove(_ctx.Transform.position + dir, 0.5f);

            _ctx.AnimatorController.SetCrawlLayer(1);
            _ctx.AnimatorController.ToggleRootMotion(false);
            _ctx.AnimatorController.ToggleFootIk(false);

            _ctx.InteractionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _ctx.CameraController.MoveCamera();
            _ctx.CrouchController.CrawlMovement();
        }

        protected override void OnExit()
        {
            Vector3 inputNormalized = _ctx.MovementController.MoveInputVector.normalized;
            Vector3 dir = (_ctx.Transform.forward * inputNormalized.z) + (_ctx.Transform.right * inputNormalized.x);
            _ctx.Transform.DOMove(_ctx.Transform.position + dir, 0.5f);

            _ctx.AnimatorController.SetCrawlLayer(0);
            _ctx.AnimatorController.ToggleRootMotion(true);
            _ctx.AnimatorController.ToggleFootIk(true);
        }

        internal override Type GetNextState()
        {
            if (!_ctx.CrouchController.IsCrawlArea)
            {
                return typeof(CrouchWalkState);
            }

            return GetType();
        }
    }
}