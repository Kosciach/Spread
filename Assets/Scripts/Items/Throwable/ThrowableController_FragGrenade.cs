using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_FragGrenade : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;
    [SerializeField] GameObject _pin;
    [SerializeField] GameObject _lever;


    public override void OnSafe()
    {

    }
    public override void OnInHand()
    {
        _pin.SetActive(false);
        _lever.SetActive(false);
    }
    public override void OnThrown()
    {
        _stateMachine.ChangeLayer(transform, 0);
        StartCoroutine(Explode());

    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2);


        Explode(_explosionParticle, 5, 7, 1000);

        Destroy(gameObject);
    }
}
