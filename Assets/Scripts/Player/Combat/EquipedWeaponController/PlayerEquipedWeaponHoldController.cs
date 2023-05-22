using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipedWeaponHoldController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
    }




    public void ChangeEquipedHoldMode()
    {
        if (_equipedWeaponController.Run.WeaponLock) return;
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _equipedWeaponController.Aim.IsAim || _equipedWeaponController.Block.IsBlock) return;

        WeaponHoldController equipedWeaponHoldController = _combatController.EquipedWeapon.HoldController;
        WeaponHoldController.HoldModeEnum equipedMode = equipedWeaponHoldController.IsHoldMode(WeaponHoldController.HoldModeEnum.Hip) ? WeaponHoldController.HoldModeEnum.Rest : WeaponHoldController.HoldModeEnum.Hip;

        equipedWeaponHoldController.ChangeHoldMode(equipedMode);
        equipedWeaponHoldController.MoveHandsToCurrentHoldMode(0.2f, 0.2f);
    }
}
