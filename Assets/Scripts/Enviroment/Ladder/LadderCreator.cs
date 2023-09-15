using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCreator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] protected LadderParts _parts;
    [SerializeField] protected BoxCollider _collider;
    [SerializeField] protected Transform _stepsParent;




    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] protected Transform _stepPrefab;
    [Space(5)]
    [SerializeField] protected Transform _ladderOrigin;
    [SerializeField] protected Transform _ladderTop;
    [SerializeField] protected Transform _armsSpacing;
    [SerializeField] protected Transform _firstStep;
    [Range(0.1f, 2)]
    [SerializeField] protected float _stepsSpacing;


    protected float _ladderHeight;
    protected float _halfLadderWidth;



    [System.Serializable]
    public struct LadderParts
    {
        public Transform LeftArm;
        public Transform RightArm;
        public List<Transform> Steps;
    }




    protected void Awake()
    {
        SetLadderHeight();
        SetArmsSpacing();
        HandleSteps();
        HandleCollider();
        DisableMeshes();
    }


    private void SetLadderHeight()
    {
        Vector3 pos = _ladderTop.position;
        pos.x = transform.position.x;
        pos.z = transform.position.z;
        _ladderTop.position = pos;

        _ladderHeight = (_ladderTop.position.y - transform.position.y)/2;
        _parts.LeftArm.parent.localScale = new Vector3(1, _ladderHeight, 1);
    }
    private void SetArmsSpacing()
    {
        Vector3 pos = _armsSpacing.position;
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        _armsSpacing.position = pos;

        _halfLadderWidth = Vector3.Distance(_armsSpacing.position, transform.position);
        _parts.LeftArm.localPosition = new Vector3(0, _parts.LeftArm.localScale.y, -_halfLadderWidth);
        _parts.RightArm.localPosition = new Vector3(0, _parts.RightArm.localScale.y, _halfLadderWidth);
    }
    private void HandleSteps()
    {
        Vector3 pos = _firstStep.position;
        pos.x = transform.position.x;
        pos.z = transform.position.z;
        _firstStep.position = pos;

        float spaceForSteps = _ladderTop.position.y - _firstStep.position.y + 0.3f;
        float tempStepCount = spaceForSteps / _stepsSpacing;
        int stepCount = (int)tempStepCount;

        for (int i=0; i<stepCount; i++)
        {
            Transform newStep = Instantiate(_stepPrefab, _stepsParent);
            newStep.position = _firstStep.position + Vector3.up * (_stepsSpacing * i);
            newStep.localScale = new Vector3(0.1f, _halfLadderWidth, 0.1f);

            _parts.Steps.Add(newStep);
        }
    }
    private void HandleCollider()
    {
        Vector3 size = _collider.size;
        size.y = _ladderHeight * 2;
        size.z = (_halfLadderWidth + 0.1f) *2;
        _collider.size = size;

        Vector3 center = _collider.center;
        center.y = size.y / 2;
        _collider.center = center;
    }
    private void DisableMeshes()
    {
        _ladderOrigin.localScale = Vector3.zero;
        _ladderTop.localScale = Vector3.zero;
        _armsSpacing.localScale = Vector3.zero;
        _firstStep.localScale = Vector3.zero;
    }
}
