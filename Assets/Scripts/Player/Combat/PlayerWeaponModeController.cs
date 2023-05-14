using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponModeController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCombatController _playerCombatController;



    public void ChangeEquipedHoldMode()
    {
        if (!_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        WeaponHoldController equipedModeController = _playerCombatController.EquipedWeapon.EquipedController;
        WeaponHoldController.HoldModeEnum equipedMode = equipedModeController.IsHoldMode(WeaponHoldController.HoldModeEnum.Hip) ? WeaponHoldController.HoldModeEnum.Rest : WeaponHoldController.HoldModeEnum.Hip;

        equipedModeController.ChangeHoldMode(equipedMode);
        equipedModeController.MoveHandsToCurrentHoldMode(0.2f, 0.2f);
    }
    public void ADS(bool aimDS)
    {

    }
    public void Block()
    {

    }
}
