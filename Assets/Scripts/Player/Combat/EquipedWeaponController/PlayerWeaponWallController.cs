using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponWallController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isWall; public bool IsWall { get { return _isWall; } }


    private Action[] _wallMethods = new Action[2];


    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
        _wallMethods[0] = WallDisable;
        _wallMethods[1] = WallEnable;
    }



    public void ToggleWall(bool enable)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        int index = enable ? 1 : 0;

        ToggleWallBool(enable);

        _wallMethods[index]();
    }
    public void ToggleWallBool(bool enable) { _isWall = enable; }




    private void WallEnable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = false;
        _equipedWeaponController.Aim.ToggleAimBool(false);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        _equipedWeaponController.Run.ToggleRunWeaponLockBool(false);

        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetPos(_combatController.EquipedWeaponData.WeaponTransforms.Wall.RightHand_Position, 6);
        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetRot(_combatController.EquipedWeaponData.WeaponTransforms.Wall.RightHand_Rotation, 6);

        _combatController.PlayerStateMachine.AnimatingControllers.IkController.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, true);
    }
    private void WallDisable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;

        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.HoldController;
        equipedModeController.MoveHandsToCurrentHoldMode(6, 6);
    }
}
