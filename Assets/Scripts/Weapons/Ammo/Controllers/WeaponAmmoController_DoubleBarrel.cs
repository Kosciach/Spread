using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoController_DoubleBarrel : BaseWeaponAmmoController
{
    [Header("====Debugs====")]
    [SerializeField] bool[] _barrels = new bool[2];


    protected override void AbsAwake()
    {
        _isAmmoReadyToBeShoot = _barrels[0] || _barrels[1];
    }




    public override void OnShoot()
    {
        //Controll ammo
        if (_barrels[0]) _barrels[0] = false;
        else if (_barrels[1]) _barrels[1] = false;

        _isAmmoReadyToBeShoot = _barrels[0] || _barrels[1];

        //Update UI
        CanvasController.Instance.HudControllers.Ammo.Controllers.DoubleBarrel.UpdateBarrelRounds(_barrels[0], _barrels[1]);
    }
    public override void OnReload()
    {
        //Check if both barrel are loaded
        if (_barrels[0] && _barrels[1]) return;

        //Check if there is ammo in inventory
        PlayerInventory_Ammo playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;
        if (playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex] <= 0) return;

        //Check loaded barrels
        int ammoToReload = 0;
        int loadedBarrelCount = 0;
        if (_barrels[0]) loadedBarrelCount++;
        if (_barrels[1]) loadedBarrelCount++;

        //Calculate ammo
        ammoToReload = 2 - loadedBarrelCount;
        ammoToReload = Mathf.Clamp(ammoToReload, 0, playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);

        //Load barrels
        if (ammoToReload > 0)
            for (int i = 0; i < ammoToReload; i++) _barrels[i] = true;

        _isAmmoReadyToBeShoot = _barrels[0] || _barrels[1];


        //Remove ammo from inventory and update UI
        playerAmmoInventory.RemoveAmmo(_weaponData.AmmoSettings.AmmoType, ammoToReload);
        CanvasController.Instance.HudControllers.Ammo.Controllers.DoubleBarrel.UpdateBarrelRounds(_barrels[0], _barrels[1]);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);
    }




    public override void OnWeaponEquip()
    {
        SetUI();
    }
    public override void OnWeaponUnEquip()
    {
        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.Toggle(false, 0.01f);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Cylinder.Toggle(false, 0.01f);
        CanvasController.Instance.HudControllers.Ammo.Controllers.DoubleBarrel.Toggle(false, 0.01f);
        CanvasController.Instance.HudControllers.Ammo.Toggle(false, 0.01f);
    }


    public void SetUI()
    {
        PlayerInventory_Ammo playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;

        CanvasController.Instance.HudControllers.Ammo.SwitchAmmoHud(HudController_Ammo.AmmoHudType.DoubleBarrel);
        CanvasController.Instance.HudControllers.Ammo.ChangeRoundIcon(_weaponData.AmmoSettings.AmmoType.SingleRoundIcon);
        CanvasController.Instance.HudControllers.Ammo.Controllers.DoubleBarrel.UpdateBarrelRounds(_barrels[0], _barrels[1]);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);

        CanvasController.Instance.HudControllers.Ammo.Controllers.Chamber.Toggle(false, 0.01f);
        CanvasController.Instance.HudControllers.Ammo.Controllers.Cylinder.Toggle(false, 0.01f);
        CanvasController.Instance.HudControllers.Ammo.Controllers.DoubleBarrel.Toggle(true, 0.01f);
        CanvasController.Instance.HudControllers.Ammo.Toggle(true, 0.1f);
    }
}