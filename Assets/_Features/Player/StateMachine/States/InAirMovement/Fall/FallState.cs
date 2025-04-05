using System;

namespace Spread.Player.StateMachine
{
    public class FallState : PlayerBaseState
    {
        protected override void OnEnter()
        {
            if (_ctx.LastState.GetType() != typeof(JumpState))
            {
                _ctx.AnimatorController.SetInAirLayer(1);
                _ctx.AnimatorController.Fall();
            }

            _ctx.InteractionsController.SetInteractable(null);
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
            if (!_ctx.GravityController.IsFalling)
            {
                return typeof(LandState);
            }

            return GetType();
        }
    }
}