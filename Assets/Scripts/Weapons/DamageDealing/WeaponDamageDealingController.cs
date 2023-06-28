using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponDamageDealingController : MonoBehaviour
{
    protected WeaponStateMachine _stateMachine; public WeaponStateMachine StateMachine { get { return _stateMachine; } }
    protected WeaponDamageDealingInputs _inputs; public WeaponDamageDealingInputs Inputs { get { return _inputs; } }
    [SerializeField] protected bool _mainToggle;


    private Action[] _toggleMethods = new Action[2];




    private void Awake()
    {
        _inputs = new WeaponDamageDealingInputs();
        _stateMachine = GetComponent<WeaponStateMachine>();
        _toggleMethods[0] = ToggleOff;
        _toggleMethods[1] = ToggleOn;

        VirtualAwake();
        Toggle(false);
    }
    public abstract void VirtualAwake();




    public void Toggle(bool enable)
    {
        _mainToggle = enable;

        int index = _mainToggle ? 1 : 0;
        _toggleMethods[index]();
    }

    public abstract void ToggleOn();
    public abstract void ToggleOff();




    public abstract void OnWeaponEquip();
    public abstract void OnWeaponUnEquip();

    public abstract void OnPlayerIdle();
    public abstract void OnPlayerWalk();
    public abstract void OnPlayerCrouch();

    public abstract void OnWeaponAim(bool isAim);

    private void OnEnable()
    {
        _inputs.Enable();
    }
    private void OnDisable()
    {
        _inputs.Disable();
    }
}
