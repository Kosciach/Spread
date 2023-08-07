using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_FragGrenade : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;
    [SerializeField] GameObject _pin;
    [SerializeField] GameObject _lever;

    private float _timerToggle = 0;
    private float _timeToExplosion = 10;
    public float _currentTimeToExplosion = 10;

    public override void OnSafe()
    {
        _timerToggle = 0;
        _currentTimeToExplosion = _timeToExplosion;
    }
    public override void OnInHand()
    {
        _pin.SetActive(false);
        _lever.SetActive(false);

        _timerToggle = -2;
    }
    public override void OnThrown()
    {
        _stateMachine.ChangeLayer(transform, 0);


    }


    private void Update()
    {
        _currentTimeToExplosion += _timerToggle * Time.deltaTime;
        _currentTimeToExplosion = Mathf.Clamp(_currentTimeToExplosion, 0, _timeToExplosion);
        if (_currentTimeToExplosion > 0) return;

        Explode(_explosionParticle, 5, 7, 1000);
        Destroy(gameObject);
    }
}
