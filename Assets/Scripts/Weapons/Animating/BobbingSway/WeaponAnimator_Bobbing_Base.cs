using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator_Bobbing_Base : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator_Bobbing _bobbingController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] WeaponAnimator.PosRotStruct _weaponVectors; public WeaponAnimator.PosRotStruct WeaponVectors { get { return _weaponVectors; } }
    [SerializeField] WeaponAnimator.PosRotStruct _cameraVectors; public WeaponAnimator.PosRotStruct CameraVectors { get { return _cameraVectors; } }
    [SerializeField] WeaponAnimator.PosRotStruct _tempVectors;

    [SerializeField] bool _toggle;


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


    private Action[] _bobMethods = new Action[3];
    private int _bobbingTypeIndex => (int)(_bobbingController.WeaponAnimator.PlayerStateMachine.MovementControllers.Movement.OnGround.CurrentMovementVector.magnitude / 4 + 0.3f);


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


    private void Awake()
    {
        _bobMethods[0] = BobIdle;
        _bobMethods[1] = BobWalk;
        _bobMethods[2] = BobRun;
    }
    private void Update()
    {
        //_bobMethods[_bobbingTypeIndex]();
        SetBaseBob();

        int weaponVectorsWeight = _bobbingController.WeaponAnimator.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim ? 0 : 1;
        SmoothOutWeaponVectors(weaponVectorsWeight);

        int cameraVectorsWeight = _bobbingController.WeaponAnimator.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim ? 1 : 0;
        SmoothOutCameraVectors(cameraVectorsWeight);
    }



    private void BobIdle()
    {

    }
    private void BobWalk()
    {

    }
    private void BobRun()
    {

    }

    private void SetBaseBob()
    {
        //Pos
        _tempVectors.Pos.x = Mathf.Sin(Time.time * _posSettingsX.Speed * _speedMultipliers[_bobbingTypeIndex]) * _posSettingsX.TravelDistance * _distanceMultipliers[_bobbingTypeIndex] / 50;
        _tempVectors.Pos.y = Mathf.Sin(Time.time * _posSettingsY.Speed * _speedMultipliers[_bobbingTypeIndex]) * _posSettingsY.TravelDistance * _distanceMultipliers[_bobbingTypeIndex] / 50;



        //Rot
        _tempVectors.Rot.x = Mathf.Cos(Time.time * _rotSettingsX.Speed * _speedMultipliers[_bobbingTypeIndex]) * _rotSettingsX.TravelDistance * _distanceMultipliers[_bobbingTypeIndex];
        _tempVectors.Rot.y = Mathf.Cos(Time.time * _rotSettingsY.Speed * _speedMultipliers[_bobbingTypeIndex]) * _rotSettingsY.TravelDistance * _distanceMultipliers[_bobbingTypeIndex];

        _tempVectors.Rot.x *= _rotSettingsX.Strength;
        _tempVectors.Rot.y *= _rotSettingsY.Strength;
    }



    private void SmoothOutWeaponVectors(int vectorsWeight)
    {
        int toggleWeight = _toggle ? 1 : 0;
        _weaponVectors.Pos = Vector3.Lerp(_weaponVectors.Pos, _tempVectors.Pos, _bobbingSmoothSpeed * Time.deltaTime) * vectorsWeight * toggleWeight;
        _weaponVectors.Rot = Vector3.Lerp(_weaponVectors.Rot, _tempVectors.Rot, _bobbingSmoothSpeed * Time.deltaTime) * vectorsWeight * toggleWeight;
    }
    private void SmoothOutCameraVectors(int vectorsWeight)
    {
        int toggleWeight = _toggle ? 1 : 0;
        _cameraVectors.Pos = Vector3.Lerp(_cameraVectors.Pos, _tempVectors.Pos, _bobbingSmoothSpeed * Time.deltaTime) * vectorsWeight * toggleWeight;
        _cameraVectors.Rot = Vector3.Lerp(_cameraVectors.Rot, _tempVectors.Rot, _bobbingSmoothSpeed * Time.deltaTime) * vectorsWeight * toggleWeight;
    }







    public void Toggle(bool toggle)
    {
        _toggle = toggle;
    }
}
