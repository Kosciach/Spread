using Cinemachine.Utility;
using Tools;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerSlopeController : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;
        private CharacterController _characterController;

        [Header("--Debugs--")]
        [SerializeField, ReadOnly] private float _currentSlopeAngle; public float CurrentSlopeAngle => _currentSlopeAngle;
        [SerializeField, ReadOnly] private float _currentSpeed; public float CurrentSpeed => _currentSpeed;
        [SerializeField, ReadOnly] private RaycastHit _currentSlopeInfo; public RaycastHit CurrentSlopeInfo => _currentSlopeInfo;
        [SerializeField, ReadOnly] private Vector3 _currentVelocity; public Vector3 CurrentVelocity => _currentVelocity;

        public bool ShouldSlide => _currentSlopeAngle > _characterController.slopeLimit-0.01f;


        private void Awake()
        {
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
            _characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            GetSlopeAngle();
            _currentVelocity = ShouldSlide ? _currentVelocity : Vector3.zero;
        }


        private void GetSlopeAngle()
        {
            LayerMask groundMask = LayerMask.GetMask("Ground");
            if(!_playerContext.GroundCheck.IsGrounded || _playerContext.MovementController.IsJump)
            {
                _currentSlopeAngle = 0;
                return;
            }

            Physics.Raycast(transform.position, Vector3.down, out _currentSlopeInfo, 0.3f, groundMask);
            _currentSlopeAngle = Vector3.Angle(_currentSlopeInfo.normal, Vector3.up);

        }
        public Vector3 Slide()
        {
            _currentVelocity = Vector3.ProjectOnPlane(new Vector3(0, _playerContext.GravityController.CurrentGravityForce, 0), _currentSlopeInfo.normal);
            _characterController.Move(_currentVelocity * Time.deltaTime * _currentSlopeAngle/35 * 4);
            return _currentVelocity;
        }
        public void AfterSlide(Vector3 p_oldVelocity)
        {
            //_characterController.Move(p_oldVelocity * Time.deltaTime * 4);
        }
    }
}