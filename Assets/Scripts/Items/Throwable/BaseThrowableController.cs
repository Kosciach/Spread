using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseThrowableController : MonoBehaviour
{
    protected ThrowableStateMachine _stateMachine;


    private void Awake()
    {
        _stateMachine = GetComponent<ThrowableStateMachine>();
        OnAwake();
    }



    protected virtual void OnAwake() { }
    public abstract void OnSafe();
    public abstract void OnActivate();
}
