using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] float _timeToDestroy;


    private void Awake()
    {
        Destroy(gameObject, _timeToDestroy);
    }
}
