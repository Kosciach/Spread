using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Transform _leftHandIk;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] AnimatingModeEnum _animatingMode;
    [Space(10)]
    [SerializeField] TransformVectorsStruct _currentTransformVectors;
    [SerializeField] TransformVectorsStruct _desiredTransformVectors;
    [Space(10)]
    [SerializeField] VectorSpeeds _vectorSpeeds;


    private Action[] _animatingModesMethods = new Action[2];
    private int _animatingModeIndex => (int)_animatingMode;



    [System.Serializable] 
    private struct TransformVectorsStruct
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }

    [System.Serializable]
    private struct VectorSpeeds
    {
        [Range(0, 10)]
        public float Pos;
        [Range(0, 10)]
        public float Rot;
    }


    private enum AnimatingModeEnum
    { 
        Gameplay, SettingUp
    }




    private void Awake()
    {
        _animatingModesMethods[0] = UpdateVectors;
        _animatingModesMethods[1] = SetupVectors;
    }
    private void Update()
    {
        _animatingModesMethods[_animatingModeIndex]();

        ApplyVectors();
    }


    private void UpdateVectors()
    {
        _currentTransformVectors.Pos = Vector3.Lerp(_currentTransformVectors.Pos, _desiredTransformVectors.Pos, _vectorSpeeds.Pos * Time.deltaTime);
        _currentTransformVectors.Rot = Vector3.Lerp(_currentTransformVectors.Rot, _desiredTransformVectors.Rot, _vectorSpeeds.Rot * Time.deltaTime);
    }
    private void SetupVectors()
    {

    }


    private void ApplyVectors()
    {
        _leftHandIk.localPosition = _currentTransformVectors.Pos;
        _leftHandIk.localRotation = Quaternion.Euler(_currentTransformVectors.Rot);
    }



    public void SetPos(Vector3 pos, float speed)
    {
        _desiredTransformVectors.Pos = pos;
        _vectorSpeeds.Pos = speed;
    }
    public void SetRot(Vector3 pos, float speed)
    {
        _desiredTransformVectors.Rot = pos;
        _vectorSpeeds.Rot = speed;
    }
}
