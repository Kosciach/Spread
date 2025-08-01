using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using FischlWorks;
using SaintsField.Playa;

namespace Spread.Player.Animating
{
    using System;
    using Movement;
    using UnityEngine.Animations.Rigging;

    public class PlayerAnimatorController : MonoBehaviour
    {
        [LayoutStart("References", ELayout.TitleBox)]
        [SerializeField] private Animator _animator;
        [SerializeField] private csHomebrewIK _footIk;
        [SerializeField] private Rig _ladderRig;
        [SerializeField] private Rig _ladderSlideRig;
        [SerializeField] private PlayerAnimatorController_AnimatorMove _animatorMove;
        internal PlayerAnimatorController_AnimatorMove AnimatorMove => _animatorMove;

        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private float _movementTypeBlendTime = 0.3f;
        [SerializeField] private float _movementBlendTime = 0.5f;
        [SerializeField] private float _turnBlendTime = 0.05f;
        
        private Dictionary<int, string> _currentStateName; internal Dictionary<int, string> CurrentStateName => _currentStateName;
        private Dictionary<int, string> _previousStateName; internal Dictionary<int, string> PreviousStateName => _previousStateName;
        public Action<int, string> OnStateChange;

        private Tween _inAirLayerTween;
        private Tween _crawlLayerTween;
        private Tween _slideLayerTween;
        private Tween _ladderRigTween;
        private Tween _ladderSlideRigTween;
        private Tween _ikCrouchTween;

        internal bool TransitioningToCrawl => _crawlLayerTween != null;

        private void Awake()
        {
            _currentStateName = new Dictionary<int, string>();
            _previousStateName = new Dictionary<int, string>();

            for (int i = 0; i < _animator.layerCount; i++)
            {
                _currentStateName.Add(i, "---------");
                _previousStateName.Add(i, "---------");
            }
        }

        private void OnDestroy()
        {
            OnStateChange = null;
        }

        #region States
        internal void StateEnter(int p_layerIndex, string p_stateName)
        {
            _currentStateName[p_layerIndex] = p_stateName;
            OnStateChange?.Invoke(p_layerIndex, p_stateName);
        }

        internal void StateExit(int p_layerIndex, string p_stateName)
        {
            _previousStateName[p_layerIndex] = p_stateName;

            if (p_layerIndex == 0 && _currentStateName[p_layerIndex] == "Idle" && _previousStateName[p_layerIndex] == "Move")
            {
                _animator.SetInteger("MovementType", 0);
                _animator.SetFloat("MovementTypeF", 0);

                _animator.SetFloat("MovementX", 0);
                _animator.SetFloat("MovementZ", 0);
            }
        }
        #endregion

        #region Other
        internal void ToggleRootMotion(bool p_enable)
        {
            _animator.applyRootMotion = p_enable;
        }

        internal void ToggleFootIk(bool p_enable)
        {
            _footIk.globalWeight = p_enable ? 1 : 0;
            _footIk.enableIKPositioning = p_enable;
            _footIk.enableIKRotating = p_enable;
        }

        internal void SetIkCrouch(float p_value)
        {
            if (_ikCrouchTween != null)
            {
                _ikCrouchTween.Kill();
                _ikCrouchTween.onComplete = null;
                _ikCrouchTween = null;
            }

            _ikCrouchTween = _animator.transform.DOLocalMoveY(Mathf.Clamp(p_value, -0.33f, 0), 0.2f);
            _ikCrouchTween.onComplete += () =>
            {
                _ikCrouchTween = _animator.transform.DOLocalMoveY(0, 0.5f);
                _ikCrouchTween.onComplete += () =>
                {
                    _ikCrouchTween.onComplete = null;
                    _ikCrouchTween = null;
                };
            };
        }
        #endregion

        #region Idle
        internal void SetTurn(float p_turnDir)
        {
            _animator.SetFloat("TurnDir", p_turnDir, _turnBlendTime, Time.deltaTime);
        }

        internal void SetIdleType(IdleTypes p_idleType)
        {
            _animator.SetInteger("IdleType", (int)p_idleType);
        }

        internal void SetIdleTypeF(IdleTypes p_idleType)
        {
            _animator.SetFloat("IdleTypeF", (int)p_idleType, 0.2f, Time.deltaTime);
            _animator.SetLayerWeight(1, _animator.GetFloat("IdleTypeF"));
        }
        #endregion

        #region Movement
        internal void SetMovementType(MovementTypes p_movementType)
        {
            _animator.SetInteger("MovementType", (int)p_movementType);
        }

        internal void SetMovementTypeF(MovementTypes p_movementType)
        {
            _animator.SetFloat("MovementTypeF", (int)p_movementType, _movementTypeBlendTime, Time.deltaTime);
        }

        internal void SetMovement(float p_movementX, float p_movementZ)
        {
            _animator.SetFloat("MovementX", p_movementX, _movementBlendTime, Time.deltaTime);
            _animator.SetFloat("MovementZ", p_movementZ, _movementBlendTime, Time.deltaTime);
        }
        #endregion

        internal void SetGravityForce(float p_gravityForce)
        {
            _animator.SetFloat("GravityForce", p_gravityForce, 0.2f, Time.deltaTime);
        }

        #region Layers
        internal void SetInAirLayer(float p_weight)
        {
            if (_inAirLayerTween != null)
            {
                _inAirLayerTween.Kill();
                _inAirLayerTween.onUpdate = null;
                _inAirLayerTween = null;
            }

            float weight = _animator.GetLayerWeight(2);
            _inAirLayerTween = DOTween.To(() => weight, x => weight = x, p_weight, 0.5f);
            _inAirLayerTween.onUpdate += () =>
            {
                _animator.SetLayerWeight(2, weight);
            };
            _inAirLayerTween.onComplete += () =>
            {
                _inAirLayerTween.onUpdate = null;
                _inAirLayerTween.onComplete = null;
                _inAirLayerTween = null;
            };
        }

        internal void SetCrouchWeight(bool p_crouch)
        {
            float targetWeight = p_crouch ? 1.0f : 0.0f;
            _animator.SetFloat("CrouchWeight", targetWeight, 0.2f, Time.deltaTime);
            _animator.SetLayerWeight(1, _animator.GetFloat("CrouchWeight"));
        }

        internal void SetCrawlLayer(float p_weight)
        {
            if (_crawlLayerTween != null)
            {
                _crawlLayerTween.Kill();
                _crawlLayerTween.onUpdate = null;
                _crawlLayerTween = null;
            }

            float weight = _animator.GetLayerWeight(3);
            _crawlLayerTween = DOTween.To(() => weight, x => weight = x, p_weight, 0.5f);
            _crawlLayerTween.onUpdate += () =>
            {
                _animator.SetLayerWeight(3, weight);
            };
            _crawlLayerTween.onComplete += () =>
            {
                _crawlLayerTween.onUpdate = null;
                _crawlLayerTween.onComplete = null;
                _crawlLayerTween = null;
            };
        }

        internal void SetSlideLayer(float p_weight)
        {
            if (_slideLayerTween != null)
            {
                _slideLayerTween.Kill();
                _slideLayerTween.onUpdate = null;
                _slideLayerTween = null;
            }

            float weight = _animator.GetLayerWeight(4);
            _slideLayerTween = DOTween.To(() => weight, x => weight = x, p_weight, 0.5f);
            _slideLayerTween.onUpdate += () =>
            {
                _animator.SetLayerWeight(4, weight);
            };
            _slideLayerTween.onComplete += () =>
            {
                _slideLayerTween.onUpdate = null;
                _slideLayerTween.onComplete = null;
                _slideLayerTween = null;
            };
        }

        internal void SetLadderRig(float p_weight, float p_duration = 0.5f)
        {
            if (_ladderRigTween != null)
            {
                _ladderRigTween.Kill();
                _ladderRigTween.onComplete = null;
                _ladderRigTween = null;
            }

            _ladderRigTween = DOTween.To(() => _ladderRig.weight, x => _ladderRig.weight = x, p_weight, p_duration);
            _ladderRigTween.onComplete += () =>
            {
                _ladderRigTween.onComplete = null;
                _ladderRigTween = null;
            };
        }
        
        internal void SetLadderSlideRig(float p_weight, float p_duration = 0.5f)
        {
            if (_ladderSlideRigTween != null)
            {
                _ladderSlideRigTween.Kill();
                _ladderSlideRigTween.onComplete = null;
                _ladderSlideRigTween = null;
            }

            _ladderSlideRigTween = DOTween.To(() => _ladderSlideRig.weight, x => _ladderSlideRig.weight = x, p_weight, p_duration);
            _ladderSlideRigTween.onComplete += () =>
            {
                _ladderSlideRigTween.onComplete = null;
                _ladderSlideRigTween = null;
            };
        }
        #endregion

        #region Toggles
        internal void Jump()
        {
            _animator.SetTrigger("Jump");
        }

        internal void Fall()
        {
            _animator.SetTrigger("Fall");
        }

        internal void Land()
        {
            _animator.SetFloat("LandF", _animator.GetFloat("GravityForce"));
            _animator.SetTrigger("Land");
        }

        internal void SlopeSlide(bool p_slopeSlide)
        {
            _animator.SetBool("SlopeSlide", p_slopeSlide);
        }

        internal void LadderEnter(bool p_fromTop)
        {
            _animator.SetTrigger(p_fromTop ? "LadderEnterTop" : "LadderEnterBottom");
        }

        internal void LadderExit(bool p_toTop)
        {
            _animator.SetTrigger(p_toTop ? "LadderExitTop" : "LadderExitBottom");
        }
        #endregion
    }
}