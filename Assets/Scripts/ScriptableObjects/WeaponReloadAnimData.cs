using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "WeaponReloadAnimData", fileName = "WeaponReloadAnimData")]
public class WeaponReloadAnimData : ScriptableObject
{
    public HandTransformSettings[] RightHandTransforms;
    public HandTransformSettings[] LeftHandTransforms;




    [System.Serializable]
    public struct HandTransformSettings
    {
        public Vector3 Pos;
        public Vector3 Rot;

        public Vector3 Hint_Pos;

        public float TimeToHappen;

        public float Pos_Time;
        public float Rot_Time;
    }
}
