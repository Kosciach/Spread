using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoController : MonoBehaviour
{
    private WeaponStateMachine _stateMachine;


    [Header("====Debugs====")]
    [SerializeField] bool _isRoundInChamber; public bool IsRoundInChamber { get { return _isRoundInChamber; } }
    [SerializeField] int _ammoInMag; public int AmmoInMag { get { return _ammoInMag; } }
    [SerializeField] RangeWeaponData _weaponData;
    [SerializeField] AnimatorOverrideController _reloadAnimOveride;

    private Action<int, PlayerAmmoInventory>[] _reloadMethods = new Action<int, PlayerAmmoInventory>[2];




    private void Awake()
    {
        _stateMachine = GetComponent<WeaponStateMachine>();
        _reloadMethods[0] = ReloadWithoutRoundInChamber;
        _reloadMethods[1] = ReloadWithRoundInChamber;
    }







    public void OnShoot()
    {
        _ammoInMag--;
        CheckChamber();

        _ammoInMag = Mathf.Clamp(_ammoInMag, 0, _ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
    }

    private void CheckChamber()
    {
        _isRoundInChamber = _ammoInMag > -1;
        CanvasController.Instance.HudControllers.Ammo.UpdateRoundInChamber(_isRoundInChamber);
    }






    public void Reload()
    {
        _stateMachine.PlayerStateMachine.AnimatingControllers.Reload.Reload(_reloadAnimOveride);


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
        _reloadMethods[reloadMethodIndex](ammoToReload, playerAmmoInventory);

        playerAmmoInventory.RemoveAmmo(_weaponData.AmmoSettings.AmmoType, ammoToReload);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);
        CanvasController.Instance.HudControllers.Ammo.UpdateRoundInChamber(_isRoundInChamber);
    }



    private void ReloadWithRoundInChamber(int ammoToReload, PlayerAmmoInventory playerAmmoInventory)
    {
        //Put ammo in mag
        _ammoInMag += ammoToReload;
    }
    private void ReloadWithoutRoundInChamber(int ammoToReload, PlayerAmmoInventory playerAmmoInventory)
    {
        //Put ammo in mag and place one in the chamber
        _ammoInMag += (ammoToReload-1);
        _isRoundInChamber = true;
    }




    private void OnEnable()
    {
        PlayerAmmoInventory playerAmmoInventory = _stateMachine.PlayerStateMachine.InventoryControllers.Inventory.Ammo;
        int ammoTypeIndex = (int)_weaponData.AmmoSettings.AmmoType.AmmoType;

        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInMag(_ammoInMag);
        CanvasController.Instance.HudControllers.Ammo.UpdateAmmoInInventory(playerAmmoInventory.AmmoTypesAmmount[ammoTypeIndex]);
        CanvasController.Instance.HudControllers.Ammo.UpdateRoundInChamber(_isRoundInChamber);
    }
}
