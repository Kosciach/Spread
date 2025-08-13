using UnityEngine;
using System.Collections.Generic;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Ladder
{
    using Interactions;

    public class Ladder : Interactable
    {
        [LayoutStart("Parents", ELayout.TitleBox)]
        [SerializeField] private Transform _rungsParent;
        [SerializeField] private Transform _railsParent;
        
        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [SerializeField, ReadOnly] private Vector3 _size;
        [SerializeField, ReadOnly] private Vector3 _topPromptPoint;
        [SerializeField, ReadOnly] private Vector3 _bottomPromptPoint;
        [SerializeField, ReadOnly] private List<Vector3> _rungs;
        [SerializeField, ReadOnly] private List<Vector3> _attachPoints;
        [SerializeField, ReadOnly] private Vector3 _topExitPoint;
        [SerializeField, ReadOnly] private Vector3 _bottomExitPoint;

        public Vector3 Size => _size;
        public IReadOnlyList<Vector3> Rungs => _rungs;
        public IReadOnlyList<Vector3> AttachPoints => _attachPoints;
        public Vector3 TopExitPoint => _topExitPoint;
        public Vector3 BottomExitPoint => _bottomExitPoint;
        
        
        protected override void OnSelect(Transform p_player)
        {
            SetPromptPos(p_player.position);
        }

        private void SetPromptPos(Vector3 p_playerPos)
        {
            _promptPosRef.localPosition = IsPlayerTop(p_playerPos)
                ? _topPromptPoint
                : _bottomPromptPoint;
        }

        public bool IsPlayerTop(Vector3 p_playerPos)
        {
            float playerDistanceToTopPrompt = Vector3.Distance(p_playerPos, _topPromptPoint);
            float playerDistanceToBottomPrompt = Vector3.Distance(p_playerPos, _bottomPromptPoint);

            return playerDistanceToTopPrompt < playerDistanceToBottomPrompt;
        }

        public int GetClosestRungIndex(Vector3 p_pos, int p_topEnterIndexOffset)
        {
            int left = 0;
            int right = _rungs.Count - p_topEnterIndexOffset - 1;
            int result = right;

            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (_rungs[mid].y > p_pos.y)
                {
                    result = mid;
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return result;
        }
        
        internal void SetupLadderData(Vector3 p_size,
            Vector3 p_topPromptPoint, Vector3 p_bottomPromptPoint, 
            List<Vector3> p_rungs, List<Vector3> p_attachPoints,
            Vector3 p_topExitPoint, Vector3 p_bottomExitPoint)
        {
            _size = p_size;
            _topPromptPoint = p_topPromptPoint;
            _bottomPromptPoint = p_bottomPromptPoint;

            _rungs = p_rungs;
            _attachPoints = p_attachPoints;

            _topExitPoint = p_topExitPoint;
            _bottomExitPoint = p_bottomExitPoint;
        }
    }
}
