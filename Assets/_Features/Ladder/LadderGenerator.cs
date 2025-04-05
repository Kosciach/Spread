using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;

namespace Spread.Ladder
{
    public class LadderGenerator : MonoBehaviour
    {
        [BoxGroup("Colliders"), SerializeField] private BoxCollider _mainCollider;
        [BoxGroup("Colliders"), SerializeField] private BoxCollider _topCollider;

        [BoxGroup("ExitPoints"), SerializeField] private Transform _topExitPoint;
        [BoxGroup("ExitPoints"), SerializeField] private Transform _bottomExitPoint;

        [BoxGroup("Prefabs"), SerializeField] private Transform _legPrefab;
        [BoxGroup("Prefabs"), SerializeField] private LadderGeneratorHandle _handlePrefab;

        [BoxGroup("Settings"), SerializeField] private float _width;
        [BoxGroup("Settings"), SerializeField] private float _height;
        [BoxGroup("Settings"), SerializeField] private float _handlesSpacing;
        [BoxGroup("Settings"), SerializeField] private float _handlesOffsetFromBottom;

        [BoxGroup("Other"), SerializeField] private Transform _elementsParent;
        [BoxGroup("Other"), SerializeField] private Transform _topPromptPosRef;

        [Foldout("Debug"), SerializeField, ReadOnly] private Transform _leftLeg;
        [Foldout("Debug"), SerializeField, ReadOnly] private Transform _rightLeg;
        [Foldout("Debug"), SerializeField, ReadOnly] private List<LadderGeneratorHandle> _handles;

        [Button]
        private void Generate()
        {
            Clear();

            GenerateLegs();
            GenerateHandles();

            GenerateMainCollider();
            GenerateTopCollider();

            GenerateExitPoints();
        }

        private void GenerateLegs()
        {
            float halfWidth = _width / 2;
            _leftLeg = PrefabUtility.InstantiatePrefab(_legPrefab, _elementsParent) as Transform;
            _rightLeg = PrefabUtility.InstantiatePrefab(_legPrefab, _elementsParent) as Transform;
            _leftLeg.localPosition = new Vector3(-halfWidth, 0, 0);
            _rightLeg.localPosition = new Vector3(halfWidth, 0, 0);

            float height = _height / 2;
            _leftLeg.localScale = new Vector3(1, height, 1);
            _rightLeg.localScale = new Vector3(1, height, 1);
        }

        private void GenerateHandles()
        {
            if (_handlesSpacing <= 0) return;

            float halfWidth = _width / 2;
            float currentHeight = _handlesOffsetFromBottom;
            while (currentHeight <= _height)
            {
                LadderGeneratorHandle handle = PrefabUtility.InstantiatePrefab(_handlePrefab, _elementsParent) as LadderGeneratorHandle;
                handle.transform.localPosition = new Vector3(0, currentHeight, 0);
                handle.transform.localScale = new Vector3(halfWidth, 1, 1);
                _handles.Add(handle);

                currentHeight += _handlesSpacing;
            }
        }

        private void GenerateMainCollider()
        {
            Vector3 size = _mainCollider.size;
            size.x = _width;
            size.y = _height;
            _mainCollider.size = size;

            Vector3 center = Vector3.zero;
            center.y = size.y / 2f;
            _mainCollider.center = center;
        }

        private void GenerateTopCollider()
        {
            Vector3 size = _topCollider.size;
            size.x = _width;
            _topCollider.size = size;

            Vector3 center = Vector3.zero;
            center.y = size.y / 2f + _height;
            _topCollider.center = center;

            _topPromptPosRef.position = transform.position + (transform.up * _height) + (transform.up * (size.y / 2));
        }

        private void GenerateExitPoints()
        {
            _topExitPoint.position = transform.position + (transform.up * _height) + transform.forward * 0.75f;
            _bottomExitPoint.position = transform.position - transform.forward * 0.75f;
        }

        [Button]
        private void Clear()
        {
            for(int i=_elementsParent.childCount-1; i>=0; i--)
            {
                DestroyImmediate(_elementsParent.GetChild(i).gameObject);
            }

            _leftLeg = null;
            _rightLeg = null;
            _handles.Clear();
        }

        internal List<Vector3> GetHandles()
        {
            return _handles.Select(x => x.transform.position).ToList();
        }

        internal List<Vector3> GetSteps()
        {
            return _handles.Select(x => x.Step.position).ToList();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Legs
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_elementsParent.position, 0.1f);

            //Corners
            Gizmos.color = Color.cyan;
            float halfWidth = _width / 2;

            Vector3 bottom = _elementsParent.position + _elementsParent.right * halfWidth;
            Vector3 top = bottom + _elementsParent.up * _height;
            Gizmos.DrawSphere(bottom, 0.1f);
            Gizmos.DrawSphere(top, 0.1f);
            Gizmos.DrawLine(bottom, top);

            bottom = _elementsParent.position - _elementsParent.right * halfWidth;
            top = bottom + _elementsParent.up * _height;
            Gizmos.DrawSphere(bottom, 0.1f);
            Gizmos.DrawSphere(top, 0.1f);
            Gizmos.DrawLine(bottom, top);

            //Handles
            Gizmos.color = Color.red;
            float currentHeight = _handlesOffsetFromBottom;
            while (currentHeight <= _height)
            {
                Gizmos.DrawSphere(_elementsParent.position + _elementsParent.up * currentHeight, 0.1f);
                currentHeight += _handlesSpacing;
            }

            //ExitEnterPoints
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_topExitPoint.position, 0.1f);
            Gizmos.DrawSphere(_bottomExitPoint.position, 0.1f);
        }
#endif
    }
}
