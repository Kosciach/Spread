using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_FragGrenade : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;
    [SerializeField] GameObject _pin;
    [SerializeField] GameObject _lever;


    public override void OnActivate()
    {
        _pin.SetActive(false);
        _lever.SetActive(false);

        StartCoroutine(Explode());
    }

    public override void OnSafe()
    {

    }





    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(3);


        Explode(_explosionParticle, 6, 8, 1100);

        Destroy(gameObject);
    }
}
