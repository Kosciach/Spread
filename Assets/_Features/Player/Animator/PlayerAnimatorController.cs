using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using FischlWorks;
using SaintsField.Playa;

namespace Spread.Player.Animating
{
    using Movement;

    public class PlayerAnimatorController : PlayerControllerBase
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
        
        [LayoutStart("Layers&Tweens", ELayout.TitleBox)]
        [SerializeField, SerializedDictionary("Layer", "LayerData")]
        private SerializedDictionary<AnimatorLayer, AnimatorLayerData> _layers = new();
        [SerializeField, SerializedDictionary("Rig", "RigData")]
        private SerializedDictionary<AnimatorIkRig, AnimatorIkRigData> _rigs = new();
        
        private Dictionary<int, string> _currentStateName; internal Dictionary<int, string> CurrentStateName => _currentStateName;
        private Dictionary<int, string> _previousStateName; internal Dictionary<int, string> PreviousStateName => _previousStateName;
        public Action<int, string> OnStateChange;
        
        private Tween _ikCrouchTween;
        
        internal bool TransitioningToCrawl => _layers[AnimatorLayer.Crawl].Tween != null;

        protected override void OnSetup()
        {
            _currentStateName = new Dictionary<int, string>();
            _previousStateName = new Dictionary<int, string>();

            for (int i = 0; i < _animator.layerCount; i++)
            {
                _currentStateName.Add(i, "---------");
                _previousStateName.Add(i, "---------");
            }
        }

        protected override void OnDispose()
        {
            OnStateChange = null;
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
        #endregion

        // Idles
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
        
        // Grounded/Normal Movement
        internal void SetMovementType(MovementTypes p_movementType, Vector3 p_moveVector)
        {
            _animator.SetFloat("MovementType", (int)p_movementType, _movementTypeBlendTime, Time.deltaTime);
            _animator.SetBool("IsIdle", p_movementType == MovementTypes.Idle);
            _animator.SetFloat("MovementX", p_moveVector.x, _movementBlendTime, Time.deltaTime);
            _animator.SetFloat("MovementZ", p_moveVector.z, _movementBlendTime, Time.deltaTime);
        }
        
        // In air
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
        
        internal void SetGravityForce(float p_gravityForce)
        {
            _animator.SetFloat("GravityForce", p_gravityForce, 0.2f, Time.deltaTime);
        }

        // Slide
        internal void SlopeSlide(bool p_slopeSlide)
        {
            _animator.SetBool("SlopeSlide", p_slopeSlide);
        }

        // Ladder
        internal void LadderEnter(bool p_fromTop)
        {
            _animator.SetTrigger(p_fromTop ? "LadderEnterTop" : "LadderEnterBottom");
        }

        internal void LadderExit(bool p_toTop)
        {
            _animator.SetTrigger(p_toTop ? "LadderExitTop" : "LadderExitBottom");
        }
        
        // Layers
        internal void SetAnimatorLayerWeight(AnimatorLayer p_layer, float p_weight, float p_duration = 0.5f)
        {
            AnimatorLayerData data = _layers[p_layer];
            
            if (data.Tween != null)
            {
                data.Tween.Kill();
                data.Tween.onUpdate = null;
                data.Tween.onComplete = null;
                data.Tween = null;
            }

            float weight = _animator.GetLayerWeight(data.Index);
            data.Tween = DOTween.To(() => weight, x => weight = x, p_weight, p_duration);
            data.Tween.onUpdate += () =>
            {
                _animator.SetLayerWeight(data.Index, weight);
            };
            data.Tween.onComplete += () =>
            {
                data.Tween.onComplete = null;
                data.Tween = null;
            };
        }
        
        internal void SetAnimatorIkRigWeight(AnimatorIkRig p_rig, float p_weight, float p_duration = 0.5f)
        {
            AnimatorIkRigData data = _rigs[p_rig];
            
            if (data.Tween != null)
            {
                data.Tween.Kill();
                data.Tween.onComplete = null;
                data.Tween = null;
            }

            data.Tween = DOTween.To(() => data.Rig.weight, x => data.Rig.weight = x, p_weight, p_duration);
            data.Tween.onComplete += () =>
            {
                data.Tween.onComplete = null;
                data.Tween = null;
            };
        }
        
        internal void SetCrouchWeight(bool p_crouch)
        {
            int layerIndex = _animator.GetLayerIndex("CrouchHandsLayer");
            float targetWeight = p_crouch ? 1.0f : 0.0f;
            
            _animator.SetFloat("CrouchWeight", targetWeight, 0.2f, Time.deltaTime);
            _animator.SetLayerWeight(layerIndex, _animator.GetFloat("CrouchWeight"));
        }
    }
}