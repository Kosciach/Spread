using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData/MeleeWeaponData", fileName = "MeleeWeaponData")]
public class MeleeWeaponData : WeaponData
{
    [Space(20)]
    [Header("====HoldTransforms====")]
    [Space(5)]
    public WeaponTransform Hip;
}
