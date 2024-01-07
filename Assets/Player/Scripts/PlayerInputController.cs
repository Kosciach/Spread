using Tools;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;
        private PlayerInputs _inputs;

        [Header("--IsMoving--")]
        [SerializeField, ReadOnly] private float _currentIsMovingTimer = 0;
        [SerializeField] private float _requiredIsMovingTimer;
        public bool IsMoving => _currentIsMovingTimer < _requiredIsMovingTimer;


        [Space(20)]
        [Header("--Keyboard--")]
        [SerializeField, ReadOnly] private Vector2 _moveInputVector;
        public Vector2 MoveInputVector => _moveInputVector;

        [SerializeField, ReadOnly] private bool _isRun;
        public bool IsRun => _isRun;

        [HideInInspector] public Action OnCrouch;
        [HideInInspector] public Action OnJump;


        [Space(20)]
        [Header("--Mouse--")]
        [SerializeField, ReadOnly] private Vector2 _mouseInputVector;
        public Vector2 MouseInputVector => _mouseInputVector;



        private void Awake()
        {
            _inputs = new PlayerInputs();
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
        }
        private void Update()
        {
            KeyboardInput();
            MouseInput();
        }

        public void Init(PlayerStateMachineContext p_playerContext)
        {
            _playerContext = p_playerContext;
        }


        private void KeyboardInput()
        {
            _moveInputVector = _inputs.Keyboard.Move.ReadValue<Vector2>();
            IsMovingTimer();

            _isRun = _inputs.Keyboard.Run.ReadValue<float>() > 0.5f;

            _inputs.Keyboard.Jump.performed += ctx => OnJump?.Invoke();
            _inputs.Keyboard.Crouch.performed += ctx => OnCrouch?.Invoke();
        }
        private void MouseInput()
        {
            _mouseInputVector = _inputs.Mouse.Look.ReadValue<Vector2>();
        }

        private void IsMovingTimer()
        {
            if (_moveInputVector.magnitude > 0)
            {
                _currentIsMovingTimer = 0;
                return;
            }

            _currentIsMovingTimer += Time.deltaTime;
            _currentIsMovingTimer = Mathf.Clamp(_currentIsMovingTimer, 0, _requiredIsMovingTimer);
        }

        private void OnEnable() => _inputs.Enable();
        private void OnDisable() => _inputs.Disable();
    }
}