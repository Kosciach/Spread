using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBarrelController : MonoBehaviour
{
    private Transform _target;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("WeaponBarrelTarget").transform;
    }


    private void Update()
    {
        transform.LookAt(_target);
    }
}
