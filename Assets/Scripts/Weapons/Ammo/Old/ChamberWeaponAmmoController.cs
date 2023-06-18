using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberWeaponAmmoController : BaseWeaponAmmoController
{
    [Header("====References====")]
    [SerializeField] WeaponSlideAnimator _slideAnimator;
    [SerializeField] BulletShellEjector _shellEjector;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isRoundInChamber;
    [SerializeField] int _ammoInMag;
    [SerializeField] AnimatorOverrideController _reloadAnimOveride;

    private Action<int>[] _reloadMethods = new Action<int>[2];



    protected override void AbsAwake()
    {
        _canWeaponShoot = _isRoundInChamber;
        _reloadMethods[0] = ReloadWithoutRoundInChamber;
        _reloadMethods[1] = ReloadWithRoundInChamber;
    }







    public override void OnShoot()
    {
        //Controll ammo
        _ammoInMag--;
        CheckChamber();
        _ammoInMag = Mathf.Clamp(_ammoInMag, 0, _ammoInMag);

        //Animate slide
        WeaponSlideAnimator.SlideAnimType slideAnimType = _isRoundInChamber ? WeaponSlideAnimator.SlideAnimType.BackAndForward : WeaponSlideAnimator.SlideAnimType.Back;
        _slideAnimator.MoveSlide(slideAnimType);

        //Eject shell
        _shellEjector.EjectShell(_stateMachine.PlayerStateMachine.CoreControllers.Input.MovementInputVector.x);

        //Update UI
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.Chamber.UpdateRoundInChamberColor(_isRoundInChamber);
    }

    private void CheckChamber()
    {
        _isRoundInChamber = _ammoInMag > -1;
        _canWeaponShoot = _isRoundInChamber;
    }




    public override void OnReload()
    {
        //Check if mag is full
        int magSize = _weaponData.AmmoSettings.MagSize;
        if (_ammoInMag >= magSize) return;

        
        //Check if there is ammo in inventory
        PlayerAmmoInventory playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;
        if (playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex] <= 0) return;


        //Calculate ammo to reload
        int ammoToReload = magSize - _ammoInMag;
        ammoToReload = Mathf.Clamp(ammoToReload, 0, playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);


        //Choose reload method
        int reloadMethodIndex = _isRoundInChamber ? 1 : 0;
        _reloadMethods[reloadMethodIndex](ammoToReload);


        //Move slide back to firing position
        _slideAnimator.MoveSlide(WeaponSlideAnimator.SlideAnimType.BackAndForward);


        //Remove ammo from inventory and update UI
        playerAmmoInventory.RemoveAmmo(_weaponData.AmmoSettings.AmmoType, ammoToReload);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);
        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.Chamber.UpdateRoundInChamberColor(_isRoundInChamber);
    }

    private void ReloadWithRoundInChamber(int ammoToReload)
    {
        //Put ammo in mag
        _ammoInMag += ammoToReload;
    }
    private void ReloadWithoutRoundInChamber(int ammoToReload)
    {
        //Put ammo in mag and place one in the chamber
        _ammoInMag += (ammoToReload-1);
        _isRoundInChamber = true;
        _canWeaponShoot = true;
    }




    public override void OnWeaponEquip()
    {
        SetUI();
    }
    public override void OnWeaponUnEquip()
    {
        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.Chamber.Toggle(false, 0.1f);
    }



    public void SetUI()
    {
        PlayerAmmoInventory playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;

        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.Chamber.UpdateRoundInChamberColor(_isRoundInChamber);
        CanvasController.Instance.HudControllers.Ammo.SwitchAmmoHud(AmmoHudController.AmmoHudType.Chamber);
        CanvasController.Instance.HudControllers.Ammo.ChangeRoundIcon(_weaponData.AmmoSettings.AmmoType.SingleRoundIcon);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);

        CanvasController.Instance.HudControllers.Ammo.Toggle(true, 0.1f);
        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.Chamber.Toggle(true, 0.1f);
    }
}
