using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipedWeaponController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCombatController _playerCombatController;


    [Header("====Debugs====")]
    [SerializeField] bool _aimDS;
    [SerializeField] bool _block;

    public void ChangeEquipedHoldMode()
    {
        if (!_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _aimDS) return;
        if (_playerCombatController.EquipedWeaponData.WeaponHolder != WeaponData.WeaponTypeEnum.Primary && _playerCombatController.EquipedWeaponData.WeaponHolder != WeaponData.WeaponTypeEnum.Secondary) return;
        
        WeaponHoldController equipedModeController = _playerCombatController.EquipedWeapon.EquipedController;
        WeaponHoldController.HoldModeEnum equipedMode = equipedModeController.IsHoldMode(WeaponHoldController.HoldModeEnum.Hip) ? WeaponHoldController.HoldModeEnum.Rest : WeaponHoldController.HoldModeEnum.Hip;

        equipedModeController.ChangeHoldMode(equipedMode);
        equipedModeController.MoveHandsToCurrentHoldMode(0.2f, 0.2f);
    }
    public void ADS(bool aimDS)
    {
        if (!_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        _aimDS = aimDS;
    }
    public void Block()
    {

    }
}
