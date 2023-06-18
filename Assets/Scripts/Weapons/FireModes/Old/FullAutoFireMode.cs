using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class FullAutoFireMode : BaseFireMode
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
        if(_isShootingInput && CheckTimeToShoot())
        {
            _currentTimeToShoot = _timeToShoot;
            _weaponShootingController.Shoot();
        }
    }


    private bool CheckTimeToShoot()
    {
        _currentTimeToShoot -= _weaponData.RangeStats.FireRate * 10 * Time.deltaTime;
        _currentTimeToShoot = Mathf.Clamp(_currentTimeToShoot, 0, _timeToShoot);

        return _currentTimeToShoot == 0;
    }
}
