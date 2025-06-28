using System;
using DG.Tweening;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace Spread.Player.Ladder
{
    using Player.StateMachine;
    using Spread.Interactions;
    using Spread.Ladder;

    public class PlayerLadderController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [LayoutStart("Legs", ELayout.TitleBox)]
        [LayoutStart("Legs/Left", ELayout.TitleBox)]
        [SerializeField, SaintsRow(inline: true)] private LadderIk _leftLeg;
        [LayoutStart("Legs/Right", ELayout.TitleBox)]
        [SerializeField, SaintsRow(inline: true)] private LadderIk _rightLeg;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private bool _useCustomClimbDuration;
        [SerializeField, ShowIf(nameof(_useCustomClimbDuration))] private float _customClimbDuration;
        [SerializeField] private Vector3 _leftLegOffset;
        [SerializeField] private Vector3 _rightLegOffset;

        private Vector3 _leftLegPos;
        private Vector3 _rightLegPos;
        
        private Ladder _currentLadder;
        internal Ladder CurrentLadder => _currentLadder;
        
        
        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;

            _ctx.InteractionsController.OnInteract += Interaction;
        }
        
        private void Interaction(Interactable p_interactable)
        {
            if (p_interactable is Ladder ladder)
            {
                _currentLadder = ladder;
            }
        }
        
        internal void SetIkPos(int p_rungIndex)
        { 
            _leftLegPos = _currentLadder.Rungs[p_rungIndex];
            _rightLegPos = _currentLadder.Rungs[p_rungIndex + 1];

            if (p_rungIndex % 2 == 0)
                (_leftLegPos, _rightLegPos) = (_rightLegPos, _leftLegPos);

            _leftLegPos += _currentLadder.transform.TransformDirection(_leftLegOffset);
            _rightLegPos += _currentLadder.transform.TransformDirection(_rightLegOffset);
            
            UpdateIk();
        }
        
        internal void SetIkPos(int p_rungIndex, float p_climbDuration, int p_climbDirection)
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
            Vector3 legTarget = p_climbDirection == -1
                ? _currentLadder.Rungs[p_rungIndex]
                : _currentLadder.Rungs[p_rungIndex + 1];
            legTarget += _currentLadder.transform.TransformDirection(offset);
            
            //Setup arc
            Vector3 arcDir = -_currentLadder.transform.forward;
            float arcHeight = 0.1f;

            //Lerp pos
            float duration = _useCustomClimbDuration
                ? _customClimbDuration
                : p_climbDuration;
            DOTween.To(() => 0f, x =>
            {
                Vector3 newPos = Vector3.Lerp(currentPos, legTarget, x);
                
                //Add arc
                float arcOffset = Mathf.Sin(x * Mathf.PI) * arcHeight;
                newPos += arcDir * arcOffset;

                //Apply to leg pos
                if (leftLeg)
                    _leftLegPos = newPos;
                else
                    _rightLegPos = newPos;

            }, 1f, duration).SetEase(Ease.InOutQuad);
        }
        
        internal void UpdateIk()
        {
            _leftLeg.Target.position = _leftLegPos;
            _rightLeg.Target.position = _rightLegPos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_leftLegPos, 0.1f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_rightLegPos, 0.1f);
        }
    }

    [System.Serializable]
    public class LadderIk
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _hint;

        public Transform Target => _target;
        public Transform Hint => _hint;
    }
}
