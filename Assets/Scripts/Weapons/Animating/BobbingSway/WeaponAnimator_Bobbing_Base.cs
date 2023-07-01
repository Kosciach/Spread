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
    [SerializeField] WeaponAnimator.PosRotStruct _smoothVectors; public WeaponAnimator.PosRotStruct SmoothVectors { get { return _smoothVectors; } }
    [SerializeField] WeaponAnimator.PosRotStruct _rawVectors;

    [SerializeField] bool _toggle;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float[] _speedMultipliers;
    [Range(0, 10)]
    [SerializeField] float[] _distanceMultipliers;
    [Space(10)]
    [Range(0, 10)]
    [SerializeField] float _smoothSpeed;


    private int _bobbingTypeIndex => (int)(_bobbingController.WeaponAnimator.PlayerStateMachine.MovementControllers.Movement.OnGround.CurrentMovementVector.magnitude / 4 + 0.3f);




    private void Update()
    {
        SetRawVectors();

        SmoothOutVectors();
    }



    private void SetRawVectors()
    {
        float lowStaminaBobStrength = _bobbingController.WeaponAnimator.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.LowStaminaBobStrength;
        float lowStaminaBobStrengthCorrected = lowStaminaBobStrength == 0 ? 1 : lowStaminaBobStrength;

        //Pos
        _rawVectors.Pos.x = Mathf.Sin(Time.time * 3 * _speedMultipliers[_bobbingTypeIndex]) * 0.5f * (_distanceMultipliers[_bobbingTypeIndex] * lowStaminaBobStrengthCorrected) / 50;
        _rawVectors.Pos.y = Mathf.Sin(Time.time * 6 * _speedMultipliers[_bobbingTypeIndex]) * 0.25f * (_distanceMultipliers[_bobbingTypeIndex] * lowStaminaBobStrengthCorrected) / 50;

        //Rot
        _rawVectors.Rot.y = Mathf.Cos(Time.time * 3 * _speedMultipliers[_bobbingTypeIndex]) * 0.2f * (_distanceMultipliers[_bobbingTypeIndex] * lowStaminaBobStrengthCorrected);
        _rawVectors.Rot.y *= 2;
    }
    private void SmoothOutVectors()
    {
        float aimWeight = _bobbingController.WeaponAnimator.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim ? 0.1f : 1;
        int toggleWeight = _toggle ? 1 : 0;
        _smoothVectors.Pos = Vector3.Lerp(_smoothVectors.Pos, _rawVectors.Pos * aimWeight, _smoothSpeed * Time.deltaTime) * toggleWeight;
        _smoothVectors.Rot = Vector3.Lerp(_smoothVectors.Rot, _rawVectors.Rot * aimWeight, _smoothSpeed * Time.deltaTime) * toggleWeight;
    }





    public void Toggle(bool toggle)
    {
        _toggle = toggle;
    }
}
