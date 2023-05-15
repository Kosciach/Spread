using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData/RangeWeaponData", fileName = "RangeWeaponData")]
public class RangeWeaponData : WeaponData
{
    [Space(20)]
    [Header("====HoldTransforms====")]
    [Space(5)]
    public WeaponTransform Rest;
    [Space(5)]
    public WeaponTransform Hip;


    [Space(20)]
    [Header("====RangeStats====")]
    [Range(0, 120)]
    public float Damage;
    [Range(0, 4)]
    public float Range;
    [Range(0, 10)]
    public float FireRate;
}
