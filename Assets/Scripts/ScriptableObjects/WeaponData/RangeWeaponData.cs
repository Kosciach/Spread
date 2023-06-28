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
    [Space(5)]
    public AmmoSettingsStruct AmmoSettings;
    [Space(5)]
    public CrosshairSettingStruct CrosshairSetting;





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
        [Range(0, 5)]
        public float AccuracyOffset;
        [Range(0, 100)]
        public float PenetrationForce;
    }

    [System.Serializable]
    public struct RecoilSettingsStruct
    {
        [Header("====WeaponRecoil====")]
        [Range(0.1f, 5)]
        public float Speed;
        [Space(5)]
        [Range(0, 0.5f)]
        public float BackPush;
        [Range(0, 10)]
        public float RotX;
        [Range(0, 5)]
        public float RotY;
        [Range(0, 5)]
        public float RotZ;

        [Space(5)]
        [Header("====CameraRecoil====")]
        [Range(0, 10)]
        public float Vertical;
        [Range(0, 5)]
        public float Horizontal;

        [Range(0, 5)]
        public float CameraShake;
    }

    [System.Serializable]
    public struct AmmoSettingsStruct
    {
        public int MagSize;
        public Ammo AmmoType;
    }

    [System.Serializable]
    public struct CrosshairSettingStruct
    {
        [Range(0, 5)]
        public float AccuracyOffsetMultiplier;
        public HudController_Crosshair.CrosshairTypeEnum CrosshairType;
    }
}
