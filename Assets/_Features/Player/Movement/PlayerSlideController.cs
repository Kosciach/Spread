using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace Spread.Player.Movement
{
    using StateMachine;

    public class PlayerSlideController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [BoxGroup("Settings"), SerializeField] private float _startSpeed;
        [BoxGroup("Settings"), SerializeField] private float _inSlideDrag;
        [BoxGroup("Settings"), SerializeField] private float _outOfSlideDrag;
        [BoxGroup("Settings"), SerializeField] private float _stoppingSpeed;

        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isSlide; internal bool IsSlide => _isSlide;
        [Foldout("Debug"), SerializeField, ReadOnly] private Vector3 _slideVelocity;


        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            _ctx.InputController.Inputs.Keyboard.Slide.performed += SlideInput;
        }

        private void OnDestroy()
        {
            _ctx.InputController.Inputs.Keyboard.Slide.performed -= SlideInput;
        }

        private void Update()
        {
            float drag = _isSlide ? _inSlideDrag : _outOfSlideDrag;
            _slideVelocity = Vector3.Lerp(_slideVelocity, Vector3.zero, drag * Time.deltaTime);

            _ctx.CharacterController.Move(_slideVelocity * Time.deltaTime);

            if (_isSlide && _slideVelocity.magnitude <= _stoppingSpeed)
            {
                _isSlide = false;
            }
        }

        internal void StartSlide()
        {
            _slideVelocity = transform.forward * _startSpeed;
        }

        internal void StopSlide()
        {
            _isSlide = false;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if(!hit.collider.CompareTag("CrawlArea") && !hit.collider.CompareTag("Ground") && !hit.collider.CompareTag("Player"))
            {
                _isSlide = false;
            }
        }

        #region Input
        private void SlideInput(InputAction.CallbackContext p_ctx)
        {
            if (_ctx.CurrentState.GetType() == typeof(RunState))
                _isSlide = true;
        }
        #endregion
    }
}