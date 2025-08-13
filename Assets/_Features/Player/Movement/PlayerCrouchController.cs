using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Player.Movement
{
    using StateMachine;
    using Input;
    using Animating;
    using Gravity;

    public class PlayerCrouchController : PlayerControllerBase
    {
        private PlayerInputController _inputController;
        private PlayerMovementController _movementController;
        private PlayerAnimatorController _animatorController;
        private PlayerGravityController _gravityController;
        private PlayerSlopeController _slopeController;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _crawlSpeed;

        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [SerializeField, ReadOnly] private Vector3 _moveInput;
        [SerializeField, ReadOnly] private bool _isCrouchInput; internal bool IsCrouchInput => _isCrouchInput;
        [SerializeField, ReadOnly] private bool _isCrawlArea; internal bool IsCrawlArea => _isCrawlArea;
        [SerializeField, ReadOnly] private Vector3 _crawlVelocity;

        protected override void OnSetup()
        {
            _isCrouchInput = false;
            
            _inputController = _ctx.GetController<PlayerInputController>();
            _movementController = _ctx.GetController<PlayerMovementController>();
            _animatorController = _ctx.GetController<PlayerAnimatorController>();
            _gravityController = _ctx.GetController<PlayerGravityController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
            
            _inputController.Inputs.Keyboard.Move.performed += MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled += MoveInput;
            _inputController.Inputs.Keyboard.Crouch.performed += CrouchInput;
        }
        
        protected override void OnDispose()
        {
            _inputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _inputController.Inputs.Keyboard.Move.canceled -= MoveInput;
            _inputController.Inputs.Keyboard.Crouch.performed -= CrouchInput;
        }

        internal void CheckCrouch()
        {
            bool canResetIsCrouchInput = _movementController.IsRunInput
                                         || !_gravityController.IsGrounded
                                         || _slopeController.IsSlopeSlide;
            if (canResetIsCrouchInput)
                _isCrouchInput = false;
            
            _animatorController.SetCrouchWeight(_ctx.CurrentState.IsCrouchState);
        }

        internal void CrawlMovement()
        {
            _movementController.NormalMovement();

            Vector3 inputNormalized = _moveInput.normalized;
            Vector3 dir = (transform.forward * inputNormalized.z) + (transform.right * inputNormalized.x);
            dir.y = 0;
            dir *= _crawlSpeed;
            _crawlVelocity = Vector3.Lerp(_crawlVelocity, dir, 20 * Time.deltaTime);

            _ctx.CharacterController.Move(_crawlVelocity * Time.deltaTime);
        }

        // Input
        private void MoveInput(InputAction.CallbackContext p_ctx)
        {
            Vector2 input = p_ctx.ReadValue<Vector2>();
            _moveInput = new Vector3(input.x, 0, input.y);
        }

        private void CrouchInput(InputAction.CallbackContext p_ctx)
        {
            if (_isCrouchInput && _gravityController.IsCeiling) return;

            _isCrouchInput = !_isCrouchInput;
        }

        // Collision
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("CrawlArea")) return;

            _isCrawlArea = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("CrawlArea")) return;

            _isCrawlArea = false;
        }
    }
}