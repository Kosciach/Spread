using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BoltActionAmmoController : BaseWeaponAmmoController
{
    [Header("====References====")]
    [SerializeField] BulletShellEjector _shellEjector;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isRoundInChamber;
    [SerializeField] int _shootCount;
    [SerializeField] int _ammoInMag;
    [SerializeField] AnimatorOverrideController _reloadAnimOveride;




    protected override void AbsAwake()
    {
        _canWeaponShoot = _isRoundInChamber;
        _shootCount = _isRoundInChamber ? _weaponData.AmmoSettings.MagSize - 1 - _ammoInMag : _weaponData.AmmoSettings.MagSize;
    }







    public override void OnShoot()
    {
        //Controll ammo
        _ammoInMag--;
        CheckChamber();
        _ammoInMag = Mathf.Clamp(_ammoInMag, 0, _ammoInMag);


        //Update UI
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.BoltAction.UpdateRoundInChamberColor(_isRoundInChamber);
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


        //Put ammo into weapon
        _ammoInMag += (ammoToReload - 1);
        _isRoundInChamber = true;
        _canWeaponShoot = true;


        _shootCount = 0;

        //Remove ammo from inventory and update UI
        playerAmmoInventory.RemoveAmmo(_weaponData.AmmoSettings.AmmoType, ammoToReload);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);
        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.BoltAction.UpdateRoundInChamberColor(_isRoundInChamber);
    }


    public void OnCharge()
    {
        _shootCount++;
        _shootCount = Mathf.Clamp(_shootCount, 0, _weaponData.AmmoSettings.MagSize + 2);

        if (_shootCount > _weaponData.AmmoSettings.MagSize) return;
        Debug.Log("Charge!");

        //Eject shell
        _shellEjector.EjectShell(_stateMachine.PlayerStateMachine.CoreControllers.Input.MovementInputVector.x);
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

        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.BoltAction.UpdateRoundInChamberColor(_isRoundInChamber);
        CanvasController.Instance.HudControllers.Ammo.SwitchAmmoHud(AmmoHudController.AmmoHudType.BoltAction);
        CanvasController.Instance.HudControllers.Ammo.ChangeRoundIcon(_weaponData.AmmoSettings.AmmoType.SingleRoundIcon);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);

        CanvasController.Instance.HudControllers.Ammo.Toggle(true, 0.1f);
        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.BoltAction.Toggle(true, 0.1f);
    }
}
