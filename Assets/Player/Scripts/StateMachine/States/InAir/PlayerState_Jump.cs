using System;

namespace Player
{
    public class PlayerState_Jump : PlayerBaseState
    {

        public PlayerState_Jump(PlayerStateMachineContext p_ctx) : base(p_ctx) { }

        public override void InitializeSubState()
        {

        }


        public override void Enter()
        {
            _ctx.MovementController.IsCrouch = false;
            _ctx.GravityController.AddGravityForce(_ctx.MovementController.JumpForce);
            _ctx.MovementController.SaveMaxInAirSpeed();
            _ctx.AnimatorController.Jump();
        }

        public override void Update()
        {
            _ctx.CameraController.Look();
            _ctx.MovementController.NormalMove();
        }

        public override void Exit()
        {
            _ctx.MovementController.IsJump = false;
        }

        public override Type GetNextState()
        {
            if (_ctx.GravityController.CurrentGravityForce <= 0)
            {
                return typeof(PlayerState_Fall);
            }
            return GetType();
        }
    }
}