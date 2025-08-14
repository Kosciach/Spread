using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Player.Movement
{
    using StateMachine;
    using Input;
    using Animating;
    using Gravity;

    public class PlayerMovementController : PlayerControllerBase
    {
        private PlayerInputController _inputController;
        private PlayerAnimatorController _animatorController;
        private PlayerCrouchController _crouchController;
        private PlayerGravityController _gravityController;
        private PlayerSlopeController _slopeController;
        
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private Animator _animator;

        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private bool _jogOnStart = true;
        [SerializeField, Range(0, 1)] private float _isMovingTime;
        [LayoutStart("Settings/InAir", ELayout.TitleBox)]
        [SerializeField, Range(0, 10)] private float _inAirWalkSpeed;
        [SerializeField, Range(0, 10)] private float _inAirJogSpeed;
        [SerializeField, Range(0, 10)] private float _inAirRunSpeed;

        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [LayoutStart("Debug/MovementTypes", ELayout.TitleBox)]
        [SerializeField, ReadOnly] private IdleTypes _idleType;
        [SerializeField, ReadOnly] private MovementTypes _movementType;
        [LayoutStart("Debug/Velocity", ELayout.TitleBox)]
        [SerializeField, ReadOnly] private Vector3 _inAirVelocity;
        [LayoutStart("Debug/Input", ELayout.TitleBox)]
        [SerializeField, ReadOnly] private Vector3 _moveInput;
        [SerializeField, Range(0, 1), ReadOnly] private float _isMovingTimer;
        [SerializeField, ReadOnly] private bool _isJogInput;
        [SerializeField, ReadOnly] private bool _isRunInput;

        private Type _nextMovementState;
        internal Type NextMovementState => _nextMovementState;
        
        internal IdleTypes IdleType => _idleType;
        internal MovementTypes MovementType => _movementType;
        
        internal Vector3 MoveInputVector => _moveInput;
        public bool IsJogInput => _isJogInput;
        public bool IsRunInput => _isRunInput;
        
        private readonly Dictionary<(IdleTypes idleTypes, MovementTypes movementType), Type> MovementStatesMap = new()
        {
            {(IdleTypes.Crouch, MovementTypes.Idle), typeof(CrouchIdleState)},
            {(IdleTypes.Crouch, MovementTypes.Crouch), typeof(CrouchWalkState)},
            {(IdleTypes.Normal, MovementTypes.Idle), typeof(IdleState)},
            {(IdleTypes.Normal, MovementTypes.Walk), typeof(WalkState)},
            {(IdleTypes.Normal, MovementTypes.Jog), typeof(JogState)},
            {(IdleTypes.Normal, MovementTypes.Run), typeof(RunState)}
        };

        protected override void OnSetup()
        {
            _isJogInput = _jogOnStart;
            
            _inputController = _ctx.GetController<PlayerInputController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _crouchController = _ctx.GetController<PlayerCrouchController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();

            _inputController.Inputs.Keyboard.Move.performed += MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled += MoveInput;
            _inputController.Inputs.Keyboard.Jog.performed += JogInput;
            _inputController.Inputs.Keyboard.Run.performed += RunInput;
            _inputController.Inputs.Keyboard.Run.canceled += RunInput;

            _animatorController.AnimatorMove.OnAnimatorMoveEvent += AnimatorMove;
        }

        protected override void OnTick()
        {
            _isMovingTimer = _moveInput.magnitude > 0
                ? _isMovingTime
                : Mathf.Max(0, _isMovingTimer - Time.deltaTime);
            
            // Normal movement
            SetMovementTypes();
            Vector3 inputNormalized = _moveInput.normalized;
            _animatorController.SetMovementType(_movementType, inputNormalized);
            _animatorController.SetCrouchWeight(_ctx.CurrentState.IsCrouchState);
        }

        protected override void OnDispose()
        {
            _inputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled -= MoveInput;
            _inputController.Inputs.Keyboard.Jog.performed -= JogInput;
            _inputController.Inputs.Keyboard.Run.performed -= RunInput;
            _inputController.Inputs.Keyboard.Run.canceled -= RunInput;

            _animatorController.AnimatorMove.OnAnimatorMoveEvent -= AnimatorMove;
        }

        // In Air
        internal void InAirMovement()
        {
            float speed = _isMovingTimer <= 0
                ? 0 : _isRunInput
                    ? _inAirRunSpeed : _isJogInput
                        ? _inAirJogSpeed : _inAirWalkSpeed;

            Vector3 inputNormalized = _moveInput.normalized;
            Vector3 dir = (transform.forward * inputNormalized.z) + (transform.right * inputNormalized.x);
            dir.y = 0;
            dir *= speed;
            _inAirVelocity = Vector3.Lerp(_inAirVelocity, dir, 3 * Time.deltaTime);

            _ctx.CharacterController.Move(_inAirVelocity * Time.deltaTime);
        }

        // Helpers
        private void SetMovementTypes()
        {
            _crouchController.CheckCrouch();
            _idleType = _crouchController.IsCrouchInput
                ? IdleTypes.Crouch
                : IdleTypes.Normal;

            _movementType = GetMovementType();
            _nextMovementState = MovementStatesMap[(_idleType, _movementType)];
        }

        private MovementTypes GetMovementType()
        {
            if (_isMovingTimer <= 0) return MovementTypes.Idle;
            if (_crouchController.IsCrouchInput) return MovementTypes.Crouch;
            if (_isRunInput && _moveInput.z > 0) return MovementTypes.Run;
            return _isJogInput ? MovementTypes.Jog : MovementTypes.Walk;
        }
        
        internal void PushInAir(Vector3 p_velocity)
        {
            p_velocity.y = 0;
            _inAirVelocity += p_velocity;
        }
        
        // Rootmotion
        private void AnimatorMove()
        {
            Vector3 velocity = _animator.deltaPosition;
            velocity.y = 0;

            if (_gravityController.IsFalling
                || _gravityController.IsJump
                || velocity.magnitude == 0) return;

            velocity = _slopeController.GetSlopeVelocity(velocity);

            _ctx.CharacterController.Move(velocity);
            
            // Prep inAir while grounded
            if (_gravityController.IsGrounded)
            {
                float speed = _isMovingTimer <= 0 ? 0
                            : _isRunInput ? _inAirRunSpeed 
                            : _isJogInput ? _inAirJogSpeed
                                          : _inAirWalkSpeed;

                Vector3 inputNormalized = _moveInput.normalized;
                Vector3 dir = (transform.forward * inputNormalized.z) + (transform.right * inputNormalized.x);
                _inAirVelocity = dir * speed;
                _inAirVelocity.y = 0;
            }
        }

        // Input
        private void MoveInput(InputAction.CallbackContext p_ctx)
        {
            Vector2 input = p_ctx.ReadValue<Vector2>();
            _moveInput = new Vector3(input.x, 0, input.y);
        }

        private void JogInput(InputAction.CallbackContext p_ctx)
        {
            _isJogInput = !_isJogInput;
        }

        private void RunInput(InputAction.CallbackContext p_ctx)
        {
            bool inPermaCrouch = _crouchController.IsCrouchInput && _gravityController.IsCeiling;
            bool isCrawling = _ctx.CurrentState.GetType() == typeof(CrawlState);
            if (inPermaCrouch || isCrawling)
                return;

            _isRunInput = p_ctx.ReadValue<float>() > 0.5f;
        }
    }

    internal enum IdleTypes
    {
        Normal, Crouch
    }

    internal enum MovementTypes
    {
        Idle, Crouch, Walk, Jog, Run
    }
}