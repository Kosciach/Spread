using System;
using DG.Tweening;
using SaintsField;
using SaintsField.Playa;
using Spread.Player.Interactions;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace Spread.Player.Ladder
{
    using Player.StateMachine;
    using Spread.Interactions;
    using Spread.Ladder;

    public class  PlayerLadderController : PlayerControllerBase
    {
        private PlayerInteractionsController _interactionsController;
        
        [LayoutStart("Legs", ELayout.TitleBox)]
        [SerializeField] private Transform _leftLeg;
        [SerializeField] private Transform _rightLeg;
        
        [LayoutStart("Arms", ELayout.TitleBox)]
        [SerializeField] private Transform _leftArm;
        [SerializeField] private Transform _rightArm;
        
        [LayoutStart("Thumbs", ELayout.TitleBox)]
        [SerializeField] private Transform _leftThumb;
        [SerializeField] private Transform _rightThumb;
        
        [LayoutStart("SpineSway", ELayout.TitleBox)]
        [SerializeField] private MultiRotationConstraint _spineSway;

        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private bool _lockUpdateIK;
        [LayoutStart("Settings/Legs", ELayout.TitleBox)]
        [SerializeField] private bool _useCustomLegsDuration;
        [SerializeField, ShowIf(nameof(_useCustomLegsDuration))] private float _customLegsDuration;
        [SerializeField] private float _legArcHeight;
        [SerializeField] private Vector3 _leftLegOffset;
        [SerializeField] private Vector3 _rightLegOffset;
        [LayoutStart("Settings/Arms", ELayout.TitleBox)]
        [SerializeField] private bool _useCustomArmsDuration;
        [SerializeField, ShowIf(nameof(_useCustomArmsDuration))] private float _customArmsDuration;
        [SerializeField] private float _armArcHeight;
        [SerializeField] private float _armsHeightOffset;
        [SerializeField] private Vector3 _rightArmOffset;
        [LayoutStart("Settings/Thumbs", ELayout.TitleBox)]
        [SerializeField] private Vector3 _leftThumbOffset;
        [SerializeField] private Vector3 _rightThumbOffset;
        [LayoutStart("Settings/SpineSway", ELayout.TitleBox)]
        [SerializeField] private float _maxSway;

        [LayoutStart("Extentions", ELayout.TitleBox)]
        [SaintsRow(inline: true)]
        [SerializeField] private PlayerLadderController_Slide _slide;

        private Vector3 _leftLegPos;
        private Vector3 _rightLegPos;
        
        private Vector3 _leftArmPos;
        private Vector3 _rightArmPos;
        
        private Ladder _currentLadder;
        internal Ladder CurrentLadder => _currentLadder;

        private Tween _swayTween;

        internal bool IsMoving;
        
        
        protected override void OnSetup()
        {
            _interactionsController = _ctx.GetController<PlayerInteractionsController>();
            _interactionsController.OnInteract += Interaction;
        }

        protected override void OnDispose()
        {
            _interactionsController.OnInteract -= Interaction;
        }
        
        private void Interaction(Interactable p_interactable)
        {
            if (p_interactable is Ladder ladder)
            {
                _currentLadder = ladder;
            }
        }

        internal void Clear()
        {
            _currentLadder = null;
        }

        internal void SetSlideIk() => _slide.SetupIK(_currentLadder, _ctx.Transform.position);
        
        internal void SetStartIkPos(int p_rungIndex)
        {
            //Legs
            _leftLegPos = _currentLadder.Rungs[p_rungIndex];
            _rightLegPos = _currentLadder.Rungs[p_rungIndex + 1];

            if (p_rungIndex % 2 == 0)
                (_leftLegPos, _rightLegPos) = (_rightLegPos, _leftLegPos);

            _leftLegPos += _currentLadder.transform.TransformDirection(_leftLegOffset);
            _rightLegPos += _currentLadder.transform.TransformDirection(_rightLegOffset);
            
            //Arms
            _leftArmPos = _currentLadder.Rungs[p_rungIndex + 1];
            _rightArmPos = _currentLadder.Rungs[p_rungIndex];

            if (p_rungIndex % 2 == 0)
                (_leftArmPos, _rightArmPos) = (_rightArmPos, _leftArmPos);
                
            Vector3 rightArmOffset = new Vector3(_currentLadder.Size.x / 2 + _currentLadder.Size.z / 2, _armsHeightOffset, 0) + _rightArmOffset;
            Vector3 leftArmOffset = rightArmOffset;
            leftArmOffset.x *= -1;
            _leftArmPos += _currentLadder.transform.TransformDirection(leftArmOffset);
            _rightArmPos += _currentLadder.transform.TransformDirection(rightArmOffset);
            
            //Update IK
            UpdateIk();
        }
        
        internal void SetLegIkPos(int p_rungIndex, float p_climbDuration, int p_climbDirection)
        {
            //Set initial values
            Vector3 offset = _rightLegOffset;
            Vector3 currentPos = _rightLegPos;
            
            //Update initial values for left leg
            bool leftLeg = p_climbDirection == -1
                ? p_rungIndex % 2 != 0
                : p_rungIndex % 2 == 0;
            
            if (leftLeg)
            {
                offset = _leftLegOffset;
                currentPos = _leftLegPos;
            }
            
            //Calculate target
            Vector3 target = p_climbDirection == -1
                ? _currentLadder.Rungs[p_rungIndex]
                : _currentLadder.Rungs[p_rungIndex + 1];
            target += _currentLadder.transform.TransformDirection(offset);
            
            //Setup arc
            Vector3 arcDir = -_currentLadder.transform.forward;

            //Lerp pos
            float duration = _useCustomLegsDuration
                ? _customLegsDuration
                : p_climbDuration;
            DOTween.To(() => 0f, x =>
            {
                Vector3 newPos = Vector3.Lerp(currentPos, target, x);
                
                //Add arc
                float arcOffset = Mathf.Sin(x * Mathf.PI) * _legArcHeight;
                newPos += arcDir * arcOffset;

                //Apply to leg pos
                if (leftLeg)
                    _leftLegPos = newPos;
                else
                    _rightLegPos = newPos;

            }, 1f, duration).SetEase(Ease.InOutQuad);
        }
        
        internal void SetArmIkPos(int p_rungIndex, float p_climbDuration, int p_climbDirection)
        {
            Vector3 offset = new Vector3(_currentLadder.Size.x / 2 + _currentLadder.Size.z / 2, _armsHeightOffset, 0) + _rightArmOffset;
            Vector3 arcDir = Vector3.Lerp(-_ctx.Transform.forward, _ctx.Transform.right, 0.5f);
            Vector3 currentPos = _rightArmPos;
            
            bool leftArm = p_climbDirection != -1
                ? p_rungIndex % 2 != 0
                : p_rungIndex % 2 == 0;
            
            if (leftArm)
            {
                offset.x *= -1;
                arcDir = Vector3.Lerp(-_ctx.Transform.forward, -_ctx.Transform.right, 0.5f);
                currentPos = _leftArmPos;
            }
            
            Vector3 target = p_climbDirection == -1
                ? _currentLadder.Rungs[p_rungIndex]
                : _currentLadder.Rungs[p_rungIndex + 1];
            target += _currentLadder.transform.TransformDirection(offset);
            
            //Lerp pos
            float duration = _useCustomArmsDuration
                ? _customArmsDuration
                : p_climbDuration;
            DOTween.To(() => 0f, x =>
            {
                Vector3 newPos = Vector3.Lerp(currentPos, target, x);
                
                //Add arc
                float arcOffset = Mathf.Sin(x * Mathf.PI) * _armArcHeight;
                newPos += arcDir * arcOffset;

                //Apply to leg pos
                if (leftArm)
                    _leftArmPos = newPos;
                else
                    _rightArmPos = newPos;

            }, 1f, duration).SetEase(Ease.InOutQuad);
        }

        internal void SpineSway(int p_rungIndex, float p_climbDuration, int p_climbDirection)
        {
            float swayTarget = p_rungIndex % 2 == 0
                ? -_maxSway
                : _maxSway;

            if (p_climbDirection == -1)
                swayTarget *= -1;

            _swayTween?.Kill();
            
            _swayTween = DOTween.To(() => _spineSway.data.offset.z, x =>
            {
                if (_swayTween.ElapsedPercentage() >= 0.5f && !IsMoving)
                {
                    _swayTween.Kill();
                    
                    _swayTween = DOTween.To(() => _spineSway.data.offset.z, y =>
                    {
                        _spineSway.data.offset = new Vector3(0, 0, y);

                    }, 0, p_climbDuration*2).SetEase(Ease.OutQuad);
                    
                    return;
                }

                _spineSway.data.offset = new Vector3(0, 0, x);

            }, swayTarget, p_climbDuration).SetEase(Ease.InOutQuad);
        }
        
        internal void UpdateIk()
        {
            if (_lockUpdateIK) return;
            
            _leftLeg.position = _leftLegPos;
            _rightLeg.position = _rightLegPos;
            
            _leftArm.position = _leftArmPos;
            _rightArm.position = _rightArmPos;
            
            _leftThumb.position = _leftArmPos + _currentLadder.transform.TransformDirection(_leftThumbOffset);
            _rightThumb.position = _rightArmPos + _currentLadder.transform.TransformDirection(_rightThumbOffset);
        }
    }
}
