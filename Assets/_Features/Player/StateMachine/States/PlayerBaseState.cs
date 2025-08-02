using System;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    public abstract class PlayerBaseState : MonoBehaviour
    {
        protected PlayerStateMachineContext _ctx { get; private set; }

        [SerializeField] private bool _isCrouchState;
        public bool IsCrouchState => _isCrouchState;


        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            gameObject.SetActive(false);
            OnSetup();
        }

        internal void Dispose()
        {
            OnDispose();
        }

        internal void EnterState()
        {
            gameObject.SetActive(true);
            OnEnter();
        }

        internal void UpdateState()
        {
            OnUpdate();
        }

        internal void ExitState()
        {
            OnExit();
            gameObject.SetActive(false);
        }

        protected virtual void OnSetup() { }
        protected virtual void OnDispose() { }
        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }

        internal virtual Type GetNextState()
        {
            return null;
        }
    }

    public enum StateCategory { Normal, Jumpable, InAir, Crouch }
}
