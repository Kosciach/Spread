using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerState_Idle : PlayerBaseState
    {
        public PlayerState_Idle(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }



        public override void Enter()
        {
            _ctx.Movement.OnGround.SetIdleSpeed();
            _ctx.Animator.applyRootMotion = _ctx.Movement.OnGround.UseRootMotionMovement;
        }
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

            _ctx.SetStateEmblem(StateEmblems.Idle);
        }
    }
}