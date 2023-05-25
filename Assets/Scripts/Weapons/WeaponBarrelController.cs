using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBarrelController : MonoBehaviour
{
    private WeaponStateMachine _stateMachine;
    private Transform _target;


    private void Awake()
    {
        _stateMachine = transform.parent.GetComponent<WeaponStateMachine>();
        _target = GameObject.FindGameObjectWithTag("WeaponBarrelTarget").transform;
    }


    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget < 2) return;

        transform.LookAt(_target);
    }
}
