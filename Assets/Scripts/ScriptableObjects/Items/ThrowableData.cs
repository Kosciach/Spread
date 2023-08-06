using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Throwable", fileName = "ThrowableData")]
public class ThrowableData : ItemData
{
    [Header("====Throwable====")]
    [Range(0, 30)]
    public float ThrowStrenght;
    [Range(0, 200)]
    public float Damage;

    public PosRotStruct InHand;
    public PosRotStruct RightHandHold;
    public PosRotStruct RightHandThrow;



    [System.Serializable]
    public struct PosRotStruct
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }

}
