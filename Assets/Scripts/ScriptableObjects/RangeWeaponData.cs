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
}
