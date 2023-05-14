using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    [Header("====BaseInfo====")]
    public string WeaponName;
    public WeaponTypeEnum WeaponHolder;


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


    [System.Serializable]
    public struct WeaponTransform
    {
        public Vector3 RightHand_Position;
        public Vector3 RightHand_Rotation;
    }

    public enum WeaponTypeEnum
    { 
        Primary, Secondary, Melee_Big, Melee_Medium, Melee_Small
    }
}
