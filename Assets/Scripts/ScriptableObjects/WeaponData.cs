using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    [Header("====BaseInfo====")]
    public string WeaponName;
    public WeaponHolderEnum WeaponHolder;
    public WeaponTypeEnum WeaponType;


    [Space(20)]
    [Header("====WeaponTransform====")]
    public Vector3 InHandPosition;
    public Vector3 InHandRotation;


    [Space(20)]
    [Header("====IkFingers====")]
    public FingerPreset FingersPreset;

    [Space(20)]
    [Header("====LeftHandIkTransform====")]
    public Vector3 LeftHand_Position;
    public Vector3 LeftHand_Rotation;


    [Space(20)]
    [Header("====AimTransforms====")]
    [Space(5)]
    public WeaponTransform[] Aim;


    [System.Serializable]
    public struct WeaponTransform
    {
        public Vector3 RightHand_Position;
        public Vector3 RightHand_Rotation;
    }

    public enum WeaponHolderEnum
    { 
        Primary, Secondary, Melee_Big, Melee_Medium, Melee_Small
    }
    public enum WeaponTypeEnum
    {
        Range, Melee
    }
}
