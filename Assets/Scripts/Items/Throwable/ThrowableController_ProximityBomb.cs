using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_ProximityBomb : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;
    [SerializeField] GameObject _detector;


    public override void OnActivate()
    {
        this.Delay(1, () =>
        {
            _detector.SetActive(true);
        });
    }

    public override void OnSafe()
    {

    }


    public void OnDetection()
    {
        Explode(_explosionParticle, 7, 9, 1200);
        Destroy(gameObject);
    }
}
