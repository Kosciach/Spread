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
        _combatController = _equipedWeaponController.PlayerStateMachine.CombatControllers.Combat;
    }




    public void ChangeEquipedHoldMode()
    {
        if (_equipedWeaponController.Run.IsRun) return;
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)
            || _equipedWeaponController.Aim.IsAim
            || _equipedWeaponController.Block.IsBlock
            || _equipedWeaponController.Wall.IsWall) return;

        WeaponHoldController equipedWeaponHoldController = _combatController.EquipedWeapon.HoldController;
        WeaponHoldController.HoldModeEnum equipedMode = equipedWeaponHoldController.IsHoldMode(WeaponHoldController.HoldModeEnum.Hip) ? WeaponHoldController.HoldModeEnum.Rest : WeaponHoldController.HoldModeEnum.Hip;

        equipedWeaponHoldController.ChangeHoldMode(equipedMode);
        equipedWeaponHoldController.MoveHandsToCurrentHoldMode(0.3f, 0.3f);
    }
}
