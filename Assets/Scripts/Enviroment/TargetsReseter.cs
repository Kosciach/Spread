using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsReseter : MonoBehaviour, IDamageable
{
    [SerializeField] TargetsPlacerScript _targetsPlacerScript;



    public void TakeDamage(float damage)
    {
        _targetsPlacerScript.ResetTargets();
    }
}
