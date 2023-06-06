using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForBulletColliderInfo : MonoBehaviour
{
    [Header("====Settings====")]
    [SerializeField] GameObject _hitEffect; public GameObject HitEffect { get { return _hitEffect; } }
    [Space(5)]
    [SerializeField] float _bulletResistance; public float BulletResistance { get { return _bulletResistance; } }
}
