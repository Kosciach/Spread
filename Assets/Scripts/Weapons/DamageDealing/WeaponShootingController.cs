using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingController : WeaponDamageDealingController
{
    [Header("====References====")]
    [SerializeField] BaseFireMode[] _fireModes;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _barrel;
    [SerializeField] GameObject _muzzleFlash;
    private WeaponBarrelController _barrelController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] BaseFireMode _currentFireMode;
    [SerializeField] int _currentFireModeIndex;
    [SerializeField] FireModeTypeEnum _currentFireModeType;


    public enum FireModeTypeEnum
    {
        Safety, Semi, Auto,
    }



    public override void VirtualAwake()
    {
        _barrel = transform.GetChild(1);
        _barrelController = _barrel.GetComponent<WeaponBarrelController>();

        _fireModes = GetComponents<BaseFireMode>();
        _currentFireMode = _fireModes[0];

        foreach (BaseFireMode fireMode in _fireModes)
        {
            fireMode.WeaponShootingController = this;
            fireMode.enabled = false;
        }
        _currentFireModeType = _currentFireMode.FireModeType;
    }
    private void Start()
    {
        _inputs.Range.ChangeFireMode.performed += ctx => ChangeFireMode();
    }





    public void Shoot()
    {
        GameObject newBullet = Instantiate(_bulletPrefab, _barrel.position, _barrel.rotation);

        BulletController newBulletController = newBullet.GetComponent<BulletController>();
        newBulletController.PassData(_stateMachine.DataHolder.WeaponData);

        Instantiate(_muzzleFlash, _barrel.position, _barrel.rotation);
    }








    private void ChangeFireMode()
    {
        _currentFireModeIndex++;
        _currentFireModeIndex = _currentFireModeIndex == _fireModes.Length ? 0 : _currentFireModeIndex;
        _currentFireMode = _fireModes[_currentFireModeIndex];

        foreach (BaseFireMode fireMode in _fireModes) fireMode.enabled = false;
        _currentFireMode.enabled = true;

        _currentFireModeType = _currentFireMode.FireModeType;
    }



    public override void VirtualOnEnable()
    {
        _currentFireMode.enabled = true;
        _barrelController.enabled = true;
    }
    public override void VirtualOnDisable()
    {
        _currentFireMode.enabled = false;
        _barrelController.enabled = false;
    }
}
