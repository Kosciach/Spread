using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spread.Player.StateMachine
{
    using Spread.Ladder;
    
    public class LadderState : PlayerBaseState
    {
        [SerializeField] private float _climbDuration;
        
        internal int CurrentRangIndex;
        private int _climbDirection;
        private Tween _climbTween;
        private Ladder _currentLadder;
        
        protected override void OnEnter()
        {
            _currentLadder = _ctx.LadderController.CurrentLadder;
            _climbDirection = 0;
            
            _ctx.InputController.Inputs.Keyboard.Move.performed += MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled += MoveInput;
        }

        protected override void OnUpdate()
        {
            //Check if climb anim is happening
            if (_climbTween != null) return;
            
            //Check next rang
            int nextRang = CurrentRangIndex + _climbDirection;
            nextRang = Mathf.Clamp(nextRang, 0, _currentLadder.AttachPoints.Count - 1);
            if (nextRang == CurrentRangIndex) return;

            //Move to next rang if possible
            CurrentRangIndex = nextRang;
            _climbTween = _ctx.Transform.DOMove(_currentLadder.AttachPoints[CurrentRangIndex], _climbDuration);
            _climbTween.SetEase(Ease.Linear);
            _climbTween.OnComplete(() => { _climbTween = null; });
        }

        protected override void OnExit()
        {
            _ctx.InputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled -= MoveInput;
        }

        internal override Type GetNextState()
        {
            if (_ctx.GravityController.IsJump)
            {
                return typeof(JumpState);
            }
            
            return GetType();
        }
        
        private void MoveInput(InputAction.CallbackContext p_ctx)
        {
            _climbDirection = (int)p_ctx.ReadValue<Vector2>().y;
        }
    }
}