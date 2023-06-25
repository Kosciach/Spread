using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponShootingController : WeaponDamageDealingController
{
    [Header("====References====")]
    [SerializeField] BaseFireMode[] _fireModes;
    [Space(5)]
    [SerializeField] GameObject _muzzleFlashPrefab;
    [Space(5)]
    [SerializeField] Transform _barrel;

    private BaseBulletSpawner _bulletSpawner;
    private WeaponBarrelController _barrelController;
    private BaseWeaponAmmoController _ammoController;
    private BaseWeaponPartsAnimator _partsAnimator;
    private WeaponFireModesAnimator _fireModesAnimator;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] BaseFireMode _currentFireMode;
    [SerializeField] int _currentFireModeIndex;
    [SerializeField] FireModeTypeEnum _currentFireModeType;
    [Space(5)]
    [SerializeField] bool _shootToggle;
    [SerializeField] bool _reloadToggle;
    [SerializeField] bool _isEquiped;



    private RangeWeaponData _rangeWeaponData;
    public enum FireModeTypeEnum
    {
        Safety, Semi, Auto, Burst, Charge
    }



    public override void VirtualAwake()
    {
        _rangeWeaponData = (RangeWeaponData)_stateMachine.DataHolder.WeaponData;
        _barrel = transform.GetChild(1);

        _bulletSpawner = _barrel.GetComponent<BaseBulletSpawner>();
        _barrelController = _barrel.GetComponent<WeaponBarrelController>();
        _ammoController = GetComponent<BaseWeaponAmmoController>();
        _partsAnimator = GetComponent<BaseWeaponPartsAnimator>();
        _fireModesAnimator = GetComponent<WeaponFireModesAnimator>();



        _fireModes = GetComponents<BaseFireMode>();
        _currentFireMode = _fireModes[0];
        foreach (BaseFireMode fireMode in _fireModes)
        {
            fireMode.WeaponShootingController = this;
            fireMode.enabled = false;
        }
        _currentFireModeType = _currentFireMode.FireModeType;
        _fireModesAnimator.OnFireModeChange(_currentFireModeIndex);
    }
    private void Start()
    {
        _inputs.Range.ChangeFireMode.performed += ctx => ChangeFireMode();
        _inputs.Range.Reload.performed += ctx =>
        {
            if (!_reloadToggle) return;
            _ammoController.OnReload();
            _partsAnimator.OnReload();
        };
    }





    public bool Shoot()
    {
        if (!_shootToggle) return false;

        if (!_ammoController.IsAmmoReadyToBeShoot) return false;

        _barrelController.RotateBarrel();

        //SpawnBullet
        _bulletSpawner.SpawnBullet(_rangeWeaponData);

        //SpawnMuzzleFlash
        GameObject muzzleFlash = Instantiate(_muzzleFlashPrefab, _barrel.position, _barrel.rotation);
        muzzleFlash.transform.parent = _barrel;


        //Ammo
        _ammoController.OnShoot();

        //Parts anim
        _partsAnimator.OnShoot(_ammoController.IsAmmoReadyToBeShoot);

        //Recoil
        _stateMachine.PlayerStateMachine.AnimatingControllers.Weapon.Recoil.Recoil(_rangeWeaponData.RecoilSettings);

        //Shake
        CameraShake.Instance.Shake(_rangeWeaponData.RecoilSettings.CameraShake, 10);


        return true;
    }








    private void ChangeFireMode()
    {
        if (!_isEquiped) return;

        _currentFireModeIndex++;
        _currentFireModeIndex = _currentFireModeIndex == _fireModes.Length ? 0 : _currentFireModeIndex;
        _currentFireMode = _fireModes[_currentFireModeIndex];

        foreach (BaseFireMode fireMode in _fireModes) fireMode.enabled = false;
        _currentFireMode.enabled = true;

        _currentFireModeType = _currentFireMode.FireModeType;
        _fireModesAnimator.OnFireModeChange(_currentFireModeIndex);

        CanvasController.Instance.HudControllers.Firemodes.ChangeFireMode(_currentFireModeType);

        _stateMachine.PlayerStateMachine.AnimatingControllers.Weapon.FireMode.ChangeFireModeAnim();
    }
    public void CallFireModeOnReload()
    {
        _currentFireMode.OnReload();
    }


    public override void ToggleOn()
    {
        _shootToggle = true;
        _barrelController.enabled = true;

        if (!_isEquiped) return;
        _stateMachine.PlayerStateMachine.AnimatingControllers.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_rangeWeaponData, false);
    }
    public override void ToggleOff()
    {
        _shootToggle = false;
        _barrelController.enabled = false;

        if (!_isEquiped) return;
        _stateMachine.PlayerStateMachine.AnimatingControllers.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_rangeWeaponData, true);
    }




    public override void OnWeaponEquip()
    {
        _reloadToggle = true;
        _isEquiped = true;

        _ammoController.OnWeaponEquip();
        CanvasController.Instance.HudControllers.Weapon.UpdateIcon(_stateMachine.DataHolder.WeaponData.Icon);
        CanvasController.Instance.HudControllers.Firemodes.ChangeFireMode(_currentFireModeType);

        CanvasController.Instance.HudControllers.Weapon.Toggle(true, 0.1f);
        CanvasController.Instance.HudControllers.Firemodes.Toggle(true, 0.1f);

        _stateMachine.PlayerStateMachine.AnimatingControllers.Weapon.Sway.SetWeight(_rangeWeaponData.SwayWeight);
    }
    public override void OnWeaponUnEquip()
    {
        _reloadToggle = false;
        _isEquiped = false;

        _ammoController.OnWeaponUnEquip();
        CanvasController.Instance.HudControllers.Weapon.Toggle(false, 0.1f);
        CanvasController.Instance.HudControllers.Ammo.Toggle(false, 0.1f);
        CanvasController.Instance.HudControllers.Firemodes.Toggle(false, 0.1f);
    }
}
