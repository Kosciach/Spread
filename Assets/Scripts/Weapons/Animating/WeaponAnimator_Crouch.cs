using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator_Crouch : MonoBehaviour
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
    [SerializeField] WeaponAnimator.PosRotStruct _baseVectors;
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _posSmoothSpeed;
    [Range(0, 10)]
    [SerializeField] float _rotSmoothSpeed;




    private void Update()
    {
        UpdateVectors();
    }




    private void UpdateVectors()
    {
        _currentVectors.Pos = Vector3.Lerp(_currentVectors.Pos, _desiredVectors.Pos, _posSmoothSpeed * Time.deltaTime);
        _currentVectors.Rot = Vector3.Lerp(_currentVectors.Rot, _desiredVectors.Rot, _rotSmoothSpeed * Time.deltaTime);
    }


    public void Toggle(bool enable)
    {
        _toggle = enable ? 1 : 0;
        _desiredVectors.Pos = _baseVectors.Pos * _toggle;
        _desiredVectors.Rot = _baseVectors.Rot * _toggle;
    }
}
