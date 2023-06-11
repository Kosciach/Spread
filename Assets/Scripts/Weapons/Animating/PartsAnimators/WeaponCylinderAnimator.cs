using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCylinderAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _cylinder;
    [SerializeField] Transform _rotationAxis;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 0.5f)]
    [SerializeField] float _animationTime;
    [Range(0, 8)]
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


    public void RotateOnShoot()
    {
        LeanTween.value(_currentRotation, (_currentRotation + (360 / _cylinderSize)), _animationTime).setOnUpdate((float val) =>
        {
            _cylinder.localRotation = Quaternion.Euler(val, 90, 90);
        }).setOnComplete(() =>
        {
            _currentRotation += 360 / _cylinderSize;
        });
    }
    public void ResetRotation()
    {
        _cylinder.localRotation = _startingRotation;
        _currentRotation = 0;
    }
}
