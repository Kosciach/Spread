using Tools;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerGravityController : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;
        private CharacterController _characterController;

        [Header("--Settings--")]
        [Range(-20, 0)][SerializeField] float _gravityForce;
        [Range(-1, 0)][SerializeField] float _groundedGravityForce;

        [Space(20)]
        [Header("--Toggles--")]
        [SerializeField] bool _calculateGravity;
        [SerializeField] bool _applyGravity;

        [Space(20)]
        [Header("--Debugs--")]
        [SerializeField, ReadOnly] private float _currentGravityForce; public float CurrentGravityForce => _currentGravityForce;


        private void Awake()
        {
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
            _characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            CalculateGravity();
            ApplyGravity();
        }


        private void CalculateGravity()
        {
            if(_playerContext.GroundCheck.IsGrounded && !_playerContext.MovementController.IsJump)
            {
                _currentGravityForce = _groundedGravityForce;
                _playerContext.AnimatorController.SetGravity(_currentGravityForce);
                return;
            }
            _currentGravityForce += _gravityForce * Time.deltaTime;
            _playerContext.AnimatorController.SetGravity(_currentGravityForce);
        }
        private void ApplyGravity()
        {
            _characterController.Move(new Vector3(0, _currentGravityForce - _playerContext.SlopeController.CurrentSlopeAngle, 0) * Time.deltaTime);
        }

        public void AddGravityForce(float force)
        {
            _currentGravityForce += force;
        }
    }
}