using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [Header("====Settings====")]
    [Range(0, 20)]
    [SerializeField] float _delay;



    private void Awake()
    {
        Destroy(gameObject, _delay);
    }
}
