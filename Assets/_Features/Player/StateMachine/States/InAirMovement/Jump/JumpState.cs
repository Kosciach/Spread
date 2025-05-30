using System;
using NaughtyAttributes;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    public class JumpState : PlayerBaseState
    {
        [SerializeField, Label("XZ Ladder Jump Force")] private float _xzLadderJumpForce = 10;
        [SerializeField] private float _yLadderJumpForce = 1;

        protected override void OnEnter()
        {
            _ctx.GravityController.AddJumpForce();
            _ctx.AnimatorController.SetInAirLayer(1);
            _ctx.AnimatorController.Jump();
            _ctx.InteractionsController.SetInteractable(null);

            if(_ctx.LastState is LadderState)
            {
                Transform cameraTransform = _ctx.CameraController.Main.transform;
                Vector3 xzVelocity = new Vector3(cameraTransform.transform.forward.x, 0, cameraTransform.transform.forward.z);
                _ctx.MovementController.PushInAir(xzVelocity * _xzLadderJumpForce);

                float gravityForce = Mathf.Max(0, cameraTransform.forward.y);
                _ctx.GravityController.AddGravity(gravityForce * _yLadderJumpForce);
            }
        }

        protected override void OnUpdate()
        {
            _ctx.CameraController.MoveCamera();
            _ctx.MovementController.InAirMovement();
        }

        protected override void OnExit()
        {

        }

        internal override Type GetNextState()
        {
            if (_ctx.GravityController.IsFalling)
            {
                return typeof(FallState);
            }

            if (!_ctx.GravityController.IsJump)
            {
                return typeof(FallState);
            }

            return GetType();
        }
    }
}