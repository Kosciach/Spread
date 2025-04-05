using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace Spread.Player.StateMachine
{
    using Input;
    using Camera;
    using Animating;
    using Movement;
    using Gravity;
    using Collisions;
    using Interactions;
    using Ladder;

    [System.Serializable]
    public class PlayerStateMachineContext
    {
        [HorizontalLine(color: EColor.Gray)]

        [SerializeField, ReadOnly] internal PlayerBaseState LastState;
        [SerializeField, ReadOnly] internal PlayerBaseState CurrentState;
        [SerializeField] internal List<PlayerBaseState> States;
        [Header("References"), HorizontalLine(color: EColor.Gray)]

        [SerializeField] internal Transform Transform;
        [SerializeField] internal CharacterController CharacterController;
        [Header("Controllers"), HorizontalLine(color: EColor.Gray)]

        [SerializeField] public PlayerInputController InputController;
        [SerializeField] public PlayerCameraController CameraController;
        [SerializeField] public PlayerAnimatorController AnimatorController;
        [SerializeField] public PlayerMovementController MovementController;
        [SerializeField] public PlayerCrouchController CrouchController;
        [SerializeField] public PlayerSlideController SlideController;
        [SerializeField] public PlayerGravityController GravityController;
        [SerializeField] public PlayerSlopeController SlopeController;
        [SerializeField] public PlayerColliderController ColliderController;
        [SerializeField] public PlayerInteractionsController InteractionsController;
        [SerializeField] public PlayerLadderController LadderController;
        [Header("Other"), HorizontalLine(color: EColor.Gray)]

        private Dictionary<Type, PlayerBaseState> _statesKey = new Dictionary<Type, PlayerBaseState>();
        private Tween _moveTween;
        private Tween _rotTweenYAxis;

        internal Action<(Type lastState, Type newState)> OnStateTransition;

        internal void Awake()
        {
            foreach (var state in States)
            {
                _statesKey.Add(state.GetType(), state);
            }

            InputController.Setup();
            CameraController.Setup(this);
            MovementController.Setup(this);
            CrouchController.Setup(this);
            SlideController.Setup(this);
            GravityController.Setup(this);
            SlopeController.Setup(this);
            ColliderController.Setup(this);
            InteractionsController.Setup(this);
            LadderController.Setup(this);
        }

        internal PlayerBaseState GetStateByType(Type p_type)
        {
            if (!_statesKey.ContainsKey(p_type))
                return null;
            return _statesKey[p_type];
        }

        internal void RotToYAxis(float p_rot, float p_duration, Action p_onComplete = null)
        {
            if (_rotTweenYAxis != null)
            {
                _rotTweenYAxis.Kill();
                _rotTweenYAxis.onComplete = null;
                _rotTweenYAxis = null;
            }

            Quaternion targetRotation = Quaternion.Euler(0f, p_rot, 0f);
            _rotTweenYAxis = Transform.DORotateQuaternion(targetRotation, p_duration);
            _rotTweenYAxis.onComplete += () =>
            {
                p_onComplete?.Invoke();

                _moveTween.onComplete = null;
                _moveTween = null;
            };
            _rotTweenYAxis.SetEase(Ease.Linear);
        }
    }
}