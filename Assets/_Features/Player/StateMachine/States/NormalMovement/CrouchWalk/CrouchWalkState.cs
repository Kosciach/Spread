using System;

namespace Spread.Player.StateMachine
{
    public class CrouchWalkState : PlayerBaseState
    {
        protected override void OnEnter()
        {
            _ctx.InteractionsController.SetInteractable(null);
        }

        protected override void OnUpdate()
        {
            _ctx.CameraController.MoveCamera();
            _ctx.MovementController.NormalMovement();
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

            if(_ctx.CrouchController.IsCrawlArea && !_ctx.AnimatorController.TransitioningToCrawl)
            {
                return typeof(CrawlState);
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