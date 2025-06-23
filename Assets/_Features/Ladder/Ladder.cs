using System;
using UnityEngine;
using System.Collections.Generic;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Ladder
{
    using Interactions;

    public class Ladder : Interactable
    {
        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private Vector3 _topPromptPos;
        [SerializeField] private Vector3 _bottomPromptPos;
        
        
        protected override void OnSelect(Transform p_player)
        {
            SetPromptPos(p_player.position);
        }

        protected override void OnUnSelect()
        {
            
        }

        private void SetPromptPos(Vector3 p_playerPos)
        {
            _promptPosRef.localPosition = IsPlayerTop(p_playerPos)
                ? _topPromptPos
                : _bottomPromptPos;
        }

        private bool IsPlayerTop(Vector3 p_playerPos)
        {
            float playerDistanceToTopPrompt = Vector3.Distance(p_playerPos, _topPromptPos);
            float playerDistanceToBottomPrompt = Vector3.Distance(p_playerPos, _bottomPromptPos);

            return playerDistanceToTopPrompt < playerDistanceToBottomPrompt;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.TransformPoint(_topPromptPos), 0.1f);
            
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(transform.TransformPoint(_bottomPromptPos), 0.1f);
        }
#endif
    }
}
