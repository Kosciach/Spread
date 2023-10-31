using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerState_Jump : PlayerBaseState
    {
        public PlayerState_Jump(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }



        public override void Enter()
        {
            _ctx.Animator.applyRootMotion = false;
            _ctx.Animator.SetTrigger("Jump");
            _ctx.VerticalVel.Jump.ApplyJumpForce();
            _ctx.Movement.InAir.SetJumpSmoothTime();
        }
        public override void Update()
        {
            _ctx.Movement.InAir.Movement();
        }
        public override void LateUpdate()
        {
            _ctx.Camera.Look.Look();
        }
        public override void CheckStateChange()
        {
            if (_ctx.VerticalVel.Gravity.CurrentGravityForce < 0) ChangeState(_factory.Fall());

            _ctx.SetStateEmblem(StateEmblems.Jump);
        }
    }
}