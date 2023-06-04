using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInAirAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] WeaponAnimator.PosRotStruct _currentVectors; public WeaponAnimator.PosRotStruct CurrentVectors { get { return _currentVectors; } }
    [Space(5)]
    [SerializeField] WeaponAnimator.PosRotStruct _desiredVectors;
    [Space(10)]
    [Range(0, 1)]
    [SerializeField] int _toggle;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 20)]
    [SerializeField] float _posSmoothSpeed;
    [Range(0, 20)]
    [SerializeField] float _rotSmoothSpeed;



    private PlayerVerticalVelocityController _verticalVelocity;
    private float _gravityStrength => _verticalVelocity.GravityController.CurrentGravityForce - _verticalVelocity.SlopeController.SlopeAngle;




    private void Awake()
    {
        _verticalVelocity = _weaponAnimator.PlayerStateMachine.MovementControllers.VerticalVelocity;
    }
    private void Update()
    {
        SetVectors();
        UpdateVectors();
    }




    private void SetVectors()
    {
        _desiredVectors.Pos.y = _gravityStrength / 100;
        _desiredVectors.Rot.x = _gravityStrength * 4;
    }
    private void UpdateVectors()
    {
        _currentVectors.Pos = Vector3.Lerp(_currentVectors.Pos, _desiredVectors.Pos, _posSmoothSpeed * Time.deltaTime) * _toggle;
        _currentVectors.Rot = Vector3.Lerp(_currentVectors.Rot, _desiredVectors.Rot, _rotSmoothSpeed * Time.deltaTime) * _toggle;
    }



    public void SetPosSpeed(float speed)
    {
        _posSmoothSpeed = speed;
    }
    public void SetRotSpeed(float speed)
    {
        _rotSmoothSpeed = speed;
    }



    public void Toggle(bool enable)
    {
        _toggle = enable ? 1 : 0;
    }
}
