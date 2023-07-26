using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_Dynamite : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;


    public override void OnActivate()
    {
        StartCoroutine(Explode());
    }

    public override void OnSafe()
    {

    }





    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2);


        Explode(_explosionParticle, 5, 7, 1000);

        Destroy(gameObject);
    }
}