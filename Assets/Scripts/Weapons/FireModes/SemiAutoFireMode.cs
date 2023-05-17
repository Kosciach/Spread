using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoFireMode : BaseFireMode
{
    private RangeWeaponData _weaponData;

    private float _timeToShoot = 10;
    private float _currentTimeToShoot;




    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Semi;
        _weaponData = GetComponent<WeaponDataHolder>().WeaponData as RangeWeaponData;
    }
    private void Start()
    {
        _currentTimeToShoot = _timeToShoot;

        _inputs.Range.Shoot.performed += ctx => 
        {
            if (_currentTimeToShoot > 0) return;

            _weaponShootingController.Shoot();
            _currentTimeToShoot = _timeToShoot;
        };
    }

    private void Update()
    {
        CheckTimeToShoot();
    }


    public void CheckTimeToShoot()
    {
        _currentTimeToShoot -= _weaponData.FireRate * 10 * Time.deltaTime;
        _currentTimeToShoot = Mathf.Clamp(_currentTimeToShoot, 0, _timeToShoot);
    }
}
