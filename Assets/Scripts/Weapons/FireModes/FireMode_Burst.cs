using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMode_Burst : BaseFireMode
{
    [Range(1, 4)]
    [SerializeField] int _shootCount;
    [Range(0, 1)]
    [SerializeField] float _delayBetweenShoots;
    private float _timeToBurst = 10;
    private float _currentTimeToBurst;




    protected override void VirtualAwake()
    {
        _fireModeType = WeaponShootingController.FireModeTypeEnum.Burst;
    }
    private void Start()
    {
        _currentTimeToBurst = _timeToBurst;

        _inputs.Range.Shoot.performed += ctx =>
        {
            if (!_isInputReady) return;

            StartCoroutine(Burst());
        };
    }

    private void Update()
    {
        CheckTimeToShoot();
    }


    private void CheckTimeToShoot()
    {
        _currentTimeToBurst -= _weaponData.RangeStats.FireRate * 5 * Time.deltaTime;
        _currentTimeToBurst = Mathf.Clamp(_currentTimeToBurst, 0, _timeToBurst);
        _isInputReady = _currentTimeToBurst <= 0;
    }




    private IEnumerator Burst()
    {
        for (int i = 0; i < _shootCount; i++)
        {
            _weaponShootingController.Shoot();

            yield return new WaitForSeconds(_delayBetweenShoots);
        }

        _currentTimeToBurst = _timeToBurst;
    }
}