using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAutoFireMode : BaseFireMode
{
    [SerializeField] bool _isShootingInput;

    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Auto;
    }
    private void Start()
    {
        _inputs.Range.Shoot.started += ctx => _isShootingInput = true;
        _inputs.Range.Shoot.canceled += ctx => _isShootingInput = false;
    }

    private void Update()
    {
        if(_isShootingInput)
        {
            Debug.Log("PifPaf fast");
        }
    }
}
