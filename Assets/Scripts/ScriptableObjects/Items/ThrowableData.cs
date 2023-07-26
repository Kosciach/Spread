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
}
