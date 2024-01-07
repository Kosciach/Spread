using Tools;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;
        private CharacterController _characterController;

        [Header("--Settings--")]
        [SerializeField, Range(0, 10)] private float _walkSpeed;
        [SerializeField, Range(0, 10)] private float _runSpeed;
        [SerializeField, Range(0, 10)] private float _crouchSpeed;
        [SerializeField, Range(0, 2)] private float _onGroundSmoothTime;
        [SerializeField, Range(0, 2)] private float _inAirSmoothTime;
        [SerializeField, Range(0, 10)] private float _jumpForce; public float JumpForce => _jumpForce;

        [Space(20)]
        [Header("--Debugs--")]
        [SerializeField, ReadOnly] private bool _isCrouch;
        public bool IsCrouch
        {
            get => _isCrouch;
            set
            {
                if (!_isCrouch && _playerContext.InputController.IsRun)
                    return;

                if(_isCrouch && Physics.Raycast(transform.position, Vector3.up, 1.65f, ~LayerMask.GetMask("Player")))
                    return;

                _isCrouch = value;
                _playerContext.AnimatorController.Crouch(_isCrouch);
            }
        }
        [SerializeField, ReadOnly] public bool IsJump;
        [SerializeField, ReadOnly] private float _currentSpeed;
        [SerializeField, ReadOnly] private float _currentSmoothTime;
        [SerializeField, ReadOnly] public float MaxInAirSpeed;
        [SerializeField, ReadOnly] private Vector3 _currentDirection;

        private Vector3 _currentDirectionRef;


        private void Awake()
        {
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
            _characterController = GetComponent<CharacterController>();
        }
        private void Start()
        {
            _currentDirection = Vector3.zero;
        }
        private void Update()
        {
            SetCurrentSpeed();
            SetCurrentSmoothTime();
        }


        public void NormalMove()
        {
            Vector2 input = _playerContext.InputController.MoveInputVector;
            Vector3 direction = (transform.forward * input.y + transform.right * input.x);

            _currentDirection = Vector3.SmoothDamp(_currentDirection, direction, ref _currentDirectionRef, _currentSmoothTime);
            _characterController.Move(_currentDirection * _currentSpeed * Time.deltaTime);

            _playerContext.AnimatorController.SetMovementXY(_playerContext.InputController.MoveInputVector);
        }

        private void SetCurrentSpeed()
        {
            _currentSpeed =
            !_playerContext.InputController.IsMoving ? 0 :
            !_playerContext.InputController.IsRun ? IsCrouch ? _crouchSpeed : _walkSpeed :
            _playerContext.InputController.MoveInputVector.y > 0 ? _runSpeed : _walkSpeed;

            if (!_playerContext.GroundCheck.IsGrounded)
            {
                _currentSpeed = Mathf.Clamp(_currentSpeed, 0, MaxInAirSpeed);
            }


            float animatorMovementSpeed =
            !_playerContext.InputController.IsMoving ? 0 :
            !_playerContext.InputController.IsRun ? 1 :
            _playerContext.InputController.MoveInputVector.y > 0 ? 2 : 1;

            _playerContext.AnimatorController.SetMovementSpeed(animatorMovementSpeed);
        }
        private void SetCurrentSmoothTime()
        {
            _currentSmoothTime = _playerContext.GroundCheck.IsGrounded ? _onGroundSmoothTime : _inAirSmoothTime;
        }

        public void SaveMaxInAirSpeed()
        {
            if (MaxInAirSpeed != 0) return;

            MaxInAirSpeed = _currentSpeed == 0 ? _walkSpeed : _currentSpeed;
        }

        private void Crouch()
        {
            IsCrouch = !IsCrouch;
        }
        private void Jump()
        {
            if (!IsJump && _playerContext.GroundCheck.IsGrounded) IsJump = true;
        }

        private void OnEnable()
        {
            _playerContext.InputController.OnCrouch += Crouch;
            _playerContext.InputController.OnJump += Jump;
        }
        private void OnDisable()
        {
            _playerContext.InputController.OnCrouch -= Crouch;
            _playerContext.InputController.OnJump -= Jump;
        }
    }
}