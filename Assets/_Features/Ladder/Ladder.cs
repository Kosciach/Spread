using UnityEngine;
using System.Collections.Generic;

namespace Spread.Ladder
{
    using Interactions;
    using NaughtyAttributes;

    public class Ladder : Interactable
    {
        [BoxGroup("IK"), SerializeField] private Transform _ikLegL;
        [BoxGroup("IK"), SerializeField] private Transform _ikLegR;
        [BoxGroup("IK"), SerializeField] private Transform _ikArmL;
        [BoxGroup("IK"), SerializeField] private Transform _ikArmR;

        public Transform IkLegL => _ikLegL;
        public Transform IkLegR => _ikLegR;
        public Transform IkArmL => _ikArmL;
        public Transform IkArmR => _ikArmR;

        [HorizontalLine(color: EColor.Gray)]
        [BoxGroup("IK"), SerializeField] private Transform _ikThumbTargetL;
        [BoxGroup("IK"), SerializeField] private Transform _ikThumbHintL;
        [BoxGroup("IK"), SerializeField] private Transform _ikThumbTargetR;
        [BoxGroup("IK"), SerializeField] private Transform _ikThumbHintR;

        public Transform IkThumbTargetL => _ikThumbTargetL;
        public Transform IkThumbHintL => _ikThumbHintL;
        public Transform IkThumbTargetR => _ikThumbTargetR;
        public Transform IkThumbHintR => _ikThumbHintR;

        [BoxGroup("References"), SerializeField] private LadderGenerator _generator;
        [BoxGroup("References"), SerializeField] private Transform _topExitPoint;
        [BoxGroup("References"), SerializeField] private Transform _bottomExitPoint;
        [BoxGroup("References"), SerializeField] private Transform _promptPosRefTop;
        [BoxGroup("References"), SerializeField] private Transform _promptPosRefBottom;

        public Transform TopExitPoint => _topExitPoint;
        public Transform BottomExitPoint => _bottomExitPoint;

        private List<Vector3> _handles = new List<Vector3>();
        public IReadOnlyList<Vector3> Handles => _handles;

        private List<Vector3> _steps = new List<Vector3>();
        public IReadOnlyList<Vector3> Steps => _steps;

        protected override void Awake()
        {
            base.Awake();

            _handles = _generator.GetHandles();
            _steps = _generator.GetSteps();
        }

        public override void Select(Transform p_player)
        {
            base.Select(p_player);

            _promptPosRef.position = IsTop(p_player) ? _promptPosRefTop.position : _promptPosRefBottom.position;
        }

        public bool IsTop(Transform p_player)
        {
            float distanceToTopPrompt = Vector3.Distance(p_player.position, _promptPosRefTop.position);
            float distanceToBottomPrompt = Vector3.Distance(p_player.position, _promptPosRefBottom.position);
            return distanceToTopPrompt < distanceToBottomPrompt;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Legs
            Gizmos.color = Color.white;
            Gizmos.DrawCube(_promptPosRefTop.position, Vector3.one * 0.1f);
            Gizmos.DrawCube(_promptPosRefBottom.position, Vector3.one * 0.1f);
        }
#endif
    }
}
