using IkLayers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireMode : BaseFireMode
{
    [Header("====Debugs====")]
    [SerializeField] bool _isShootingInput;
    [SerializeField] bool _isCharged = true;


    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Charge;
    }
    private void Start()
    {
        _inputs.Range.Shoot.performed += ctx =>
        {
            if (!_isCharged) return;

            _weaponShootingController.Shoot();
        };
    }


    public void ToggleIsCharged(bool enable)
    {
        _isCharged = enable;
    }
}
