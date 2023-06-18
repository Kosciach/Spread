using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMode_Semi : BaseFireMode
{
    private float _timeToShoot = 10;
    private float _currentTimeToShoot;




    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Semi;
    }
    private void Start()
    {
        _currentTimeToShoot = _timeToShoot;

        _inputs.Range.Shoot.performed += ctx =>
        {
            if (!_isInputReady) return;

            _weaponShootingController.Shoot();
            _currentTimeToShoot = _timeToShoot;
        };
    }

    private void Update()
    {
        CheckTimeToShoot();
    }


    private void CheckTimeToShoot()
    {
        _currentTimeToShoot -= _weaponData.RangeStats.FireRate * 10 * Time.deltaTime;
        _currentTimeToShoot = Mathf.Clamp(_currentTimeToShoot, 0, _timeToShoot);
        _isInputReady = _currentTimeToShoot <= 0;
    }
}
