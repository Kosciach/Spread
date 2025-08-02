using UnityEngine;

namespace Spread.Player
{
    using StateMachine;
    
    public abstract class PlayerControllerBase : MonoBehaviour
    {
        protected PlayerStateMachineContext _ctx;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;
            OnSetup();
        }

        internal void Tick() => OnTick();
        internal void Dispose() => OnDispose();
        
        protected virtual void OnSetup() {}
        protected virtual void OnTick() {}
        protected virtual void OnDispose() {}
    }
}