using System;
using System.Linq;
using NaughtyAttributes;
using Nenn.InspectorEnhancements.Runtime.Attributes;
using UnityEngine;

namespace Spread.Player.StateMachine
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [BoxGroup("Context"), SerializeField, InlineProperty] private PlayerStateMachineContext _context;
        public PlayerStateMachineContext Ctx => _context;

        private void Awake()
        {
            //Prep Context
            foreach (var state in _context.States)
                state.Setup(_context);
            _context.Awake();

            //Init First State
            ChangeState(_context.States[0], true);
        }

        private void OnDestroy()
        {
            foreach (var state in _context.States)
                state.Dispose();
        }

        private void Update()
        {
            if (_context.CurrentState == null) return;

            CheckNextState();
            _context.CurrentState.UpdateState();
        }

        private void CheckNextState()
        {
            Type possibleNextState = _context.CurrentState.GetNextState();
            if (possibleNextState == null || possibleNextState == _context.CurrentState.GetType()) return;

            ChangeState(_context.States.FirstOrDefault(x => x.GetType() == possibleNextState), false);
        }

        private void ChangeState(PlayerBaseState p_newState, bool p_fromSetup)
        {
            if (p_newState == null) return;

            _context.LastState = _context.CurrentState;
            _context.LastState?.ExitState();
            _context.CurrentState = p_newState;
            _context.CurrentState.EnterState();

            if(!p_fromSetup)
                _context.OnStateTransition?.Invoke((_context.LastState?.GetType(), _context.CurrentState?.GetType()));
        }
    }
}