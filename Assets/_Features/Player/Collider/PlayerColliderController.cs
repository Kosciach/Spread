using System;
using System.Linq;
using UnityEngine;
using SaintsField.Playa;
using AYellowpaper.SerializedCollections;

namespace Spread.Player.Collisions
{
    using StateMachine;

    public class PlayerColliderController : PlayerControllerBase
    {
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private CharacterController _characterController;

        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField, SerializedDictionary("State", "ColliderSize")] private SerializedDictionary<string, ColliderSize> _colliderSizes;
        [SerializeField] private Vector3 _centerOffset;

        private Vector3 _center;

        protected override void OnSetup()
        {
            _center = _characterController.center;
            StateTransiton((null, typeof(IdleState)));
            
            _ctx.OnStateTransition += StateTransiton;
        }

        private void StateTransiton((Type OldState, Type NewState) p_transition)
        {
            if (p_transition.NewState == null)
                return;

            string stateTypeString = p_transition.NewState.Name.Split(".").Last();
            if (!_colliderSizes.ContainsKey(stateTypeString))
                return;

            float height = _colliderSizes[stateTypeString].Height;
            Vector3 center = _center;
            center.y = height / 2;

            _characterController.height = height;
            _characterController.center = center + _centerOffset;
        }

        internal void ToggleCollision(bool p_enable)
        {
            _characterController.detectCollisions = p_enable;
        }

        [Serializable]
        private struct ColliderSize
        {
            public float Height;
        }
    }
}