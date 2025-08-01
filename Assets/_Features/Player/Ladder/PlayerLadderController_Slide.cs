using DG.Tweening;
using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Spread.Player.Ladder
{
    using Player.StateMachine;
    using Spread.Interactions;
    using Spread.Ladder;

    [System.Serializable]
    public class PlayerLadderController_Slide
    {
        [LayoutStart("Legs", ELayout.TitleBox)]
        [SerializeField] private Transform _leftLeg;
        [SerializeField] private Transform _leftLegHint;
        [SerializeField] private Transform _rightLeg;
        [SerializeField] private Transform _rightLegHint;
        
        [LayoutStart("Arms", ELayout.TitleBox)]
        [SerializeField] private Transform _leftArm;
        [SerializeField] private Transform _rightArm;
        
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private Vector3 _rightLegOffset;
        [SerializeField] private Vector3 _rightLegOffsetOffset;
        [SerializeField] private Vector3 _rightArmOffset;
        
        internal void SetupIK(Ladder p_ladder, Vector3 p_playerPos)
        {
            Vector3 offsetToLadderRightEdge = new Vector3(p_ladder.Size.x / 2 + p_ladder.Size.z / 2, 0, 0);

            SetLegsPos(p_ladder, p_playerPos, offsetToLadderRightEdge);
            SetArmsPos(p_ladder, p_playerPos, offsetToLadderRightEdge);
        }

        private void SetLegsPos(Ladder p_ladder, Vector3 p_playerPos, Vector3 p_offsetToLadderRightEdge)
        {
            //Set origin
            Vector3 origin = p_ladder.Rungs[0];
            origin.y = p_playerPos.y;
            _rightLeg.position = origin;
            _leftLeg.position = origin;
            
            //Apply offsets
            Vector3 rightOffset = p_offsetToLadderRightEdge + _rightLegOffset;
            _rightLeg.position += p_ladder.transform.TransformDirection(rightOffset);
            _leftLeg.position += p_ladder.transform.TransformDirection(Vector3.Scale(rightOffset, new Vector3(-1, 1, 1)));

            //Move Right hint
            Vector3 hintPos = _rightLegHint.localPosition;
            hintPos.x = _rightLeg.localPosition.x;
            _rightLegHint.localPosition = hintPos + _rightLegOffsetOffset;
            
            //Move Left hint
            hintPos = _leftLegHint.localPosition;
            hintPos.x = _leftLeg.localPosition.x;
            _leftLegHint.localPosition = hintPos + Vector3.Scale(_rightLegOffsetOffset, new Vector3(-1, 1, 1));
        } 
        
        private void SetArmsPos(Ladder p_ladder, Vector3 p_playerPos, Vector3 p_offsetToLadderRightEdge)
        {
            //Set origin
            Vector3 origin = p_ladder.Rungs[0];
            origin.y = p_playerPos.y;
            _rightArm.position = origin;
            _leftArm.position = origin;
            
            //Apply offsets
            Vector3 rightOffset = p_offsetToLadderRightEdge + _rightArmOffset;
            _rightArm.position += p_ladder.transform.TransformDirection(rightOffset);
            _leftArm.position += p_ladder.transform.TransformDirection(Vector3.Scale(rightOffset, new Vector3(-1, 1, 1)));
        } 
    }
}
