using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseBobbing : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponBobbingController _bobbingController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] WeaponAnimator.PosRotStruct _currentVectors; public WeaponAnimator.PosRotStruct CurrentVectors { get { return _currentVectors; } }
    [SerializeField] WeaponAnimator.PosRotStruct _desiredVectors; public WeaponAnimator.PosRotStruct DesiredVectors { get { return _desiredVectors; } }


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] BaseBobPosSettings _posSettingsX;
    [SerializeField] BaseBobPosSettings _posSettingsY;
    [Space(10)]
    [SerializeField] BaseBobRotSettings _rotSettingsX;
    [SerializeField] BaseBobRotSettings _rotSettingsY;
    [Space(15)]
    [Range(0, 10)]
    [SerializeField] float[] _speedMultipliers;
    [Range(0, 10)]
    [SerializeField] float[] _distanceMultipliers;
    [Space(10)]
    [Range(0, 10)]
    [SerializeField] float _bobbingSmoothSpeed;


    [System.Serializable]
    public struct BaseBobPosSettings
    {
        [Range(0, 2)]
        public float TravelDistance;
        [Range(0, 10)]
        public float Speed;
    }
    [System.Serializable]
    public struct BaseBobRotSettings
    {
        [Range(0, 2)]
        public float TravelDistance;
        [Range(0, 10)]
        public float Speed;
        [Range(-10, 10)]
        public float Strength;
    }


    private int _speedSelector => (int)(_bobbingController.PlayerVelocity/4 + 0.4f);


    private void Update()
    {
        SetBaseBobPos();
        SetBaseBobRot();

        SmoothOutBobbing();
    }


    private void SetBaseBobPos()
    {
        _desiredVectors.Pos.x = Mathf.Sin(Time.time * _posSettingsX.Speed * _speedMultipliers[_speedSelector]) * _posSettingsX.TravelDistance * _distanceMultipliers[_speedSelector] / 50;
        _desiredVectors.Pos.y = Mathf.Sin(Time.time * _posSettingsY.Speed * _speedMultipliers[_speedSelector]) * _posSettingsY.TravelDistance * _distanceMultipliers[_speedSelector] / 50;

    }


    private void SetBaseBobRot()
    {
        _desiredVectors.Rot.x = Mathf.Cos(Time.time * _rotSettingsX.Speed * _speedMultipliers[_speedSelector]) * _rotSettingsX.TravelDistance * _distanceMultipliers[_speedSelector];
        _desiredVectors.Rot.y = Mathf.Cos(Time.time * _rotSettingsY.Speed * _speedMultipliers[_speedSelector]) * _rotSettingsY.TravelDistance * _distanceMultipliers[_speedSelector];

        _desiredVectors.Rot.x *= _rotSettingsX.Strength;
        _desiredVectors.Rot.y *= _rotSettingsY.Strength;
    }

    private void SmoothOutBobbing()
    {
        _currentVectors.Pos = Vector3.Lerp(_currentVectors.Pos, _desiredVectors.Pos, _bobbingSmoothSpeed * Time.deltaTime);
        _currentVectors.Rot = Vector3.Lerp(_currentVectors.Rot, _desiredVectors.Rot, _bobbingSmoothSpeed * Time.deltaTime);
    }
}
