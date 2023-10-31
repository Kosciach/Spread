using UnityEngine;
using PlayerMovement;
using PlayerCamera;
using PlayerVerticalVel;

namespace PlayerStateMachineSystem
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerStateFactory _stateFactory;
        private PlayerBaseState _currentState; public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

        [Header("---StateMachine---")]
        [SerializeField] StateEmblems _stateEmblem; public StateEmblems StateEmblem { get { return _stateEmblem; } }


        [Space(20)]
        [Header("---Components---")]
        [SerializeField] CharacterController _characterController; public CharacterController CharacterController { get { return _characterController; } }
        [SerializeField] Animator _animator; public Animator Animator { get { return _animator; } }


        [Space(20)]
        [Header("---Scripts---")]
        [SerializeField] PlayerInputController _input; public PlayerInputController Input { get { return _input; } }
        [SerializeField] PlayerVelocityCalculator _velocity; public PlayerVelocityCalculator Velocity { get { return _velocity; } }
        [SerializeField] PlayerMovementController _movement; public PlayerMovementController Movement { get { return _movement; } }
        [SerializeField] PlayerCameraController _camera; public PlayerCameraController Camera { get { return _camera; } }
        [SerializeField] PlayerVerticalVelController _verticalVel; public PlayerVerticalVelController VerticalVel { get { return _verticalVel; } }



        private void Awake()
        {
            _stateFactory = new PlayerStateFactory(this);
            _currentState = _stateFactory.Idle();
            _currentState.Enter();
        }
        private void Update()
        {
            _currentState.Update();
            _currentState.CheckStateChange();
        }
        private void LateUpdate()
        {
            _currentState.LateUpdate();
        }
        private void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }



        public void SetStateEmblem(StateEmblems stateEmblem)
        {
            _stateEmblem = stateEmblem;
        }
        public bool IsStateEmblem(StateEmblems stateEmblem)
        {
            return _stateEmblem == stateEmblem;
        }
    }

    public enum StateEmblems
    {
        Idle, Walk, Run,
        Jump, Fall, Land, HardLanding
    }
}