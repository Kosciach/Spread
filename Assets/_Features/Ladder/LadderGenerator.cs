using System;
using System.Collections.Generic;
using System.Linq;
using SaintsField.Playa;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;
using UnityEditor;

namespace Spread.Ladder
{
    public class LadderGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Ladder _ladder;
        
        [LayoutStart("Colliders", ELayout.TitleBox)]
        [SerializeField] private BoxCollider _topCollider;
        [SerializeField] private BoxCollider _bottomCollider;
        
        [LayoutStart("Parents", ELayout.TitleBox)]
        [SerializeField] private Transform _rungsParent;
        [SerializeField] private Transform _railsParent;
        
        [LayoutStart("Prefabs", ELayout.TitleBox)]
        [SerializeField] private Transform _rungPrefab;
        [SerializeField] private Transform _railPrefab;

        [LayoutStart("Settings", ELayout.TitleBox | ELayout.Foldout)]
        [SerializeField] private bool _drawGizmos;
        [LayoutStart("Settings/Size", ELayout.TitleBox)]
        [SerializeField] private Color _sizeColor;
        [SerializeField] private Vector3 _size;
        [LayoutStart("Settings/Rungs", ELayout.TitleBox)]
        [SerializeField] private Color _rungsColor;
        [SerializeField, PropRange(0.05f, nameof(_maxRugsSpacing))] private float _rungsSpacing;
        [SerializeField, PropRange(0, nameof(_maxRugsOffset))] private float _rungsTopOffset;
        [SerializeField, PropRange(0, nameof(_maxRugsOffset))] private float _rungsBottomOffset;
        [LayoutStart("Settings/EnterPoints", ELayout.TitleBox)]
        [SerializeField] private Color _enterPointsColor;
        [SerializeField, PropRange(0, nameof(_maxEnterPointIndexOffset))] private int _topEnterPointIndexOffset;
        [SerializeField, PropRange(0, nameof(_maxEnterPointIndexOffset))] private int _bottomEnterPointIndexOffset;
        [LayoutStart("Settings/ExitPoints", ELayout.TitleBox)]
        [SerializeField] private Color _exitPointsColor;
        [SerializeField] private Vector3 _topExitPointOffset;
        [SerializeField] private Vector3 _bottomExitPointOffset;
        [LayoutStart("Settings/AttachPoints", ELayout.TitleBox)]
        [SerializeField] private Color _attachPointsColor;
        [SerializeField] private Vector3 _attachPointsOffset;
        [LayoutStart("Settings/Prompts", ELayout.TitleBox)]
        [SerializeField] private Color _promptPointsColor;
        [SerializeField] private Vector3 _topPromptPoint;
        [SerializeField] private Vector3 _bottomPromptPoint;
        
        //Editor Variables
        private float _maxRugsSpacing => _size.y / 4f;
        private float _maxRugsOffset => _size.y / 2f;
        private float _maxEnterPointIndexOffset;
        

        [LayoutStart("Buttons", ELayout.TitleBox)]
        [Button]
        private void Generate()
        {
            Clear();

            (List<Vector3> rungs, List<Vector3> attachPoints) = CreateRungs();
            CreateRails();
            AlignColliders();

            PassPointsToLadder(rungs, attachPoints);
        }

        [LayoutStart("Buttons", ELayout.TitleBox)]
        [Button]
        private void Clear()
        {
            for (int i = _rungsParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_rungsParent.GetChild(i).gameObject);
            }
            
            for (int i = _railsParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_railsParent.GetChild(i).gameObject);
            }
        }

        private (List<Vector3>, List<Vector3>) CreateRungs()
        {
            List<Vector3> rungs = new();
            List<Vector3> attachPoints = new();
            
            Vector3 halfSize = _size / 2f;
            _rungsParent.localPosition = new Vector3(0, halfSize.y, 0);
            
            float rungsStartPos = -halfSize.y + _rungsBottomOffset;
            float rungsEndPos = halfSize.y - _rungsTopOffset;
            
            float rungsAreaHeight = rungsEndPos - rungsStartPos;
            int rungsCount = Mathf.RoundToInt(rungsAreaHeight / _rungsSpacing);
            float exactSpacing = rungsAreaHeight / (rungsCount - 1);
            
            for (int i = 0; i < rungsCount; i++)
            {
                float rungYPos = rungsStartPos + i * exactSpacing;

                Transform rung = PrefabUtility.InstantiatePrefab(_rungPrefab, _rungsParent) as Transform;
                rung.localPosition = new Vector3(0, rungYPos, 0f);
                rung.localScale = new Vector3(halfSize.x, 1, 1);
                
                rungs.Add(rung.localPosition);
                attachPoints.Add(rung.localPosition + _attachPointsOffset);
            }

            return (rungs, attachPoints);
        }
        
        private void CreateRails()
        {
            Transform leftRail = PrefabUtility.InstantiatePrefab(_railPrefab, _railsParent) as Transform;
            leftRail.name = "LeftRail";
            leftRail.GetChild(0).localScale = new Vector3(_size.z, 1, _size.z);
            leftRail.localPosition = new Vector3((-_size.x / 2) + (_size.z / 2), 0 ,0);
            leftRail.localScale = new Vector3(1, _size.y / 2, 1);
            
            Transform rightRail = PrefabUtility.InstantiatePrefab(_railPrefab, _railsParent) as Transform;
            leftRail.name = "RightRail";
            rightRail.GetChild(0).localScale = leftRail.GetChild(0).localScale;
            rightRail.localPosition = -leftRail.localPosition;
            rightRail.localScale = leftRail.localScale;
        }

        private void AlignColliders()
        {
            //TopCollider
            Vector3 topColliderSize = _topCollider.size;
            topColliderSize.z = _size.z;
            _topCollider.size = topColliderSize;
            _topCollider.center = new Vector3(0, (_topCollider.size.y / 2) + _size.y, 0);
            
            //BottomCollider
            _bottomCollider.size = _size + new Vector3(0, 0, 0.05f);
            _bottomCollider.center = new Vector3(0, (_size.y / 2), 0);
        }

        private void PassPointsToLadder(List<Vector3> p_rungs, List<Vector3> p_attachPoints)
        {
            //Prompts
            Vector3 topPromptPoint = new Vector3(0, _size.y, 0) + _topPromptPoint;
            Vector3 bottomPromptPoint = _bottomPromptPoint;
            
            //Rungs
            for (int i = 0; i < p_rungs.Count; i++)
            {
                Vector3 rung = p_rungs[i];
                p_rungs[i] = _rungsParent.TransformPoint(rung);
            }
            
            //AttachPoints
            for (int i = 0; i < p_attachPoints.Count; i++)
            {
                Vector3 attachPoint = p_attachPoints[i];
                p_attachPoints[i] = _rungsParent.TransformPoint(attachPoint);
            }
            
            //ExitPoints
            Vector3 topExitPoint = transform.TransformPoint((Vector3.up * _size.y) + _topExitPointOffset);
            Vector3 bottomExitPoint = transform.TransformPoint(_bottomExitPointOffset);

            _ladder.SetupLadderData(_size,
                topPromptPoint, bottomPromptPoint,
                p_rungs, p_attachPoints,
                topExitPoint, bottomExitPoint,
                _topEnterPointIndexOffset, _bottomEnterPointIndexOffset);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!_drawGizmos) return;
            
            Vector3 halfSize = _size / 2f;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + (Vector3.up * halfSize.y), transform.rotation, transform.localScale);

            //Draw Size
            Gizmos.color = _sizeColor;
            Gizmos.DrawCube(Vector3.zero, _size);

            //Draw Rungs
            float rungsStartPos = -halfSize.y + _rungsBottomOffset;
            float rungsEndPos = halfSize.y - _rungsTopOffset;
            
            Gizmos.color = _rungsColor;
            Gizmos.DrawSphere(new Vector3(0, rungsEndPos, 0), 0.1f);
            Gizmos.DrawLine(new Vector3(0, rungsStartPos, 0), new Vector3(0, rungsEndPos, 0));
            Gizmos.DrawSphere(new Vector3(0, rungsStartPos, 0), 0.1f);
            
            float rungsAreaHeight = rungsEndPos - rungsStartPos;
            int rungsCount = Mathf.RoundToInt(rungsAreaHeight / _rungsSpacing);
            float exactSpacing = rungsAreaHeight / (rungsCount - 1);
            _maxEnterPointIndexOffset = rungsCount;
            
            for (int i = 0; i < rungsCount; i++)
            {
                float rungYPos = rungsStartPos + i * exactSpacing;
                Vector3 left = new Vector3(-halfSize.x, rungYPos, 0f);
                Vector3 right = new Vector3(halfSize.x, rungYPos, 0f);
                Gizmos.DrawLine(left, right);
            }
            
            //Draw Enter Points
            Gizmos.color = _enterPointsColor;
            
            float topEnterPointYPos = rungsStartPos + (rungsCount - _topEnterPointIndexOffset - 1) * exactSpacing;
            Gizmos.DrawSphere(new Vector3(0, topEnterPointYPos, 0), 0.1f);
            
            float bottomEnterPointYPos = rungsStartPos + _bottomEnterPointIndexOffset * exactSpacing;
            Gizmos.DrawSphere(new Vector3(0, bottomEnterPointYPos, 0), 0.1f);
            
            //Draw Exit Points
            Gizmos.color = _exitPointsColor;
            Gizmos.DrawSphere(new Vector3(0, halfSize.y, 0) + _topExitPointOffset, 0.1f);
            Gizmos.DrawSphere(new Vector3(0, -halfSize.y, 0) + _bottomExitPointOffset, 0.1f);
            
            //Draw Attach Points
            Gizmos.color = _attachPointsColor;
            for (int i = 0; i < rungsCount; i++)
            {
                float rungYPos = rungsStartPos + i * exactSpacing;
                Vector3 startPos = new Vector3(0, rungYPos, 0);
                Vector3 attachPointPos = startPos + _attachPointsOffset;
                Gizmos.DrawLine(startPos, attachPointPos);
                Gizmos.DrawSphere(attachPointPos, 0.05f);
            }
            
            //Draw Prompt Points
            Gizmos.color = _promptPointsColor;
            Gizmos.DrawSphere(new Vector3(0, halfSize.y, 0) + _topPromptPoint, 0.1f);
            Gizmos.DrawSphere(new Vector3(0, -halfSize.y, 0) + _bottomPromptPoint, 0.1f);
        }
#endif
    }
}
