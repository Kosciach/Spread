using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseBobbing : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponBobbingController _bobbingController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _pos; public Vector3 Pos { get { return _pos; } }
    [SerializeField] Vector3 _rot; public Vector3 Rot { get { return _rot; } }


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] BaseBobPosSettings _posSettingsX;
    [SerializeField] BaseBobPosSettings _posSettingsY;
    [Space(10)]
    [SerializeField] BaseBobRotSettings _rotSettingsX;
    [SerializeField] BaseBobRotSettings _rotSettingsY;
    [Space(15)]
    [Range(0, 10)]
    [SerializeField] float _mainSpeed;
    [Range(0, 10)]
    [SerializeField] float _mainAmplitude;
    [Space(10)]
    [Range(0, 10)]
    [SerializeField] float _bobbingSmoothSpeed;



    private Vector3 _posTarget;
    private Vector3 _rotTarget;


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





    private void Update()
    {
        SetBaseBobPos();
        SetBaseBobRot();

        SmoothOutBobbing();
    }


    private void SetBaseBobPos()
    {
        float baseBobSpeedX = _posSettingsX.Speed * _bobbingController.PlayerVelocity * _mainSpeed;
        float baseBobPosX = _posSettingsX.TravelDistance / 100 * Mathf.Cos(baseBobSpeedX * Time.time);
        _posTarget.x = baseBobPosX;

        float baseBobSpeedY = _posSettingsY.Speed * _bobbingController.PlayerVelocity * _mainSpeed;
        float baseBobPosY = _posSettingsY.TravelDistance / 100 * Mathf.Sin(baseBobSpeedY * Time.time);
        _posTarget.y = baseBobPosY;
    }
    private void SetBaseBobRot()
    {
        float baseBobSpeedX = _rotSettingsX.Speed * _bobbingController.PlayerVelocity * _mainSpeed;
        float baseBobRotX = _rotSettingsX.TravelDistance * Mathf.Cos(baseBobSpeedX * Time.time);
        _rotTarget.x = baseBobRotX * _rotSettingsX.Strength;

        float baseBobSpeedY = _rotSettingsY.Speed * _bobbingController.PlayerVelocity * _mainSpeed;
        float baseBobRotY = _rotSettingsY.TravelDistance * Mathf.Sin(baseBobSpeedY * Time.time);
        _rotTarget.y = baseBobRotY * _rotSettingsY.Strength;
    }




    private void SmoothOutBobbing()
    {
        _pos = Vector3.Lerp(_pos, _posTarget, _bobbingSmoothSpeed * Time.deltaTime);
        _rot = Vector3.Lerp(_rot, _rotTarget, _bobbingSmoothSpeed * Time.deltaTime);
    }
}
