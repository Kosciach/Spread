using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMode_Safety : BaseFireMode
{
    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Safety;
    }
    private void Start()
    {
        _inputs.Range.Shoot.performed += ctx => Debug.Log("PifPaf no");
    }
}