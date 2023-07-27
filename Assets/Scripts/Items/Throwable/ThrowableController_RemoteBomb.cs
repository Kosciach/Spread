using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_RemoteBomb : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;


    private ThrowableInputs _inputs;



    protected override void OnAwake()
    {
        _inputs = new ThrowableInputs();
        _inputs.Disable();
    }
    private void Start()
    {
        _inputs.RemoteBomb.Detonate.performed += ctx =>
        {
            _inputs.Disable();
            Explode(_explosionParticle, 7, 9, 1200);
            Destroy(gameObject);
        };
    }


    public override void OnSafe()
    {

    }
    public override void OnInHand()
    {

    }
    public override void OnThrown()
    {
        _stateMachine.ChangeLayer(transform, 7);

    }
}