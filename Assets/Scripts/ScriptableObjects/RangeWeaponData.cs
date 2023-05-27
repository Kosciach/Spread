using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData/RangeWeaponData", fileName = "RangeWeaponData")]
public class RangeWeaponData : WeaponData
{
    [Space(5)]
    [Header("====HoldTransforms====")]
    public HoldTransformsStruct HoldTransforms;
 

    [Space(20)]
    [Header("====Stats====")]
    public RangeStatsStruct RangeStats;
    [Space(5)]
    public RecoilSettingsStruct RecoilSettings;






    [System.Serializable]
    public struct HoldTransformsStruct
    {
        public WeaponTransform Rest;
        [Space(5)]
        public WeaponTransform Hip;
    }

    [System.Serializable]
    public struct RangeStatsStruct
    {
        [Range(0, 120)]
        public float Damage;
        [Range(0, 6)]
        public float Range;
        [Range(0, 30)]
        public float FireRate;
        [Range(0, 100)]
        public float CarredForce;
    }

    [System.Serializable]
    public struct RecoilSettingsStruct
    {
        [Range(0, 0.5f)]
        public float BackPush;
        [Range(0, 45)]
        public float RotX;
        [Range(0, 5)]
        public float RotZ;
    }

}
