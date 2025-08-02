using System;
using System.Collections.Generic;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;
using DG.Tweening;

namespace Spread.Player.StateMachine
{
    [Serializable]
    public class PlayerStateMachineContext
    {
        [LayoutStart("Context", ELayout.TitleBox | ELayout.Vertical)]
        [LayoutStart("Context/States", ELayout.TitleBox)]
        [SerializeField, ReadOnly] internal PlayerBaseState LastState;
        [SerializeField, ReadOnly] internal PlayerBaseState CurrentState;
        [SerializeField, ReadOnly] internal List<PlayerBaseState> States;
        
        [LayoutStart("Context/References", ELayout.TitleBox)]
        [SerializeField] internal Transform Transform;
        [SerializeField] internal CharacterController CharacterController;

        private Dictionary<Type, PlayerControllerBase> _controllers = new Dictionary<Type, PlayerControllerBase>();
        private Dictionary<Type, PlayerBaseState> _statesKey = new Dictionary<Type, PlayerBaseState>();
        private Tween _moveTween;
        private Tween _rotTweenYAxis;

        internal Action<(Type lastState, Type newState)> OnStateTransition;

        internal void GetStatesAndControllers()
        {
            foreach (PlayerBaseState state in States)
            {
                _statesKey.Add(state.GetType(), state);
            }
            
            foreach (PlayerControllerBase controller in Transform.GetComponents<PlayerControllerBase>())
            {
                _controllers.Add(controller.GetType(), controller);
            }
        }
        
        internal void Setup()
        {
            foreach (PlayerControllerBase controller in _controllers.Values)
            {
                controller.Setup(this);
            }
        }

        internal void Update()
        {
            foreach (PlayerControllerBase controller in _controllers.Values)
            {
                controller.Tick();
            }
        }
        
        internal void Dispose()
        {
            foreach (PlayerControllerBase controller in _controllers.Values)
            {
                controller.Dispose();
            }
        }

        internal T GetState<T>() where T : PlayerBaseState
        {
            Type type = typeof(T);
            if (!_statesKey.ContainsKey(type))
                return null;
            return _statesKey[type] as T;
        }
        
        internal T GetController<T>() where T : PlayerControllerBase
        {
            Type type = typeof(T);
            if (!_controllers.ContainsKey(type))
                return null;
            return _controllers[type] as T;
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