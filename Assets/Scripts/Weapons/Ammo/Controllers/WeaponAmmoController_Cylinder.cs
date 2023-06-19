using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoController_Cylinder : BaseWeaponAmmoController
{
    [Header("====Debugs====")]
    [SerializeField] int _ammoInCylinder; public int AmmoInCylinder { get { return _ammoInCylinder; } }


    protected override void AbsAwake()
    {
        _isAmmoReadyToBeShoot = _ammoInCylinder > 0;
    }




    public override void OnShoot()
    {
        //Controll ammo
        _ammoInCylinder--;
        _isAmmoReadyToBeShoot = _ammoInCylinder > 0;
        _ammoInCylinder = Mathf.Clamp(_ammoInCylinder, 0, _ammoInCylinder);


        //Update UI
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInCylinder);
    }
    public override void OnReload()
    {
        //Check if mag is full
        int magSize = _weaponData.AmmoSettings.MagSize;
        if (_ammoInCylinder >= magSize) return;


        //Check if there is ammo in inventory
        PlayerAmmoInventory playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;
        if (playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex] <= 0) return;


        //Calculate ammo to reload
        int ammoToReload = magSize - _ammoInCylinder;
        ammoToReload = Mathf.Clamp(ammoToReload, 0, playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);


        //Add ammo to cylinder
        _ammoInCylinder += ammoToReload;
        _isAmmoReadyToBeShoot = true;


        //Remove ammo from inventory and update UI
        playerAmmoInventory.RemoveAmmo(_weaponData.AmmoSettings.AmmoType, ammoToReload);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInCylinder);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);
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

        CanvasController.Instance.HudControllers.Ammo.SwitchAmmoHud(AmmoHudController.AmmoHudType.Cylinder);
        CanvasController.Instance.HudControllers.Ammo.ChangeRoundIcon(_weaponData.AmmoSettings.AmmoType.SingleRoundIcon);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInCylinder);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);

        CanvasController.Instance.HudControllers.Ammo.AmmoHudsControllers.Chamber.Toggle(false, 0.01f);
        CanvasController.Instance.HudControllers.Ammo.Toggle(true, 0.1f);
    }
}
