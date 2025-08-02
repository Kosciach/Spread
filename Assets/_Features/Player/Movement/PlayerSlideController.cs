using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField.Playa;
using Spread.Player.Input;

namespace Spread.Player.Movement
{
    using StateMachine;

    public class PlayerSlideController : PlayerControllerBase
    {
        private PlayerInputController _inputController;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _startSpeed;
        [SerializeField] private float _inSlideDrag;
        [SerializeField] private float _outOfSlideDrag;
        [SerializeField] private float _stoppingSpeed;

        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [SerializeField] private bool _isSlide; internal bool IsSlide => _isSlide;
        [SerializeField] private Vector3 _slideVelocity;


        protected override void OnSetup()
        {
            _inputController = _ctx.GetController<PlayerInputController>();
            _inputController.Inputs.Keyboard.Slide.performed += SlideInput;
        }

        protected override void OnDispose()
        {
            _inputController.Inputs.Keyboard.Slide.performed -= SlideInput;
        }

        protected override void OnTick()
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