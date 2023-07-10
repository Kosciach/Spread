using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoController_Chamber : BaseWeaponAmmoController
{
    [Header("====Debugs====")]
    [SerializeField] bool _isRoundInChamber;
    [SerializeField] int _ammoInMag;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] bool _canHoldExtraRound;


    private Action<int, PlayerInventory_Ammo>[] _reloadMethods = new Action<int, PlayerInventory_Ammo>[2];



    protected override void AbsAwake()
    {
        _isAmmoReadyToBeShoot = _isRoundInChamber;
        _reloadMethods[0] = ReloadWhenRoundIsNotInChamber;
        _reloadMethods[1] = ReloadWhenRoundIsInChamber;
    }




    public override void OnShoot()
    {
        //Controll ammo
        _ammoInMag--;
        CheckChamber();
        _ammoInMag = Mathf.Clamp(_ammoInMag, 0, _ammoInMag);

        //Update UI
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.UpdateRoundInChamberColor(_isRoundInChamber);
    }
    private void CheckChamber()
    {
        _isRoundInChamber = _ammoInMag > -1;
        _isAmmoReadyToBeShoot = _isRoundInChamber;
    }




    public override void OnReload()
    {
        //Check if mag is full
        int magSize = _weaponData.AmmoSettings.MagSize;
        if (_ammoInMag >= magSize) return;

        //Check if there is ammo in inventory
        PlayerInventory_Ammo playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;
        if (playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex] <= 0) return;



        _weaponShootingController.CallFireModeOnReload();

        //Calculate ammo to reload
        int ammoToReload = magSize - _ammoInMag;
        ammoToReload = Mathf.Clamp(ammoToReload, 0, playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);

        //Choose reload method
        int reloadMethodIndex = _isRoundInChamber ? 1 : 0;
        reloadMethodIndex = _canHoldExtraRound ? reloadMethodIndex : 0;
        _reloadMethods[reloadMethodIndex](ammoToReload, playerAmmoInventory);

        //Update UI
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.UpdateRoundInChamberColor(_isRoundInChamber);
    }


    private void ReloadWhenRoundIsNotInChamber(int ammoToReload, PlayerInventory_Ammo playerAmmoInventory)
    {
        if (_ammoInMag == (_weaponData.AmmoSettings.MagSize - 1)) return;


        //Put ammo in mag and place one in the chamber
        _ammoInMag += (ammoToReload - 1);
        //_canWeaponShoot = true;
        _isRoundInChamber = true;
        _isAmmoReadyToBeShoot = true;

        //Remove Ammo from inventory
        playerAmmoInventory.RemoveAmmo(_weaponData.AmmoSettings.AmmoType, ammoToReload);
    }
    private void ReloadWhenRoundIsInChamber(int ammoToReload, PlayerInventory_Ammo playerAmmoInventory)
    {
        //Put ammo in mag
        _ammoInMag += ammoToReload;
        _isAmmoReadyToBeShoot = true;

        //Remove Ammo from inventory
        playerAmmoInventory.RemoveAmmo(_weaponData.AmmoSettings.AmmoType, ammoToReload);
    }



    public override void OnWeaponEquip()
    {
        SetUI();
    }
    public override void OnWeaponUnEquip()
    {
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.Toggle.Toggle(false);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Cylinder.Toggle.Toggle(false);
        CanvasController.Instance.HudControllers.Ammo.Toggle.Toggle(false);
    }



    public void SetUI()
    {
        PlayerInventory_Ammo playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;

        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.UpdateRoundInChamberColor(_isRoundInChamber);
        CanvasController.Instance.HudControllers.Ammo.SwitchAmmoHud(HudController_Ammo.AmmoHudType.Chamber);
        CanvasController.Instance.HudControllers.Ammo.ChangeRoundIcon(_weaponData.AmmoSettings.AmmoType.SingleRoundIcon);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);

        CanvasController.Instance.HudControllers.Ammo.Toggle.Toggle(true);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.Toggle.Toggle(true);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Cylinder.Toggle.Toggle(false);
    }
}