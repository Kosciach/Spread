using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMode_Auto : BaseFireMode
{
    [SerializeField] bool _isShootingInput;

    private float _timeToShoot = 10;
    private float _currentTimeToShoot;




    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Auto;
    }
    private void Start()
    {
        _currentTimeToShoot = _timeToShoot;

        _inputs.Range.Shoot.started += ctx => _isShootingInput = true;
        _inputs.Range.Shoot.canceled += ctx => _isShootingInput = false;
    }

    private void Update()
    {
        if (_isShootingInput && _isInputReady)
        {
            _currentTimeToShoot = _timeToShoot;
            _weaponShootingController.Shoot();
        }
    }


    private void CheckTimeToShoot()
    {
        _currentTimeToShoot -= _weaponData.RangeStats.FireRate * 10 * Time.deltaTime;
        _currentTimeToShoot = Mathf.Clamp(_currentTimeToShoot, 0, _timeToShoot);

        _isInputReady = _currentTimeToShoot <= 0;
    }
}