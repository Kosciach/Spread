using System;
using UnityEngine;
using System.Collections.Generic;

namespace Player
{
    [Serializable]
    public class PlayerStateMachineContext
    {
        [HideInInspector] internal Dictionary<Type, PlayerBaseState> States;
        internal Transform Transform;

        [Header("--Scripts--")]
        [SerializeField] private PlayerInputController _inputController;
        public PlayerInputController InputController => _inputController;

        [SerializeField] private PlayerMovementController _movementController;
        public PlayerMovementController MovementController => _movementController;

        [SerializeField] private PlayerCameraController _cameraController;
        public PlayerCameraController CameraController => _cameraController;

        [SerializeField] private PlayerGravityController _gravityController;
        public PlayerGravityController GravityController => _gravityController;
        
        [SerializeField] private PlayerSlopeController _slopeController;
        public PlayerSlopeController SlopeController => _slopeController;

        [SerializeField] private PlayerGroundCheck _groundCheck;
        public PlayerGroundCheck GroundCheck => _groundCheck;

        [SerializeField] private PlayerAnimatorController _animatorController;
        public PlayerAnimatorController AnimatorController => _animatorController;
        
        [SerializeField] private PlayerColliderController _colliderController;
        public PlayerColliderController ColliderController => _colliderController;
    }
}