using UnityEngine;

namespace PlayerMovement
{
    [System.Serializable]
    public class PlayerMovement_InAir
    {
        private PlayerMovementController _movementController;

        [Header("---Speed---")]
        [Range(0, 10)][SerializeField] float _speed;
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
            Vector3 direction = _movementController.transform.right * inputVector.x + _movementController.transform.forward * inputVector.y;

            _targetVelocity = direction * _speed * Time.deltaTime;

            _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _currentVelocityRef, _smoothTime);
            _movementController.PlayerStateMachine.CharacterController.Move(_currentVelocity);

            _movementController.OnGround.SynchronizeVelocity(_currentVelocity);
        }

        public void SynchronizeVelocity(Vector3 velocity)
        {
            _targetVelocity = velocity;
            _currentVelocity = velocity;
            _currentVelocityRef = velocity;
        }
    }
}