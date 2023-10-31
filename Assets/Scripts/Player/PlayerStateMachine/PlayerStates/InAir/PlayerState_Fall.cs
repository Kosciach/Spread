using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerState_Fall : PlayerBaseState
    {
        private float _fallingVelocity = 0;
        private float _fallingForwardVelocity = 0;
        public PlayerState_Fall(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }



        public override void Enter()
        {
            _ctx.Animator.applyRootMotion = false;
            _ctx.Animator.SetTrigger("Fall");
            _ctx.Movement.InAir.SetFallSmoothTime();
        }
        public override void Update()
        {
            _ctx.Movement.InAir.Movement();

            UpdateMovementSpeed();

            _fallingVelocity += Time.deltaTime;
        }
        public override void LateUpdate()
        {
            _ctx.Camera.Look.Look();
        }
        public override void CheckStateChange()
        {
            if (_ctx.VerticalVel.GroundCheck.IsGrounded)
            {
                if (_fallingVelocity * 10 > 10) ChangeState(_factory.HardLanding());
                else ChangeState(_factory.Land());
            }

            _ctx.SetStateEmblem(StateEmblems.Fall);
        }
        public override void Exit()
        {
            _ctx.Animator.SetFloat("FallingVelocity", _fallingVelocity * 10);
            _ctx.Animator.SetFloat("FallingForwardVelocity", Mathf.Abs(Vector3.Dot(_ctx.Velocity.CurrentVelocity, _ctx.transform.forward)));
        }





        private void UpdateMovementSpeed()
        {
            float movementSpeedFromForwardVelocity = Mathf.Round(Mathf.Abs(Vector3.Dot(_ctx.Velocity.CurrentVelocity, _ctx.transform.forward)) / 3) * 3;
            _ctx.Animator.SetFloat("MovementSpeed", movementSpeedFromForwardVelocity);
        }
    }
}