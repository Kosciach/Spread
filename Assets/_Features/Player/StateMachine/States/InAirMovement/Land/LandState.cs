using System;

namespace Spread.Player.StateMachine
{
    public class LandState : PlayerBaseState
    {
        protected override void OnEnter()
        {
            _ctx.AnimatorController.Land();
            _ctx.InteractionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            if(_ctx.AnimatorController.CurrentStateName[2] != "Landing_Hard"
            || _ctx.AnimatorController.PreviousStateName[2] != "Landing_Hard")
            {
                _ctx.CameraController.MoveCamera();
                _ctx.MovementController.NormalMovement();
            }
        }

        protected override void OnExit()
        {
            _ctx.AnimatorController.SetInAirLayer(0);
        }

        internal override Type GetNextState()
        {
            if (_ctx.AnimatorController.CurrentStateName[2] == "Empty")
            {
                return typeof(IdleState);
            }

            return GetType();
        }
    }
}