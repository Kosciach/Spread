using System;

namespace Spread.Player.StateMachine
{
    public class WalkState : PlayerBaseState
    {
        protected override void OnEnter()
        {

        }

        protected override void OnUpdate()
        {
            _ctx.CameraController.MoveCamera();
            _ctx.MovementController.NormalMovement();
            _ctx.InteractionsController.CheckInteractables();
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

            if (_ctx.GravityController.IsJump)
            {
                return typeof(JumpState);
            }

            if (_ctx.SlopeController.IsSlopeSlide)
            {
                return typeof(SlopeSlideState);
            }
            
            switch (_ctx.MovementController.MovementType)
            {
                case Movement.MovementTypes.Idle:
                    return _ctx.MovementController.IdleType is Movement.IdleTypes.Normal
                        ? typeof(IdleState) : typeof(CrouchIdleState);
                case Movement.MovementTypes.Crouch:
                    return typeof(CrouchWalkState);
                case Movement.MovementTypes.Walk:
                    return typeof(WalkState);
                case Movement.MovementTypes.Jog:
                    return typeof(JogState);
                case Movement.MovementTypes.Run:
                    return typeof(RunState);
            }

            return GetType();
        }
    }
}