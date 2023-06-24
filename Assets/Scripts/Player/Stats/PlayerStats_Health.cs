using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats_Health : MonoBehaviour, IDamageable
{
    [Header("====References====")]
    [SerializeField] PlayerStatsController _statsController;


    [Space(20)]
    [Header("====Debugs====")]
    [Range(0, 100)]
    [SerializeField] float _health;
    [SerializeField] bool _isDead;
    [SerializeField] bool _canTakaDamage;



    public void TakeDamage(float damage)
    {
        if (_isDead || !_canTakaDamage) return;

        _health -= damage;
        _health = Mathf.Clamp(_health, 0, 100);

        if(_health == 0)
        {
            Die();
        }
    }
    private void Die()
    {
        _isDead = true;

    }



    public void Heal(float heal)
    {
        _health += heal;
        _health = Mathf.Clamp(_health, 0, 100);
    }
}
