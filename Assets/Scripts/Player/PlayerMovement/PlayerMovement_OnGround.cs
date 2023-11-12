using UnityEngine;

namespace PlayerMovement
{
    [System.Serializable]
    public class PlayerMovement_OnGround
    {
        private PlayerMovementController _movementController;

        [Header("---Speed---")]
        [Range(0, 2)][SerializeField] int _animatorMovementSpeed;
        [Range(0, 10)][SerializeField] float _currentSpeed;
        [Range(0, 10)][SerializeField] float _walkSpeed;
        [Range(0, 10)][SerializeField] float _runSpeed;
        [Space(5)]
        [Range(0, 1)][SerializeField] float _smoothTime;


        [Space(20)]
        [Header("---Velocity---")]
        [SerializeField] Vector3 _targetVelocity;
        [SerializeField] Vector3 _currentVelocity;
        private Vector3 _currentVelocityRef;



        public void OnAwake(PlayerMovementController movementController)
        {
            _movementController = movementController;
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
            _movementController.Crouch.SynchronizeVelocity(_currentVelocity);
        }

        public void CalculateMovementSpeeds()
        {
            PlayerInputController playerInputController = _movementController.PlayerStateMachine.Input;
            _animatorMovementSpeed =
                !playerInputController.IsWalk ? 0 :
                !playerInputController.IsRun ? 1 :
                playerInputController.MovementInputVector.y > 0 ? 2 : 1;

            _currentSpeed =
                !playerInputController.IsWalk ? 0 :
                !playerInputController.IsRun ? _walkSpeed :
                playerInputController.MovementInputVector.y > 0 ? _runSpeed : _walkSpeed;

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
    }
}