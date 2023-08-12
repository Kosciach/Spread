using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_ProximityBomb : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;
    [SerializeField] GameObject _detector;


    public override void OnSafe()
    {
        _detector.SetActive(false);
    }
    public override void OnInHand()
    {

    }
    public override void OnThrown()
    {
        _stateMachine.ChangeLayer(transform, 7);
        _detector.gameObject.layer = 0;
        this.Delay(1, () => { _detector.SetActive(true); });
    }


    public void OnDetection()
    {
        Explode(_explosionParticle, 7, 9, 1200);
        Destroy(gameObject);
    }
}
