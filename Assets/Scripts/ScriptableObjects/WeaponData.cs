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
    [Header("====InHandWeaponTransform====")]
    public Vector3 InHandPosition;
    public Vector3 InHandRotation;

    [Space(10)]
    [Header("====IkFingersPreset====")]
    public FingerPreset FingersPreset;




    [Space(20)]
    [Header("====LeftHandIkTransform====")]
    public LeftHandTransformsStruct LeftHandTransforms;


    [Space(20)]
    [Header("====WeaponIkTransforms====")]
    public WeaponTransformsStruct WeaponTransforms;






    [System.Serializable]
    public struct WeaponTransform
    {
        public Vector3 RightHand_Position;
        public Vector3 RightHand_Rotation;
    }

    [System.Serializable]
    public struct WeaponTransformsStruct
    {
        public WeaponTransform Block;

        [Space(5)]
        public WeaponTransform[] Aim;

        [Space(5)]
        public WeaponTransform Run;

        [Space(5)]
        public WeaponTransform Wall;
    }



    [System.Serializable]
    public struct LeftHandTransform
    {
        public Vector3 LeftHand_Position;
        public Vector3 LeftHand_Rotation;
    }

    [System.Serializable]
    public struct LeftHandTransformsStruct
    {
        public LeftHandTransform Base;
        public LeftHandTransform Block;
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
