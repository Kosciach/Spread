using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBarrelController : MonoBehaviour
{
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _accuracyOffsetWeight;

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
        AddAccuracyOffset();
    }
    private void AddAccuracyOffset()
    {
        Vector3 offset = Vector3.zero;
        float weaponAccuracyOffset = _rangeWeaponData.RangeStats.AccuracyOffset;
        offset.x = Random.Range(-weaponAccuracyOffset, weaponAccuracyOffset);
        offset.y = Random.Range(-weaponAccuracyOffset, weaponAccuracyOffset);
        transform.rotation *= Quaternion.Euler(offset * _accuracyOffsetWeight);
    }
}
