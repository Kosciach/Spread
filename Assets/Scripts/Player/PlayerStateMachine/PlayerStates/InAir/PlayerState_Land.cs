using KosciachTools.Delay;
using UnityEngine;

namespace PlayerStateMachineSystem
{
    public class PlayerState_Land : PlayerBaseState
    {
        private LandBehaviour _landAnimatorBehaviour;
        public PlayerState_Land(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { _landAnimatorBehaviour = _ctx.Animator.GetBehaviour<LandBehaviour>(); }



        public override void Enter()
        {
            _ctx.Animator.applyRootMotion = false;
            _ctx.Animator.SetTrigger("Land");

            _ctx.Movement.InAir.SetLandSmoothTime();
        }
        public override void Update()
        {
            _ctx.Movement.InAir.Movement();
            _ctx.Animator.SetFloat("FallingForwardVelocity", Mathf.Abs(Vector3.Dot(_ctx.Velocity.CurrentVelocity, _ctx.transform.forward)), 0.1f, Time.deltaTime);

        }
        public override void LateUpdate()
        {
            _ctx.Camera.Look.Look();
        }
        public override void CheckStateChange()
        {
            if (_landAnimatorBehaviour.HandLandEnded) ChangeState(_factory.Idle());

            _ctx.SetStateEmblem(StateEmblems.Land);
        }
    }
}