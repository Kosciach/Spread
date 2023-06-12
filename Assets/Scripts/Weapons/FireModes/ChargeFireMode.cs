using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireMode : BaseFireMode
{
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
            _isCharged = false;
            Charge();
        };
    }

    public void Charge()
    {
        _isCharged = true;
    }
}
