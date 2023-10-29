using UnityEngine;

namespace PlayerStateMachineSystem
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachine _ctx;
        protected PlayerStateFactory _factory;

        public PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory)
        {
            _ctx = ctx;
            _factory = factory;
        }


        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
        public virtual void Exit() { }
        public virtual void CheckStateChange() { }


        protected void ChangeState(PlayerBaseState newState)
        {
            Exit();
            _ctx.CurrentState = newState;
            _ctx.CurrentState.Enter();
        }
    }
}