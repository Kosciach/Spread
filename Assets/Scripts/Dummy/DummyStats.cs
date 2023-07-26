using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyStats : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
    }
}
