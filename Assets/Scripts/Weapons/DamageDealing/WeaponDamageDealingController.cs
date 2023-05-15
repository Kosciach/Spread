using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class WeaponDamageDealingController : MonoBehaviour
{
    protected WeaponDamageDealingInputs _inputs; public WeaponDamageDealingInputs Inputs { get { return _inputs; } }


    private void Awake()
    {
        _inputs = new WeaponDamageDealingInputs();
        VirtualAwake();
        enabled = false;
    }
    public virtual void VirtualAwake()
    {

    }






    private void OnEnable()
    {
        _inputs.Enable();
        VirtualOnEnable();
    }
    private void OnDisable()
    {
        _inputs.Disable();
        VirtualOnDisable();
    }


    public virtual void VirtualOnEnable()
    {

    }
    public virtual void VirtualOnDisable()
    {

    }
}
