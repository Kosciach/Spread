using UnityEngine;

namespace PlayerMovement
{
    [System.Serializable]
    public class PlayerMovement_OnGround
    {
        private PlayerMovementController _movementController;

        [Header("---OtherSettings---")]
        [SerializeField] bool _useRootMotionMovement; public bool UseRootMotionMovement { get { return _useRootMotionMovement; } }


        [Space(20)]
        [Header("---Speed---")]
        [Range(0, 10)][SerializeField] float _currentSpeed;
        [Range(0, 10)][SerializeField] float _walkSpeed;
        [Range(0, 10)][SerializeField] float _runSpeed;
        [Space(5)]
        [Range(0, 1)][SerializeField] float _smoothTime;


        [Space(20)]
        [Header("---Velocity---")]
        [SerializeField] Vector3 _targetVelocity;
        [SerializeField] Vector3 _currentVelocity;
        [SerializeField] Vector3 _rootMotionVelocity;
        private Vector3 _currentVelocityRef;



        public void OnAwake(PlayerMovementController movementController)
        {
            _movementController = movementController;
        }


        public void Movement()
        {
            Vector3 inputVector = _movementController.PlayerStateMachine.Input.MovementInputVector;

            SetAnimationSpeeds(inputVector);
            if (_useRootMotionMovement)
            {
                CalculateRootMotionVelocity();
                SynchronizeVelocity(Vector3.zero);
                _movementController.InAir.SynchronizeVelocity(_rootMotionVelocity);
                return;
            }


            Vector3 direction = _movementController.transform.right * inputVector.x + _movementController.transform.forward * inputVector.y;
            _targetVelocity = direction * _currentSpeed * Time.deltaTime;

            _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _currentVelocityRef, _smoothTime);
            _movementController.PlayerStateMachine.CharacterController.Move(_currentVelocity);

            SetAnimationSpeeds(inputVector);

            _movementController.InAir.SynchronizeVelocity(_currentVelocity);
        }
        private void CalculateRootMotionVelocity()
        {
            _rootMotionVelocity = (_movementController.PlayerStateMachine.Animator.deltaPosition / Time.deltaTime / 100) * 0.8f;
        }
        private void SetAnimationSpeeds(Vector3 inputVector)
        {
            _movementController.PlayerStateMachine.Animator.SetFloat("MovementSpeed", _currentSpeed, 0.2f, Time.deltaTime);
            _movementController.PlayerStateMachine.Animator.SetFloat("MovementX", inputVector.x, 0.15f, Time.deltaTime);
            _movementController.PlayerStateMachine.Animator.SetFloat("MovementY", inputVector.y, 0.15f, Time.deltaTime);
        }


        public void SynchronizeVelocity(Vector3 velocity)
        {
            _targetVelocity = velocity;
            _currentVelocity = velocity;
            _currentVelocityRef = velocity;
        }




        public void SetIdleSpeed()
        {
            _currentSpeed = 0;
            _movementController.InAir.SetSpeed(_walkSpeed);
        }
        public void SetWalkSpeed()
        {
            _currentSpeed = _walkSpeed;
            _movementController.InAir.SetSpeed(_walkSpeed);
        }
        public void SetRunSpeed()
        {
            _currentSpeed = _runSpeed;
            _movementController.InAir.SetSpeed(_runSpeed);
        }
    }
}