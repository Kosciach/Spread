using SaintsField;
using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField.Playa;
using Spread.Player.Animating;
using Spread.Player.Gravity;
using Spread.Player.Input;


namespace Spread.Player.Movement
{
    using StateMachine;

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
        [Header("InAir")]
        [SerializeField, Range(0, 10)] private float _inAirWalkSpeed;
        [SerializeField, Range(0, 10)] private float _inAirJogSpeed;
        [SerializeField, Range(0, 10)] private float _inAirRunSpeed;

        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [LayoutStart("./MovementTypes", ELayout.TitleOut)]
        [SerializeField, ReadOnly] private IdleTypes _idleType; internal IdleTypes IdleType => _idleType;
        [SerializeField, ReadOnly] private MovementTypes _movementType; internal MovementTypes MovementType => _movementType;
        [LayoutStart("./Velocity", ELayout.TitleOut)]
        [SerializeField, ReadOnly] private Vector3 _inAirVelocity;
        [LayoutStart("./Input", ELayout.TitleOut)]
        [SerializeField, ReadOnly] private Vector3 _moveInput; internal Vector3 MoveInputVector => _moveInput;
        [SerializeField, Range(0, 1), ReadOnly] private float _isMovingTimer;
        [SerializeField, ReadOnly] private bool _isJogInput; public bool IsJogInput => _isJogInput;
        [SerializeField, ReadOnly] private bool _isRunInput; public bool IsRunInput => _isRunInput;
        [SerializeField, ReadOnly] public bool RootMotionMove = true;


        protected override void OnSetup()
        {
            _inputController = _ctx.GetController<PlayerInputController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _crouchController = _ctx.GetController<PlayerCrouchController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
            
            _isJogInput = _jogOnStart;

            _inputController.Inputs.Keyboard.Move.performed += MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled += MoveInput;
            _inputController.Inputs.Keyboard.Jog.performed += JogInput;
            _inputController.Inputs.Keyboard.Run.performed += RunInput;
            _inputController.Inputs.Keyboard.Run.canceled += RunInput;

            _animatorController.AnimatorMove.OnAnimatorMoveEvent += AnimatorMove;
        }

        private void Update()
        {
            _isMovingTimer = _moveInput.magnitude > 0
                ? _isMovingTime
                : Mathf.Max(0, _isMovingTimer - Time.deltaTime);
        }

        private void OnDestroy()
        {
            _inputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled -= MoveInput;
            _inputController.Inputs.Keyboard.Jog.performed -= JogInput;
            _inputController.Inputs.Keyboard.Run.performed -= RunInput;
            _inputController.Inputs.Keyboard.Run.canceled -= RunInput;

            _animatorController.AnimatorMove.OnAnimatorMoveEvent -= AnimatorMove;
        }

        private void SetMovementTypes()
        {
            _crouchController.CheckCrouch();
            _idleType = _crouchController.IsCrouchInput ? IdleTypes.Crouch : IdleTypes.Normal;

            if (_isMovingTimer <= 0)
            {
                _movementType = MovementTypes.Idle;
                return;
            }

            if(_crouchController.IsCrouchInput)
            {
                _movementType = MovementTypes.Crouch;
                return;
            }

            if (!_isRunInput || (_isRunInput && _moveInput.z <= 0))
            {
                _movementType = _isJogInput ? MovementTypes.Jog : MovementTypes.Walk;
                return;
            }

            _movementType = MovementTypes.Run;
        }

        internal void NormalMovement()
        {
            SetMovementTypes();

            _animatorController.SetMovementType(_movementType);
            _animatorController.SetCrouchWeight(_ctx.CurrentState.IsCrouchState);

            if (_movementType is not MovementTypes.Idle)
            {
                Vector3 inputNormalized = _moveInput.normalized;
                _animatorController.SetMovement(inputNormalized.x, inputNormalized.z);
                _animatorController.SetMovementTypeF(_movementType);
            }
        }

        internal void RestrainNormalMovement()
        {
            _movementType = MovementTypes.Idle;
            _idleType = IdleTypes.Normal;
            
            _animatorController.SetMovementType(_movementType);
            _animatorController.SetTurn(0);
            _animatorController.SetCrouchWeight(false);
        }

        internal void InAirMovement()
        {
            NormalMovement();

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

        internal void PushInAir(Vector3 p_velocity)
        {
            p_velocity.y = 0;
            _inAirVelocity += p_velocity;
        }

        private void AnimatorMove()
        {
            if (!RootMotionMove) return;

            Vector3 velocity = _animator.deltaPosition;
            velocity.y = 0;

            if (_gravityController.IsFalling
                || _gravityController.IsJump
                || velocity.magnitude == 0) return;

            velocity = _slopeController.GetSlopeVelocity(velocity);

            _ctx.CharacterController.Move(velocity);

            if (_gravityController.IsGrounded)
            {
                float speed = _isMovingTimer <= 0
                    ? 0 : _isRunInput
                    ? _inAirRunSpeed : _isJogInput
                    ? _inAirJogSpeed : _inAirWalkSpeed;

                Vector3 inputNormalized = _moveInput.normalized;
                Vector3 dir = (transform.forward * inputNormalized.z) + (transform.right * inputNormalized.x);
                _inAirVelocity = dir * speed;
                _inAirVelocity.y = 0;
            }
        }

        #region Input
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
            if ((_crouchController.IsCrouchInput && _gravityController.IsCeiling) || _ctx.CurrentState.GetType() == typeof(CrawlState)) return;

            _isRunInput = p_ctx.ReadValue<float>() > 0.5f;
        }
        #endregion
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