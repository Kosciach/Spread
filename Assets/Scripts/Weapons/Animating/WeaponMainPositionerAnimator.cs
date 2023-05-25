using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMainPositionerAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;
    [SerializeField] Transform _positioner;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] MainVectors _currentMainVectors; public MainVectors CurrentMainVectors { get { return _currentMainVectors; } }
    [Space(5)]
    [SerializeField] MainVectors _desiredMainVectors;
    [Space(5)]
    [SerializeField] PositioningMode _positioningMode;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] float _posVectorSmoothSpeed;
    [SerializeField] float _rotVectorSmoothSpeed;


    private IEnumerator _lerpFinishCoroutine;


    private int _positioningMethodIndex => (int)_positioningMode;
    private Action[] _positioningMethods = new Action[2];

    private enum PositioningMode
    { 
        Gameplay, SettingUp
    }



    [System.Serializable]
    public struct MainVectors
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }



    private void Awake()
    {
        _positioningMethods[0] = UpdateTransformVectors;
        _positioningMethods[1] = SetupTransformVectors;
    }
    private void Update()
    {
        _positioningMethods[_positioningMethodIndex]();
    }


    private void UpdateTransformVectors()
    {
        _currentMainVectors.Pos = Vector3.Lerp(_currentMainVectors.Pos, _desiredMainVectors.Pos, _posVectorSmoothSpeed * Time.deltaTime);
        _currentMainVectors.Rot = Vector3.Lerp(_currentMainVectors.Rot, _desiredMainVectors.Rot, _rotVectorSmoothSpeed * Time.deltaTime);

        _positioner.localPosition = _desiredMainVectors.Pos;
        _positioner.localRotation = Quaternion.Euler(_desiredMainVectors.Rot);
    }
    private void SetupTransformVectors()
    {
        _currentMainVectors.Pos = _positioner.localPosition;
        _currentMainVectors.Rot = _positioner.localRotation.eulerAngles;
    }


    public WeaponMainPositionerAnimator SetPos(Vector3 pos, float speed)
    {
        _desiredMainVectors.Pos = pos;
        _posVectorSmoothSpeed = speed;
        return this;
    }
    public WeaponMainPositionerAnimator SetRot(Vector3 rot, float speed)
    {
        _desiredMainVectors.Rot = rot;
        _rotVectorSmoothSpeed = speed;
        return this;
    }


    public void ResetPosOffset()
    {
        _desiredMainVectors.Pos = Vector3.zero;
    }
    public void ResetRotOffset()
    {
        _desiredMainVectors.Rot = Vector3.zero;
    }





    public void CurrentLerpFinished(System.Action afterDelay)
    {
        
        if (_lerpFinishCoroutine != null) StopCoroutine(_lerpFinishCoroutine);

        float timeToFinishLerp = Vector3.Distance(_desiredMainVectors.Pos, _currentMainVectors.Pos) * _posVectorSmoothSpeed * Time.deltaTime;

        _lerpFinishCoroutine = SetLerpFinishDelay(timeToFinishLerp, afterDelay);
        StartCoroutine(_lerpFinishCoroutine);
    }
    private IEnumerator SetLerpFinishDelay(float delay, System.Action afterDelay)
    {
        yield return new WaitForSeconds(delay);

        afterDelay.Invoke();
    }
}
