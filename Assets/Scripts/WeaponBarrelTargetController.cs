using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBarrelTargetController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _target;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _playerMask;

    private void Update()
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~_playerMask)) return;

        _target.position = hit.point;
    }
}
