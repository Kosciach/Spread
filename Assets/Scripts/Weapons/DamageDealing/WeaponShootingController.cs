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
    [Space(5)]
    [SerializeField] bool _shootToggle;
    [SerializeField] bool _reloadToggle;



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
        _inputs.Range.Reload.performed += ctx =>
        {
            if (!_reloadToggle) return;
            _ammoController.TriggerReload();
        };
    }





    public void Shoot()
    {
        if (!_shootToggle) return;

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
        if (!_shootToggle) return;

        _currentFireModeIndex++;
        _currentFireModeIndex = _currentFireModeIndex == _fireModes.Length ? 0 : _currentFireModeIndex;
        _currentFireMode = _fireModes[_currentFireModeIndex];

        foreach (BaseFireMode fireMode in _fireModes) fireMode.enabled = false;
        _currentFireMode.enabled = true;

        _currentFireModeType = _currentFireMode.FireModeType;
    }



    public override void ToggleOn()
    {
        _shootToggle = true;
        _barrelController.enabled = true;
    }
    public override void ToggleOff()
    {
        _shootToggle = false;
        _barrelController.enabled = false;
    }




    public override void WeaponEquiped()
    {
        Debug.Log("RAnge E");
        _reloadToggle = true;

        _ammoController.CheckAllAmmoUI();
        CanvasController.Instance.HudControllers.Weapon.UpdateIcon(_stateMachine.DataHolder.WeaponData.Icon);

        CanvasController.Instance.HudControllers.Ammo.Toggle(true, 0.1f);
        CanvasController.Instance.HudControllers.Weapon.Toggle(true, 0.1f);
    }
    public override void WeaponUnEquiped()
    {
        Debug.Log("RAnge UnE");
        _reloadToggle = false;

        CanvasController.Instance.HudControllers.Ammo.Toggle(false, 0.1f);
        CanvasController.Instance.HudControllers.Weapon.Toggle(false, 0.1f);
    }
}
