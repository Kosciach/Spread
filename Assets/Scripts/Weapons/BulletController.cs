using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _bulletVisual;
    [SerializeField] GameObject _hitEffect;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 1)]
    [SerializeField] float _forwardStrength;
    [Range(0, 1)]
    [SerializeField] float _fallStrength;
    [Range(0, 1)]
    [SerializeField] float _rayLengthMultiplier;
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] float _delayBetweenRays;
    [Space(5)]
    [SerializeField] LayerMask _ignoreMask;



    private Vector3 _rayVector;
    private Vector3 _rayStartPoint;
    private Vector3 _currentRayEnd;


    private float _time;

    private float _currentFallStrength = 0;
    private RaycastHit _hit;




    private void Start()
    {
        _time = Time.time;
        _rayVector = transform.forward;
        _rayStartPoint = transform.position;

        Destroy(gameObject, 10);
    }

    private void Update()
    {
        if(Time.time >= _time)
        {
            Debug.Log("fsd");
            ShootRay();
            _time = Time.time + _delayBetweenRays;
        }
    }

    private void ObjectHit()
    {
        Instantiate(_hitEffect, _hit.point, Quaternion.LookRotation(_hit.normal));

        _hit.rigidbody?.AddForce(-_hit.normal * 2, ForceMode.Impulse);
    }


    private void ShootRay()
    {
        if (Physics.Raycast(_rayStartPoint, transform.forward * _forwardStrength + Vector3.down * _currentFallStrength, out _hit, 1, ~_ignoreMask))
        {
            ObjectHit();
            Destroy(gameObject);
        }



        Debug.DrawRay(_rayStartPoint, (transform.forward * _forwardStrength + Vector3.down * _currentFallStrength) * 1, Color.red, _delayBetweenRays);

        _rayStartPoint += (transform.forward * _forwardStrength + Vector3.down * _currentFallStrength) * 1;
        _currentRayEnd = _rayStartPoint;
        _currentFallStrength += _fallStrength;
    }
}
