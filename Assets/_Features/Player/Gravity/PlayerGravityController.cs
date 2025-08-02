using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField;
using SaintsField.Playa;
using Spread.Player.Animating;
using Spread.Player.Input;
using Spread.Player.Ladder;

namespace Spread.Player.Gravity
{
    using StateMachine;

    public class PlayerGravityController : PlayerControllerBase
    {
        private PlayerInputController _inputController;
        private PlayerLadderController _ladderController;
        private PlayerAnimatorController _animatorController;
        
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private Transform _camera;

        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _gravityForce;
        [LayoutStart("./Grounded", ELayout.TitleOut)]
        [SerializeField] private Vector3 _groundCheckOffset;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private float _groundedGravityForce;
        [SerializeField] private LayerMask _ignoreMask;
        [LayoutStart("./Ceiling", ELayout.TitleOut)]
        [SerializeField] private Vector3 _ceilingCheckOffset;
        [SerializeField] private float _ceilingCheckRange;
        [SerializeField] private float _ceilingCheckRadius;
        [LayoutStart("./JumpState", ELayout.TitleOut)]
        [SerializeField] private float _jumpForce;
        [LayoutStart("./FallStart", ELayout.TitleOut)]
        [SerializeField] private float _gravityForFall;

        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [SerializeField, ReadOnly] private bool _gizmos;
        [SerializeField, ReadOnly] private float _currentGravityForce; internal float CurrentGravityForce => _currentGravityForce;
        [SerializeField, ReadOnly] private bool _isGrounded; internal bool IsGrounded => _isGrounded;
        [SerializeField, ReadOnly] private bool _isCeiling; internal bool IsCeiling => _isCeiling;
        [SerializeField, ReadOnly] private bool _useGravity;
        [SerializeField, ReadOnly] private bool _useIkCrouch;
        [LayoutStart("./Actions", ELayout.TitleOut)]
        [SerializeField, ReadOnly] private bool _isJump; internal bool IsJump => _isJump;
        [SerializeField, ReadOnly] private bool _isFalling; internal bool IsFalling => _isFalling;

        private Vector3 _ceilingSpherePos;

        protected override void OnSetup()
        {
            ToggleGravity(true);
            ToggleIkCrouch(true);

            _inputController = _ctx.GetController<PlayerInputController>();
            _ladderController = _ctx.GetController<PlayerLadderController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            
            _inputController.Inputs.Keyboard.Jump.performed += SetIsJump;
        }
        
        protected override void OnTick()
        {
            CheckIsGrounded();
            Gravity();
            CheckIsCeiling();

            //Cleanup Later
            _isFalling = !_isGrounded && _currentGravityForce < -_gravityForFall;
            _isJump = !_isJump ? false : !(_isFalling || (_isGrounded && _currentGravityForce <= _groundedGravityForce));
            if (_ladderController.CurrentLadder != null)
            {
                _isJump = false;
            }
        }

        protected override void OnDispose()
        {
            _inputController.Inputs.Keyboard.Jump.performed -= SetIsJump;
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
                _animatorController.SetIkCrouch(_currentGravityForce / 3);
            }

            _isGrounded = isGrounded;
        }

        private void Gravity()
        {
            if (!_useGravity)
            {
                _animatorController.SetGravityForce(0);
                return;
            }

            _currentGravityForce = _isGrounded && !_isJump ? -_groundedGravityForce : _currentGravityForce - _gravityForce * Time.deltaTime;
            _ctx.CharacterController.Move(new Vector3(0, _currentGravityForce, 0) * Time.deltaTime);
            _animatorController.SetGravityForce(-_currentGravityForce);
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