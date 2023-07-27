using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_Dynamite : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;


    public override void OnSafe()
    {

    }
    public override void OnInHand()
    {

    }
    public override void OnThrown()
    {
        _stateMachine.ChangeLayer(transform, 0);

    }





    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(3);


        Explode(_explosionParticle, 6, 8, 1100);

        Destroy(gameObject);
    }
}
