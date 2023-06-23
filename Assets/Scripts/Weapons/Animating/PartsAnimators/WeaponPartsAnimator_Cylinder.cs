using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartsAnimator_Cylinder : BaseWeaponPartsAnimator
{
    [Header("====References====")]
    [SerializeField] Transform _cylinder;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0.1f, 0.5f)]
    [SerializeField] float _animationTime;
    [Range(1, 8)]
    [SerializeField] int _cylinderSize;
    [Range(-1, 1)]
    [SerializeField] int _direction;

    private Quaternion _startingRotation;
    private float _currentRotation;




    private void Awake()
    {
        _startingRotation = _cylinder.localRotation;
        _currentRotation = 0;
    }


    public override void OnShoot(bool isAmmoReadyToBeShoot)
    {
        LeanTween.value(_currentRotation, (_currentRotation + (360 / _cylinderSize)), _animationTime).setOnUpdate((float val) =>
        {
            _cylinder.localRotation = Quaternion.Euler(val, 90, 90);
        }).setOnComplete(() =>
        {
            _currentRotation += 360 / _cylinderSize;
        });
    }
    public override void OnReload()
    {
        _cylinder.localRotation = _startingRotation;
        _currentRotation = 0;
    }
}