using SaintsField;
using UnityEngine;
using UnityEngine.InputSystem;
using SaintsField.Playa;
using Spread.Player.Camera;

namespace Spread.Player.Movement
{
    using StateMachine;
    using Input;

    public class PlayerSlideController : PlayerControllerBase
    {
        private PlayerInputController _inputController;
        private PlayerSlopeController _slopeController;
        
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private CharacterController _characterController;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _startSpeed;
        [SerializeField] private float _stoppingSpeed;
        [SerializeField] private float _inSlideDeceleration;
        [SerializeField] private float _outOfSlideDeceleration;
        [SerializeField] private float _slopeAngleModifierStrength;
        [LayoutStart("Settings/UpSlope", ELayout.TitleBox)]
        [SerializeField] private float _upSlopeDeceleration;
        [LayoutStart("Settings/DownSlope", ELayout.TitleBox)]
        [SerializeField] private float _downSlopeSpeed;
        [SerializeField] private float _downSlopeAcceleration;
        [LayoutStart("Settings/ObstacleCheck", ELayout.TitleBox)]
        [SerializeField] private float _obstacleCheckHeight;
        [SerializeField] private float _obstacleCheckRange;
        [SerializeField] private float _obstacleCheckSpacing;
        [LayoutStart("Settings/Alignment", ELayout.TitleBox)]
        [SerializeField] private float _rotToSlopeSpeed;

        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [SerializeField, ReadOnly] private bool _isSlide; internal bool IsSlide => _isSlide;
        [SerializeField, ReadOnly] private Vector3 _slideVelocity;
        [SerializeField, ReadOnly] private Vector3 _startDownSlopeVelocity;

        internal Vector3 SlideVelocity => _slideVelocity;


        protected override void OnSetup()
        {
            _inputController = _ctx.GetController<PlayerInputController>();
            _slopeController = _ctx.GetController<PlayerSlopeController>();
            
            _inputController.Inputs.Keyboard.Slide.performed += SlideInput;
        }

        protected override void OnDispose()
        {
            _inputController.Inputs.Keyboard.Slide.performed -= SlideInput;
        }
        
        // Sliding
        internal void StartSlide()
        {
            _slideVelocity = transform.forward * _startSpeed;
            _startDownSlopeVelocity = transform.forward * _downSlopeSpeed;
        }
        
        internal void Slide(bool isSlideState)
        {
            // If in not slide state, keep momentum
            if (!isSlideState)
            {
                _slideVelocity = Vector3.Lerp(_slideVelocity, Vector3.zero, _outOfSlideDeceleration * Time.deltaTime);
                _ctx.CharacterController.Move(_slideVelocity * Time.deltaTime);
                return;
            }
            
            if (CheckObstacle())
            {
                _isSlide = false;
                return;
            }
            
            // Check if on slope
            SlopeData slopeData = _slopeController.GetSlopeData();
            if (slopeData == null || slopeData.Angle > _slopeController.StartSlopeSlideAngle)
            {
                _isSlide = false;
                return;
            }

            // Rotate to slope
            Vector3 slopeForward = Vector3.ProjectOnPlane(transform.forward, slopeData.Normal).normalized;
            Quaternion slopeAlignRotation = Quaternion.LookRotation(slopeForward, slopeData.Normal);
            transform.rotation = Quaternion.Lerp(transform.rotation, slopeAlignRotation, Time.deltaTime * _rotToSlopeSpeed);
            
            // Prep slope values
            float slopeDot = Vector3.Dot(_slideVelocity.normalized, slopeData.Direction);
            Vector3 targetVelocity = Vector3.zero;
            float lerpTime = 0;

            // Check slope direction
            float angleModifier = slopeData.Angle / 20f * _slopeAngleModifierStrength;
            switch (slopeDot)
            {
                case > 0.1f:
                    targetVelocity = _startDownSlopeVelocity * angleModifier;
                    lerpTime = _downSlopeAcceleration * angleModifier;
                    break;
                case < -0.1f:
                    lerpTime = _upSlopeDeceleration * angleModifier;
                    break;
                default:
                    lerpTime = _inSlideDeceleration;
                    break;
            }
            
            // Update and apply velocity
            _slideVelocity = Vector3.Lerp(_slideVelocity, targetVelocity, lerpTime * Time.deltaTime);
            Vector3 slopedVelocity = _slopeController.GetSlopeVelocity(_slideVelocity);
            _ctx.CharacterController.Move(slopedVelocity * Time.deltaTime);
            
            // Check end
            if (_isSlide && _slideVelocity.magnitude <= _stoppingSpeed)
            {
                _isSlide = false;
            }
        }

        internal void ResetSlide()
        {
            _isSlide = false;
            _slideVelocity = Vector3.zero;
        }
        
        // Helpers
        private bool CheckObstacle()
        {
            float width = _characterController.radius * 2f;
            int rayCount = Mathf.Max(1, Mathf.RoundToInt(width / _obstacleCheckSpacing) + 1);
            Vector3 mainOrigin = transform.position + Vector3.up * _obstacleCheckHeight;
            
            for (int i = 0; i < rayCount; i++)
            {
                Vector3 offsetFromCenter = _characterController.radius * Vector3.Lerp(-transform.right, transform.right, i / (rayCount - 1f));
                Vector3 rayOrigin = mainOrigin + offsetFromCenter;

                if (ShootObstacleRay(rayOrigin))
                    return true;
            }

            return false;
        }

        private bool ShootObstacleRay(Vector3 p_origin)
        {
            Vector3 direction = transform.forward;
            
            Debug.DrawRay(p_origin, direction * _obstacleCheckRange, Color.red);
            if (!Physics.Raycast(p_origin, direction, out RaycastHit hit, _obstacleCheckRange, ~LayerMask.GetMask("Player")))
                return false;
            
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > _ctx.CharacterController.slopeLimit;
        }
        
        // Inputs
        private void SlideInput(InputAction.CallbackContext p_ctx)
        {
            if (_ctx.CurrentState.GetType() == typeof(RunState))
                _isSlide = true;
        }
    }
}