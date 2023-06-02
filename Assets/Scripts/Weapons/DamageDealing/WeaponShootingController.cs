using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] bool _isEquiped;



    private RangeWeaponData _rangeWeaponData => (RangeWeaponData)_stateMachine.DataHolder.WeaponData;
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
            _ammoController.Reload();
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

        _barrelController.RotateBarrel();

        //SpawnBullet
        GameObject newBullet = Instantiate(_bulletPrefab, _barrel.position, _barrel.rotation);
        BulletController newBulletController = newBullet.GetComponent<BulletController>();
        newBulletController.PassData(_stateMachine.DataHolder.WeaponData);

        //SpawnMuzzleFlash
        GameObject muzzleFlash = Instantiate(_muzzleFlashPrefab, _barrel.position, _barrel.rotation);
        muzzleFlash.transform.parent = _barrel;

        //Ammo
        _ammoController.OnShoot();

        //AnimateSlide
        _slideAnimator.MoveSlide(_ammoController.IsRoundInChamber);

        //Recoil
        _stateMachine.PlayerStateMachine.AnimatingControllers.Weapon.Recoil.Recoil(_rangeWeaponData.RecoilSettings);

        //Shake
        CameraShake.Instance.Shake(0.5f, 10);
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


        CanvasController.Instance.HudControllers.Ammo.ChangeFireMode(_currentFireModeType);
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




    public override void WeaponEquiped()
    {
        _reloadToggle = true;
        _isEquiped = true;

        _ammoController.CheckAllAmmoUI();
        CanvasController.Instance.HudControllers.Weapon.UpdateIcon(_stateMachine.DataHolder.WeaponData.Icon);
        CanvasController.Instance.HudControllers.Ammo.ChangeRoundIcon(_rangeWeaponData.AmmoSettings.AmmoType.SingleRoundIcon);
        CanvasController.Instance.HudControllers.Ammo.ChangeFireMode(_currentFireModeType);

        CanvasController.Instance.HudControllers.Ammo.Toggle(true, 0.1f);
        CanvasController.Instance.HudControllers.Weapon.Toggle(true, 0.1f);

        _stateMachine.PlayerStateMachine.AnimatingControllers.Weapon.Sway.SetWeight(_rangeWeaponData.SwayWeight);
    }
    public override void WeaponUnEquiped()
    {
        _reloadToggle = false;
        _isEquiped = false;

        CanvasController.Instance.HudControllers.Ammo.Toggle(false, 0.1f);
        CanvasController.Instance.HudControllers.Weapon.Toggle(false, 0.1f);
    }
}
