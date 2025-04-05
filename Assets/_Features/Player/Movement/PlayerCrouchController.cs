using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace Spread.Player.Movement
{
    using StateMachine;

    public class PlayerCrouchController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [BoxGroup("Settings"), SerializeField] private float _crawlSpeed;

        [Foldout("Debug"), SerializeField, ReadOnly] private Vector3 _moveInput;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isCrouchInput; internal bool IsCrouchInput => _isCrouchInput;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isCrawlArea; internal bool IsCrawlArea => _isCrawlArea;
        [Foldout("Debug"), SerializeField, ReadOnly] private Vector3 _crawlVelocity;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            _isCrouchInput = false;

            _ctx.InputController.Inputs.Keyboard.Move.performed += MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled += MoveInput;
            _ctx.InputController.Inputs.Keyboard.Crouch.performed += CrouchInput;
        }

        private void OnDestroy()
        {
            _ctx.InputController.Inputs.Keyboard.Move.performed -= MoveInput;
            _ctx.InputController.Inputs.Keyboard.Move.canceled -= MoveInput;
            _ctx.InputController.Inputs.Keyboard.Crouch.performed -= CrouchInput;
        }

        internal void CheckCrouch()
        {
            _isCrouchInput = _ctx.MovementController.IsRunInput || !_ctx.GravityController.IsGrounded || _ctx.SlopeController.IsSlopeSlide ? false : _isCrouchInput;
            _ctx.AnimatorController.SetCrouchWeight(_ctx.CurrentState.IsCrouchState);
        }

        internal void CrawlMovement()
        {
            _ctx.MovementController.NormalMovement();

            Vector3 inputNormalized = _moveInput.normalized;
            Vector3 dir = (transform.forward * inputNormalized.z) + (transform.right * inputNormalized.x);
            dir.y = 0;
            dir *= _crawlSpeed;
            _crawlVelocity = Vector3.Lerp(_crawlVelocity, dir, 20 * Time.deltaTime);

            _ctx.CharacterController.Move(_crawlVelocity * Time.deltaTime);
        }

        #region Input
        private void MoveInput(InputAction.CallbackContext p_ctx)
        {
            Vector2 input = p_ctx.ReadValue<Vector2>();
            _moveInput = new Vector3(input.x, 0, input.y);
        }

        private void CrouchInput(InputAction.CallbackContext p_ctx)
        {
            if (_isCrouchInput && _ctx.GravityController.IsCeiling) return;

            _isCrouchInput = !_isCrouchInput;
        }
        #endregion

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