using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    [Header("====BaseInfo====")]
    public string WeaponName;
    public WeaponHolderEnum WeaponHolder;
    public WeaponTypeEnum WeaponType;
    public Vector3 WeaponOriginRotation;
    public bool Fists;


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
    [Header("====BlockTransform====")]
    public WeaponTransform Block;

    [Space(10)]
    [Header("====AimTransforms====")]
    public WeaponTransform[] Aim;

    [Space(10)]
    [Header("====RunTransform====")]
    public WeaponTransform Run;

    [Space(10)]
    [Header("====WallTransform====")]
    public WeaponTransform Wall;



    [System.Serializable]
    public struct WeaponTransform
    {
        public Vector3 RightHand_Position;
        public Vector3 RightHand_Rotation;
    }

    public enum WeaponHolderEnum
    { 
        Primary, Secondary, Melee_Big, Melee_Medium, Melee_Small, Fist
    }
    public enum WeaponTypeEnum
    {
        Range, Melee
    }
}
