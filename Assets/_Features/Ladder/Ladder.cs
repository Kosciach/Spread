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
    }
}
