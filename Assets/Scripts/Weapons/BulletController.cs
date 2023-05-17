using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _bulletVisual;
    [SerializeField] GameObject _hitEffect;
    private RangeWeaponData _weaponData;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 1)]
    [SerializeField] float _gravityStrength;
    [Space(5)]
    [Range(0, 2)]
    [SerializeField] float _delayBetweenRays;
    [Range(0.01f, 10)]
    [SerializeField] float _raylength;
    [Space(5)]
    [SerializeField] LayerMask _ignoreMask;


    private Vector3 _rayStartPoint;
    private Vector3 _rayTargetPoint;
    private float _currentGravityStrength = 0;


    private float _time;
    private RaycastHit _hit;



    public void PassData(WeaponData weaponData)
    {
        _weaponData = weaponData as RangeWeaponData;
    }
    private void Start()
    {
        _time = Time.time;

        _rayStartPoint = transform.position;
        _rayTargetPoint = _rayStartPoint + transform.forward * _raylength;

        Destroy(gameObject, _weaponData.Range);
    }

    private void Update()
    {
        if(Time.time >= _time)
        {
            ShootRay();
            _time = Time.time + _delayBetweenRays;
        }
    }





    private void ObjectHit()
    {
        Instantiate(_hitEffect, _hit.point, Quaternion.LookRotation(_hit.normal));

        _hit.rigidbody?.AddForceAtPosition(-_hit.normal * _weaponData.CarredForce * 10, _hit.point);
        _hit.transform.GetComponent<IDamageable>()?.TakeDamage(_weaponData.Damage);
    }






    private void SetRayPoints()
    {
        _currentGravityStrength += _gravityStrength/100;

        _rayStartPoint = _rayTargetPoint;
        _rayTargetPoint = _rayStartPoint + transform.forward * _raylength + Vector3.down * _currentGravityStrength;
    }
    private void ShootRay()
    {
        Debug.DrawLine(_rayStartPoint, _rayTargetPoint, Color.green, _delayBetweenRays);
        if(Physics.Linecast(_rayStartPoint, _rayTargetPoint, out _hit, ~_ignoreMask))
        {
            ObjectHit();
            Destroy(gameObject);
        }


        SetRayPoints();
    }
}
