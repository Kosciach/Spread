using System;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

namespace Spread.Player.Collisions
{
    using StateMachine;

    public class PlayerColliderController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [BoxGroup("References"), SerializeField] private CharacterController _characterController;

        [BoxGroup("Settings"), SerializeField, SerializedDictionary("State", "ColliderSize")] private SerializedDictionary<string, ColliderSize> _colliderSizes;
        [BoxGroup("Settings"), SerializeField, SerializedDictionary("State", "ColliderSize")] private Vector3 _centerOffset;

        private Vector3 _center;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;
            _ctx.OnStateTransition += StateTransiton;

            _center = _characterController.center;
            StateTransiton((null, typeof(IdleState)));
        }

        private void StateTransiton((Type OldState, Type NewState) p_transition)
        {
            if (p_transition.NewState == null) return;

            string stateTypeString = p_transition.NewState.Name.Split(".").Last();
            if (!_colliderSizes.ContainsKey(stateTypeString)) return;

            float height = _colliderSizes[stateTypeString].Height;
            Vector3 center = _center;
            center.y = height / 2;

            _characterController.height = height;
            _characterController.center = center + _centerOffset;
        }

        [System.Serializable]
        private struct ColliderSize
        {
            public float Height;
        }
    }
}