using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStateMachineSystem;

namespace PlayerMovement
{
    [System.Serializable]
    public class PlayerMovement_Crouch
    {
        private PlayerMovementController _movementController;

        [Header("---ObsticleCheck---")]
        [Range(0, 3), SerializeField] float _obstacleRayLength;

        [Header("---Speed---")]
        [Range(0, 1), SerializeField] int _animatorMovementSpeed;
        [Range(0, 10), SerializeField] float _currentSpeed;
        [Range(0, 10), SerializeField] float _walkSpeed;
        [Space(5)]
        [Range(0, 1), SerializeField] float _smoothTime;

        [Space(20)]
        [Header("---Velocity---")]
        [SerializeField] Vector3 _targetVelocity;
        [SerializeField] Vector3 _currentVelocity;
        private Vector3 _currentVelocityRef;

        [Space(20)]
        [Header("---Debugs---")]
        [SerializeField] bool _isCrouch; public bool IsCrouch { get => _isCrouch; }



        public void OnAwake(PlayerMovementController movementController)
        {
            _movementController = movementController;
        }
        public void OnEnable()
        {
            PlayerInputController.OnCrouch += SetIsCrouch;
        }
        public void OnDisable()
        {
            PlayerInputController.OnCrouch -= SetIsCrouch;
        }



        public void Movement()
        {
            Vector3 inputVector = _movementController.PlayerStateMachine.Input.MovementInputVector;

            CalculateMovementSpeeds();

            Vector3 direction = _movementController.transform.right * inputVector.x + _movementController.transform.forward * inputVector.y;
            _targetVelocity = direction * _currentSpeed * Time.deltaTime;

            _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _currentVelocityRef, _smoothTime);
            _movementController.PlayerStateMachine.CharacterController.Move(_currentVelocity);

            _movementController.InAir.SynchronizeVelocity(_currentVelocity);
            _movementController.OnGround.SynchronizeVelocity(_currentVelocity);
            _movementController.OnGround.CalculateMovementSpeeds();
        }

        public void CalculateMovementSpeeds()
        {
            PlayerInputController playerInputController = _movementController.PlayerStateMachine.Input;
            _animatorMovementSpeed = !playerInputController.IsWalk ? 0 : 1;
            _currentSpeed = !playerInputController.IsWalk ? 0 : _walkSpeed;

            SetAnimationSpeeds(playerInputController.MovementInputVector);
        }
        private void SetAnimationSpeeds(Vector3 inputVector)
        {
            _movementController.PlayerStateMachine.Animator.SetFloat("MovementSpeed", _animatorMovementSpeed, 0.25f, Time.deltaTime);
            _movementController.PlayerStateMachine.Animator.SetFloat("MovementX", inputVector.x, 0.25f, Time.deltaTime);
            _movementController.PlayerStateMachine.Animator.SetFloat("MovementY", inputVector.y, 0.25f, Time.deltaTime);
        }
        public void SynchronizeVelocity(Vector3 velocity)
        {
            _targetVelocity = velocity;
            _currentVelocity = velocity;
            _currentVelocityRef = velocity;
        }

        private void SetIsCrouch()
        {
            if (!_movementController.PlayerStateMachine.VerticalVel.GroundCheck.IsGrounded
            || _movementController.PlayerStateMachine.IsStateEmblem(StateEmblems.Run)) return;

            Debug.DrawRay(_movementController.transform.position, Vector3.up * _obstacleRayLength, Color.red, 100);
            if (_isCrouch && Physics.Raycast(_movementController.transform.position, Vector3.up, out RaycastHit hit, _obstacleRayLength, ~LayerMask.GetMask("Player")))
                return;

            _isCrouch = !_isCrouch;
        }
        public void DisableIsCrouch()
        {
            _isCrouch = false;
        }
    }
}