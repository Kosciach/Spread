using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerState_Idle : PlayerBaseState
    {
        public PlayerState_Idle(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }


        public override void Update()
        {
            _ctx.Movement.OnGround.Movement();
        }
        public override void LateUpdate()
        {
            _ctx.Camera.Look.Look();
        }
        public override void CheckStateChange()
        {
            if (_ctx.Input.IsWalk) ChangeState(_factory.Walk());
            else if (!_ctx.VerticalVel.GroundCheck.IsGrounded) ChangeState(_factory.Fall());
            else if (_ctx.VerticalVel.Jump.IsJump) ChangeState(_factory.Jump());
            else if (_ctx.Movement.Crouch.IsCrouch) ChangeState(_factory.Crouch());

            _ctx.SetStateEmblem(StateEmblems.Idle);
        }
    }
}