using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField.Playa;

namespace Spread.Player.StateMachine
{
    using Spread.Ladder;
    
    public class LadderState : PlayerBaseState
    {
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private ExitLadderState _exitLadderState;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _climbDuration;
        [SerializeField] private int _maxRungIndexOffset;
        [LayoutStart("Settings/InteractionExit", ELayout.TitleBox)]
        [SerializeField] private float _interactionExitGravity;
        [LayoutStart("Settings/SlidingDown", ELayout.TitleBox)]
        [SerializeField] private float _slidingDownSpeed;
        
        internal int MaxRungIndexOffset => _maxRungIndexOffset;
        
        internal int CurrentRangIndex;
        private int _climbDirection;
        private bool _isRunInput;
        private bool _wasSliding;
        
        private Tween _climbTween;
        private Ladder _currentLadder;
        
        private bool _normalExit;
        private bool _interactionExit;
        
        protected override void OnEnter()
        {
            _ctx.LadderController.IsMoving = false;
            _currentLadder = _ctx.LadderController.CurrentLadder;
            _climbDirection = 0;
            _normalExit = false;
            _interactionExit = false;
            _isRunInput = false;
            _wasSliding = false;
            
            _ctx.LadderController.SetSlideIk();
            
            _ctx.InputController.Inputs.Keyboard.Move.performed += MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled += MoveInput;
            _ctx.InputController.Inputs.Keyboard.Run.performed += RunInput;
            _ctx.InputController.Inputs.Keyboard.Run.canceled += RunInput;
            _ctx.InputController.Inputs.Interactions.Use.performed += InteractInput;
            
            //Check climb direction
            Vector2 moveInput = _ctx.InputController.Inputs.Keyboard.Move.ReadValue<Vector2>();
            _climbDirection = (int)moveInput.y;
            _ctx.LadderController.IsMoving = _climbDirection != 0;
        }

        protected override void OnUpdate()
        {
            //Prevent player from moving weirdly after exiting ladder
            _ctx.AnimatorController.SetMovement(0, 0);
            
            //Sliding Down
            if (_isRunInput && _climbDirection == -1)
            {
                if (!_wasSliding)
                {
                    _wasSliding = true;
                    _ctx.AnimatorController.SetLadderSlideRig(1, 0.1f);
                    _ctx.LadderController.SetSlideIk();
                }

                Vector3 nextPosition = _ctx.Transform.position + Vector3.up * -_slidingDownSpeed * Time.deltaTime;

                Vector3 minPos = _currentLadder.Rungs[0];
                Vector3 maxPos = _currentLadder.Rungs[_currentLadder.AttachPoints.Count - 1 - _maxRungIndexOffset];
                nextPosition.y = Mathf.Clamp(nextPosition.y, minPos.y - 0.1f, maxPos.y);
                _ctx.Transform.position = nextPosition;
                
                CurrentRangIndex = _ctx.LadderController.CurrentLadder.GetClosestRungIndex(_ctx.Transform.position, _maxRungIndexOffset);
                _normalExit = CurrentRangIndex == 0;
                return;
            }
            
            if (_wasSliding)
            {
                _wasSliding = false;
                _ctx.AnimatorController.SetLadderSlideRig(0, 0.1f);
                
                //Set IK
                _ctx.LadderController.SetStartIkPos(CurrentRangIndex);
            }
            
            //Update IK
            _ctx.LadderController.UpdateIk();

            //Check if not moving
            if (_climbDirection == 0) return;
            
            //Check if climb anim is happening
            if (_climbTween != null) return;
            
            //Check next rang
            int nextRang = CurrentRangIndex + _climbDirection;
            nextRang = Mathf.Clamp(nextRang, 0, _currentLadder.AttachPoints.Count - 1 - _maxRungIndexOffset);
            _normalExit = nextRang == CurrentRangIndex;
            if (_normalExit) return;

            //Move to next rang if possible
            CurrentRangIndex = nextRang;
            _climbTween = _ctx.Transform.DOMove(_currentLadder.AttachPoints[CurrentRangIndex], _climbDuration);
            _climbTween.SetEase(Ease.Linear);
            _climbTween.OnComplete(() => { _climbTween = null; });
            
            //Set IK
            _ctx.LadderController.SetLegIkPos(CurrentRangIndex, _climbDuration, _climbDirection);
            _ctx.LadderController.SetArmIkPos(CurrentRangIndex, _climbDuration, _climbDirection);
            _ctx.LadderController.SpineSway(CurrentRangIndex, _climbDuration, _climbDirection);
        }

        protected override void OnExit()
        {
            _ctx.InputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled -= MoveInput;
            _ctx.InputController.Inputs.Keyboard.Run.performed -= RunInput;
            _ctx.InputController.Inputs.Keyboard.Run.canceled -= RunInput;
            _ctx.InputController.Inputs.Interactions.Use.performed -= InteractInput;

            _ctx.AnimatorController.SetLadderSlideRig(0, 0.1f);
            
            if (_interactionExit)
            {
                QuickLadderExit(0.5f);
                _ctx.GravityController.AddGravity(-_interactionExitGravity);
            }
        } 

        internal override Type GetNextState()
        {
            if (_ctx.GravityController.IsJump)
            {
                return typeof(JumpState);
            }

            if (_interactionExit)
            {
                return typeof(IdleState);
            }
            
            if (_normalExit && _climbTween == null)
            {
                _exitLadderState.ExitDirection = _climbDirection;
                return typeof(ExitLadderState);
            }
            
            return GetType();
        }
        
        private void MoveInput(InputAction.CallbackContext p_ctx)
        {
            _climbDirection = (int)p_ctx.ReadValue<Vector2>().y;
            _ctx.LadderController.IsMoving = _climbDirection != 0;
        }
        
        private void RunInput(InputAction.CallbackContext p_ctx)
        {
            _isRunInput = p_ctx.ReadValue<float>() > 0;
        }
        
        private void InteractInput(InputAction.CallbackContext p_ctx)
        {
            if (_normalExit)
                return;

            _interactionExit = true;
        }

        internal void QuickLadderExit(float p_disableLadderRigDuration)
        {
            //Disable ladder anims
            _ctx.AnimatorController.LadderExit(false);
            _ctx.AnimatorController.SetLadderRig(0, p_disableLadderRigDuration);
            
            //Reset Camera MinMax
            _ctx.CameraController.ResetMinMax();
            _ctx.CameraController.ToggleWrap(true);

            //Root motion - on
            _ctx.AnimatorController.ToggleRootMotion(true);
            _ctx.MovementController.RootMotionMove = true;

            //Gravity - on
            _ctx.GravityController.ToggleGravity(true);
            _ctx.GravityController.ToggleIkCrouch(true);
            _ctx.ColliderController.ToggleCollision(true);
                
            //Feet Ik - on
            _ctx.AnimatorController.ToggleFootIk(true);
                
            //Reset ladder reference
            _ctx.LadderController.Clear();
        }
    }
}