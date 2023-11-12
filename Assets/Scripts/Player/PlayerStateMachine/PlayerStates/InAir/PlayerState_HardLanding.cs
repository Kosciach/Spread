using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerState_HardLanding : PlayerBaseState
    {
        private HardLandingBehaviour _hardLandingAnimatorBehaviour;
        public PlayerState_HardLanding(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { _hardLandingAnimatorBehaviour = _ctx.Animator.GetBehaviour<HardLandingBehaviour>(); }



        public override void Enter()
        {
            _ctx.Animator.applyRootMotion = false;
            _ctx.Animator.SetTrigger("HardLanding");
            _ctx.Movement.InAir.SetHardLandingSmoothTime();
        }
        public override void Update()
        {
            _ctx.Movement.OnGround.CalculateMovementSpeeds();
            _ctx.Movement.OnGround.SynchronizeVelocity(Vector3.zero);
            _ctx.Movement.InAir.SynchronizeVelocity(Vector3.zero);
        }
        public override void LateUpdate()
        {

        }
        public override void CheckStateChange()
        {
            if (_hardLandingAnimatorBehaviour.HandLandEnded) ChangeState(_factory.Idle());

            _ctx.SetStateEmblem(StateEmblems.HardLanding);
        }
        public override void Exit()
        {
            _ctx.Movement.Crouch.DisableIsCrouch();
        }
    }
}