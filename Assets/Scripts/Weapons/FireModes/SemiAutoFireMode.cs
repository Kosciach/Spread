using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoFireMode : BaseFireMode
{
    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Semi;
    }
    private void Start()
    {
        _inputs.Range.Shoot.performed += ctx => _weaponShootingController.Shoot();
    }
}
