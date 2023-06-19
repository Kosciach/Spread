using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeaponAmmoController : MonoBehaviour
{
    [SerializeField] protected bool _canWeaponShoot; public bool CanWeaponShoot { get { return _canWeaponShoot; } }
    [SerializeField] protected bool _isAmmoReadyToBeShoot; public bool IsAmmoReadyToBeShoot { get { return _isAmmoReadyToBeShoot; } }
    protected WeaponStateMachine _stateMachine;
    protected RangeWeaponData _weaponData;
    protected WeaponShootingController _weaponShootingController;


    private void Awake()
    {
        _stateMachine = GetComponent<WeaponStateMachine>();
        _weaponShootingController = (WeaponShootingController)_stateMachine.DamageDealingController;
        _weaponData = (RangeWeaponData)_stateMachine.DataHolder.WeaponData;

        AbsAwake();
    }
    protected abstract void AbsAwake();



    public abstract void OnShoot();
    public abstract void OnReload();




    public abstract void OnWeaponEquip();
    public abstract void OnWeaponUnEquip();
}
