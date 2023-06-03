using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBarrelTargetController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Transform _target;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _playerMask;

    private void Update()
    {
        ApplyAdditionalWeaponAnimatorRotation();
        ShootRay();
    }



    private void ApplyAdditionalWeaponAnimatorRotation()
    {
        transform.localRotation = Quaternion.Euler(_playerStateMachine.AnimatingControllers.Weapon.ExtraVectors.Rot);
    }
    private void ShootRay()
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~_playerMask)) return;

        _target.position = hit.point;
    }
}
