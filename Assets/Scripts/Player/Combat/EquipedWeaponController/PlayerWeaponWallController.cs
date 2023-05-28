using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    private Action _transitionFromWall;


    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
        _wallMethods[0] = WallDisable;
        _wallMethods[1] = WallEnable;
    }



    public void Wall(bool enable)
    {
        if (_combatController.PlayerStateMachine.AnimatingControllers.Reload.IsReloading) return;
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        int index = enable ? 1 : 0;

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

        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetPos(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Position, 6);
        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetRot(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 6);

        _combatController.PlayerStateMachine.AnimatingControllers.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, true);
    }
    private void WallDisable()
    {
        if (_equipedWeaponController.Block.IsInput)
        {
            if (_equipedWeaponController.Aim.IsInput) _transitionFromWall = TransitionToAim;
            else _transitionFromWall = TransitionToBlock;
        }
        else
        {
            if (_equipedWeaponController.Aim.IsInput) _transitionFromWall = TransitionToAim;
            else _transitionFromWall = TransitionToHold;
        }

        _transitionFromWall();
    }



    private void TransitionToHold()
    {
        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.HoldController;
        equipedModeController.MoveHandsToCurrentHoldMode(6, 6);
    }
    private void TransitionToBlock()
    {
        _isWall = false;
        _equipedWeaponController.Block.Block(true);
    }
    private void TransitionToAim()
    {
        _isWall = false;
        _equipedWeaponController.Aim.Aim(true);
    }

}
