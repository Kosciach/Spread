using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData/RangeWeaponData", fileName = "RangeWeaponData")]
public class RangeWeaponData : WeaponData
{
    [Space(20)]
    [Header("====RightHandIkTransform====")]

    [Space(5)]
    public WeaponTransform Rest;
    [Space(5)]
    public WeaponTransform Hip;
    [Space(5)]
    public WeaponTransform ADS;
    [Space(5)]
    public WeaponTransform Block;
}
