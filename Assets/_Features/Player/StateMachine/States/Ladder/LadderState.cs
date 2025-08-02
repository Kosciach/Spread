using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField.Playa;
using Spread.Player.Input;

namespace Spread.Player.StateMachine
{
    using Ladder;
    using Animating;
    using Camera;
    using Movement;
    using Gravity;
    using Collisions;
    using Spread.Ladder;
    
    public class LadderState : PlayerBaseState
    {
        private PlayerInputController _inputController;
        private PlayerLadderController _ladderController;
        private PlayerAnimatorController _animatorController;
        private PlayerCameraController _cameraController;
        private PlayerMovementController _movementController;
        private PlayerGravityController _gravityController;
        private PlayerColliderController _colliderController;
        
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
        
        protected override void OnSetup()
        {
            _inputController = _ctx.GetController<PlayerInputController>();
            _ladderController = _ctx.GetController<PlayerLadderController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _cameraController = _ctx.GetController<PlayerCameraController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _colliderController = _ctx.GetController<PlayerColliderController>();
        }
        
        protected override void OnEnter()
        {
            _ladderController.IsMoving = false;
            _currentLadder = _ladderController.CurrentLadder;
            _climbDirection = 0;
            _normalExit = false;
            _interactionExit = false;
            _isRunInput = false;
            _wasSliding = false;
            
            _ladderController.SetSlideIk();
            
            _inputController.Inputs.Keyboard.Move.performed += MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled += MoveInput;
            _inputController.Inputs.Keyboard.Run.performed += RunInput;
            _inputController.Inputs.Keyboard.Run.canceled += RunInput;
            _inputController.Inputs.Interactions.Use.performed += InteractInput;
            
            //Check climb direction
            Vector2 moveInput = _inputController.Inputs.Keyboard.Move.ReadValue<Vector2>();
            _climbDirection = (int)moveInput.y;
            _ladderController.IsMoving = _climbDirection != 0;
        }

        protected override void OnUpdate()
        {
            //Prevent player from moving weirdly after exiting ladder
            _animatorController.SetMovement(0, 0);
            
            //Sliding Down
            if (_isRunInput && _climbDirection == -1)
            {
                if (!_wasSliding)
                {
                    _wasSliding = true;
                    _animatorController.SetLadderSlideRig(1, 0.1f);
                    _ladderController.SetSlideIk();
                }

                Vector3 nextPosition = _ctx.Transform.position + Vector3.up * -_slidingDownSpeed * Time.deltaTime;

                Vector3 minPos = _currentLadder.Rungs[0];
                Vector3 maxPos = _currentLadder.Rungs[_currentLadder.AttachPoints.Count - 1 - _maxRungIndexOffset];
                nextPosition.y = Mathf.Clamp(nextPosition.y, minPos.y - 0.1f, maxPos.y);
                _ctx.Transform.position = nextPosition;
                
                CurrentRangIndex = _ladderController.CurrentLadder.GetClosestRungIndex(_ctx.Transform.position, _maxRungIndexOffset);
                _normalExit = CurrentRangIndex == 0;
                return;
            }
            
            if (_wasSliding)
            {
                _wasSliding = false;
                _animatorController.SetLadderSlideRig(0, 0.1f);
                
                //Set IK
                _ladderController.SetStartIkPos(CurrentRangIndex);
            }
            
            //Update IK
            _ladderController.UpdateIk();

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
            _ladderController.SetLegIkPos(CurrentRangIndex, _climbDuration, _climbDirection);
            _ladderController.SetArmIkPos(CurrentRangIndex, _climbDuration, _climbDirection);
            _ladderController.SpineSway(CurrentRangIndex, _climbDuration, _climbDirection);
        }

        protected override void OnExit()
        {
            _inputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled -= MoveInput;
            _inputController.Inputs.Keyboard.Run.performed -= RunInput;
            _inputController.Inputs.Keyboard.Run.canceled -= RunInput;
            _inputController.Inputs.Interactions.Use.performed -= InteractInput;

            _animatorController.SetLadderSlideRig(0, 0.1f);
            
            if (_interactionExit)
            {
                QuickLadderExit(0.5f);
                _gravityController.AddGravity(-_interactionExitGravity);
            }
        } 

        internal override Type GetNextState()
        {
            if (_gravityController.IsJump)
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
            _ladderController.IsMoving = _climbDirection != 0;
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
            _animatorController.LadderExit(false);
            _animatorController.SetLadderRig(0, p_disableLadderRigDuration);
            
            //Reset Camera MinMax
            _cameraController.ResetMinMax();
            _cameraController.ToggleWrap(true);

            //Root motion - on
            _animatorController.ToggleRootMotion(true);
            _movementController.RootMotionMove = true;

            //Gravity - on
            _gravityController.ToggleGravity(true);
            _gravityController.ToggleIkCrouch(true);
            _colliderController.ToggleCollision(true);
                
            //Feet Ik - on
            _animatorController.ToggleFootIk(true);
                
            //Reset ladder reference
            _ladderController.Clear();
        }
    }
}