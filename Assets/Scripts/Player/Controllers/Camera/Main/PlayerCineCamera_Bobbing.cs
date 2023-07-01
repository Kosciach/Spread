using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCamera_Bobbing : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;
    [SerializeField] CineCameraPositionOffset _positionOffset;
    [SerializeField] CineCameraRotationOffset _rotationOffset;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _smoothRot;
    [SerializeField] Vector3 _rawRot;
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



    private void Update()
    {
        float lowStaminaBobStrength = _cineCameraController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.LowStaminaBobStrength;
        float lowStaminaBobStrengthCorrected = lowStaminaBobStrength == 0 ? 1 : lowStaminaBobStrength;

        if (_cineCameraController.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim || lowStaminaBobStrength > 0)
        {
            SetBobbing(lowStaminaBobStrengthCorrected);
            SmoothOutBobbing();
        }
    }



    private void SetBobbing(float lowStaminaBobStrengthCorrected)
    {
        int bobbingTypeIndex = (int)(_cineCameraController.PlayerStateMachine.MovementControllers.Movement.OnGround.CurrentMovementVector.magnitude / 4 + 0.3f);
        bobbingTypeIndex = Mathf.Clamp(bobbingTypeIndex, 0, 1);


        _rawRot.x = Mathf.Sin(Time.time * 10 * _speedMultipliers[bobbingTypeIndex]) * (1 * _distanceMultipliers[bobbingTypeIndex] * lowStaminaBobStrengthCorrected);
        _rawRot.y = Mathf.Cos(Time.time * 5 * _speedMultipliers[bobbingTypeIndex]) * (2 * _distanceMultipliers[bobbingTypeIndex] * lowStaminaBobStrengthCorrected);
    }
    private void SmoothOutBobbing()
    {
        _smoothRot = Vector3.Lerp(_smoothRot, _rawRot, _smoothSpeed * Time.deltaTime);
        _rotationOffset.m_BobbingOffset = _smoothRot;
    }



    public void Toggle(bool toggle)
    {
        _toggle = toggle;
    }
}
