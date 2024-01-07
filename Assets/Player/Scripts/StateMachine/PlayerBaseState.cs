using System;
using System.Collections.Generic;

namespace Player
{
    public abstract class PlayerBaseState
    {
        protected PlayerStateMachineContext _ctx;
        protected PlayerBaseState _currentSubState;

        public PlayerBaseState(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;
        }

        public virtual void InitializeSubState() { }
        public void UpdateStates()
        {
            Update();

            if (_currentSubState == null) return;

            Type nextState = _currentSubState.GetNextState();
            if (nextState == _currentSubState.GetType())
            {
                _currentSubState.UpdateStates();
                return;
            }

            ChangeSubState(nextState);
        }
        private void ChangeSubState(Type p_nextState)
        {
            _currentSubState.Exit();
            _currentSubState = _ctx.States[p_nextState];
            _currentSubState.Enter();
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        public abstract Type GetNextState();
    }
}