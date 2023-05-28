using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingController : WeaponDamageDealingController
{
    [Header("====References====")]
    [SerializeField] BaseFireMode[] _fireModes;
    [Space(5)]
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _muzzleFlashPrefab;
    [Space(5)]
    [SerializeField] Transform _barrel;
    [SerializeField] WeaponSlideAnimator _slideAnimator;

    private WeaponBarrelController _barrelController;
    private WeaponAmmoController _ammoController; public WeaponAmmoController AmmoController { get { return _ammoController; } }



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
        _ammoController = GetComponent<WeaponAmmoController>();
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
        _inputs.Range.Reload.performed += ctx => _ammoController.Reload();
    }





    public void Shoot()
    {
        if (!_ammoController.IsRoundInChamber)
        {
            Debug.Log("No round in the chamber!");
            return;
        }


        RangeWeaponData rangeWeaponData = (RangeWeaponData)_stateMachine.DataHolder.WeaponData;

        //SpawnBullet
        GameObject newBullet = Instantiate(_bulletPrefab, _barrel.position, _barrel.rotation);
        BulletController newBulletController = newBullet.GetComponent<BulletController>();
        newBulletController.PassData(_stateMachine.DataHolder.WeaponData);

        //SpawnMuzzleFlash
        GameObject muzzleFlash = Instantiate(_muzzleFlashPrefab, _barrel.position, _barrel.rotation);
        muzzleFlash.transform.parent = _barrel;

        //AnimateSlide
        _slideAnimator.MoveSlide();

        //Recoil
        _stateMachine.PlayerStateMachine.AnimatingControllers.Weapon.Recoil.Recoil(rangeWeaponData.RecoilSettings);

        //Shake
        CameraShake.Instance.Shake(0.5f, 10);

        //Ammo
        _ammoController.OnShoot();
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
        _ammoController.enabled = true;
    }
    public override void VirtualOnDisable()
    {
        _currentFireMode.enabled = false;
        _barrelController.enabled = false;
        _ammoController.enabled = false;
    }
}
