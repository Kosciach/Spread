using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBarrelController : MonoBehaviour
{
    private WeaponStateMachine _stateMachine;
    private Transform _target;
    private RangeWeaponData _rangeWeaponData;


    private void Awake()
    {
        _stateMachine = transform.parent.GetComponent<WeaponStateMachine>();
        _rangeWeaponData = (RangeWeaponData)_stateMachine.DataHolder.WeaponData;
        _target = GameObject.FindGameObjectWithTag("WeaponBarrelTarget").transform;
    }


    public void RotateBarrel()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget < 2) return;

        transform.LookAt(_target);


        Vector3 offset = Vector3.zero;
        offset.x = Random.Range(-_rangeWeaponData.RangeStats.AccuracyOffset, _rangeWeaponData.RangeStats.AccuracyOffset + 1);
        offset.y = Random.Range(-_rangeWeaponData.RangeStats.AccuracyOffset, _rangeWeaponData.RangeStats.AccuracyOffset + 1);
        offset.z = Random.Range(-_rangeWeaponData.RangeStats.AccuracyOffset, _rangeWeaponData.RangeStats.AccuracyOffset + 1);
        transform.rotation *= Quaternion.Euler(offset);
    }
}
