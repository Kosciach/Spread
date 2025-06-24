using SaintsField.Playa;
using UnityEngine;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Ladder
{
    public class LadderGenerator : MonoBehaviour
    {
        [LayoutStart("Parents", ELayout.TitleBox)]
        [SerializeField] private Transform _rungsParent;
        [SerializeField] private Transform _railsParent;
        
        [LayoutStart("Prefabs", ELayout.TitleBox)]
        [SerializeField] private Transform _rungPrefab;
        [SerializeField] private Transform _railPrefab;

        [LayoutStart("Settings", ELayout.TitleBox)]
        [SerializeField] private Color _sizeColor;
        [SerializeField] private Vector2 _size;
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
        
        [LayoutStart("Buttons", ELayout.TitleBox | ELayout.Foldout)]
        [Button]
        private void Generate()
        {
            Clear();

            CreateRungs();
            CreateRails();
        }

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

        private void CreateRungs()
        {
            
        }
        
        private void CreateRails()
        {
            
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 halfSize = _size / 2f;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + (Vector3.up * halfSize.y), transform.rotation, transform.localScale);

//Draw Size
            Gizmos.color = _sizeColor;
            Gizmos.DrawCube(Vector3.zero, new Vector3(_size.x, _size.y, 0.2f));

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
