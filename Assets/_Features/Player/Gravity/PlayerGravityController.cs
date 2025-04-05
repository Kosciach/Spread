using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spread.Player.Gravity
{
    using StateMachine;

    public class PlayerGravityController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [BoxGroup("References"), SerializeField] private Transform _camera;

        [BoxGroup("Settings"), SerializeField] private float _gravityForce;
        [Header("Grounded")]
        [BoxGroup("Settings"), SerializeField] private Vector3 _groundCheckOffset;
        [BoxGroup("Settings"), SerializeField] private float _groundCheckRadius;
        [BoxGroup("Settings"), SerializeField] private float _groundedGravityForce;
        [BoxGroup("Settings"), SerializeField] private LayerMask _ignoreMask;
        [Header("Ceiling")]
        [BoxGroup("Settings"), SerializeField] private Vector3 _ceilingCheckOffset;
        [BoxGroup("Settings"), SerializeField] private float _ceilingCheckRange;
        [BoxGroup("Settings"), SerializeField] private float _ceilingCheckRadius;
        [Header("JumpState")]
        [BoxGroup("Settings"), SerializeField] private float _jumpForce;
        [Header("FallState")]
        [BoxGroup("Settings"), SerializeField] private float _gravityForFall;

        [Foldout("Debug"), SerializeField, ReadOnly] private bool _gizmos;
        [Foldout("Debug"), SerializeField, ReadOnly] private float _currentGravityForce; internal float CurrentGravityForce => _currentGravityForce;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isGrounded; internal bool IsGrounded => _isGrounded;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isCeiling; internal bool IsCeiling => _isCeiling;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _useGravity;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _useIkCrouch;
        [Header("Actions")]
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isJump; internal bool IsJump => _isJump;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isFalling; internal bool IsFalling => _isFalling;

        private Vector3 _ceilingSpherePos;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            ToggleGravity(true);
            ToggleIkCrouch(true);

            _ctx.InputController.Inputs.Keyboard.Jump.performed += SetIsJump;
        }

        private void Update()
        {
            CheckIsGrounded();
            Gravity();
            CheckIsCeiling();

            _isFalling = !_isGrounded && _currentGravityForce < -_gravityForFall;
            _isJump = !_isJump ? false : !(_isFalling || (_isGrounded && _currentGravityForce <= _groundedGravityForce));
        }

        private void OnDestroy()
        {
            _ctx.InputController.Inputs.Keyboard.Jump.performed -= SetIsJump;
        }

        private void SetIsJump(InputAction.CallbackContext p_ctx)
        {
            if ((_isGrounded && !_isCeiling) || _ctx.CurrentState is LadderState)
                _isJump = true;
        }

        private void CheckIsGrounded()
        {
            Vector3 offset = transform.forward * _groundCheckOffset.z + Vector3.up * _groundCheckOffset.y;
            bool isGrounded = Physics.CheckSphere(transform.position + offset, _groundCheckRadius, ~_ignoreMask);

            if (_useIkCrouch && isGrounded && !_isGrounded)
            {
                _ctx.AnimatorController.SetIkCrouch(_currentGravityForce / 3);
            }

            _isGrounded = isGrounded;
        }

        private void Gravity()
        {
            if (!_useGravity)
            {
                _ctx.AnimatorController.SetGravityForce(0);
                return;
            }

            _currentGravityForce = _isGrounded && !_isJump ? -_groundedGravityForce : _currentGravityForce - _gravityForce * Time.deltaTime;
            _ctx.CharacterController.Move(new Vector3(0, _currentGravityForce, 0) * Time.deltaTime);
            _ctx.AnimatorController.SetGravityForce(-_currentGravityForce);
        }

        private void CheckIsCeiling()
        {
            _ceilingSpherePos = transform.position + transform.rotation * _ctx.CharacterController.center + new Vector3(0, _ctx.CharacterController.height / 2, 0);

            if (Physics.Raycast(transform.position + _ceilingCheckOffset, Vector3.up, out RaycastHit hit, _ceilingCheckRange, ~_ignoreMask))
            {
                _ceilingSpherePos = hit.point;
            }

            Vector3 offset = transform.forward * _ceilingCheckOffset.z + Vector3.up * _ceilingCheckOffset.y;
            bool isCeiling = Physics.CheckSphere(_ceilingSpherePos + offset, _ceilingCheckRadius, ~_ignoreMask);

            if (isCeiling && !_isCeiling)
            {
                _currentGravityForce = Mathf.Min(0, _currentGravityForce);
            }

            _isCeiling = isCeiling;
        }

        internal void ToggleGravity(bool p_enable)
        {
            _useGravity = p_enable;
        }

        internal void ToggleIkCrouch(bool p_enable)
        {
            _useIkCrouch = p_enable;
        }

        internal void AddJumpForce()
        {
            _currentGravityForce = Mathf.Sqrt(_jumpForce * 2f * _gravityForce);
        }

        internal void AddGravity(float p_gravity)
        {
            _currentGravityForce += p_gravity;
        }

        private void OnDrawGizmos()
        {
            if(!_gizmos) return;

            Vector3 offset = transform.forward * _groundCheckOffset.z + Vector3.up * _groundCheckOffset.y;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + offset, _groundCheckRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + _ceilingCheckOffset, _ceilingSpherePos);
            Gizmos.DrawSphere(_ceilingSpherePos, _ceilingCheckRadius);
        }
    }
}