using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerState_Run : PlayerBaseState
    {
        public PlayerState_Run(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }



        public override void Enter()
        {
            _ctx.Movement.OnGround.SetRunSpeed();
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
            if (!_ctx.Input.IsWalk) ChangeState(_factory.Idle());
            else if (!_ctx.Input.IsRun || _ctx.Input.MovementInputVector.y <= 0) ChangeState(_factory.Walk());

            _ctx.SetStateEmblem(StateEmblems.Run);
        }
    }
}
