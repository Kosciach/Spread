using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BaseFireMode : MonoBehaviour
{
    protected WeaponShootingController.FireModeTypeEnum _fireModeType; public WeaponShootingController.FireModeTypeEnum FireModeType { get { return _fireModeType; } }
    protected WeaponShootingController _weaponShootingController; public WeaponShootingController WeaponShootingController { get { return _weaponShootingController; } set { _weaponShootingController = value; } }

    protected WeaponDamageDealingInputs _inputs;

    private void Awake()
    {
        _inputs = new WeaponDamageDealingInputs();
        VirtualAwake();
    }
    protected virtual void VirtualAwake()
    {

    }




    private void OnEnable()
    {
        _inputs.Enable();
    }
    private void OnDisable()
    {
        _inputs.Disable();
    }
}