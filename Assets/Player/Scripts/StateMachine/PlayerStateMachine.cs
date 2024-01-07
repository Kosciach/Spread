using Tools;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Player
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerBaseState _currentState;

        [Header("---StateMachine---")]
        [SerializeField, ReadOnly] private string _currentStateName;
        [SerializeField] private PlayerStateMachineContext _ctx = new PlayerStateMachineContext(); public PlayerStateMachineContext Ctx => _ctx;


        private void Awake()
        {
            _ctx.Transform = transform;

            _ctx.States = new Dictionary<Type, PlayerBaseState>
            {
                { typeof(PlayerState_Empty), new PlayerState_Empty(_ctx) },
                { typeof(PlayerState_Crouch), new PlayerState_Crouch(_ctx) },

                { typeof(PlayerState_Idle), new PlayerState_Idle(_ctx) },
                { typeof(PlayerState_Walk), new PlayerState_Walk(_ctx) },
                { typeof(PlayerState_Run), new PlayerState_Run(_ctx) },

                { typeof(PlayerState_Jump), new PlayerState_Jump(_ctx) },
                { typeof(PlayerState_Fall), new PlayerState_Fall(_ctx) },
                { typeof(PlayerState_Land), new PlayerState_Land(_ctx) },
                { typeof(PlayerState_HardLand), new PlayerState_HardLand(_ctx) },

                { typeof(PlayerState_SlopeSlide), new PlayerState_SlopeSlide(_ctx) }
            };

            foreach (var state in _ctx.States.Values)
                state.InitializeSubState();

            _currentState = _ctx.States[typeof(PlayerState_Idle)];
            UpdateStateName();
            _currentState.Enter();
        }

        private void Update()
        {
            Type nextState = _currentState.GetNextState();
            if(nextState == _currentState.GetType())
            {
                _currentState.UpdateStates();
                return;
            }

            ChangeState(nextState);
        }


        private void ChangeState(Type p_nextState)
        {
            _currentState.Exit();
            _currentState = _ctx.States[p_nextState];
            _currentState.Enter();

            UpdateStateName();
        }
        private void UpdateStateName()
        {
            _currentStateName = _currentState.ToString().Split(".").Last();
        }
    }
}