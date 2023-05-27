using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponRunController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _weaponLock; public bool WeaponLock { get { return _weaponLock; } }



    private Action[] _runMethods = new Action[2];
    private Action _transitionFromRun;


    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
        _runMethods[0] = DisableRun;
        _runMethods[1] = EnableRun;
    }



    public void ToggleRunWeaponLock(bool enable)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _equipedWeaponController.Aim.IsAim || _equipedWeaponController.Block.IsBlock) return;

        int index = enable ? 1 : 0;
        ToggleRunWeaponLockBool(enable);

        _runMethods[index]();
    }
    public void ToggleRunWeaponLockBool(bool enable)
    {
        _weaponLock = enable;
    }


    private void EnableRun()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = false;
        _combatController.PlayerStateMachine.AnimatingControllers.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, true);


        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetPos(_combatController.EquipedWeaponData.WeaponTransforms.Run.RightHand_Position, 6);
        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetRot(_combatController.EquipedWeaponData.WeaponTransforms.Run.RightHand_Rotation, 6);

        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetPos(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Position, 6);
        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetRot(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 6);
    }
    private void DisableRun()
    {
        if (_equipedWeaponController.Wall.IsWall) _transitionFromRun = TransitionToWall;
        else _transitionFromRun = TransitionNormally;

        _transitionFromRun();
    }



    private void TransitionToWall()
    {
        _equipedWeaponController.Wall.Wall(true);
    }
    private void TransitionNormally()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;
        _combatController.PlayerStateMachine.AnimatingControllers.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_combatController.EquipedWeaponData, false);


        WeaponHoldController equipedWeaponHoldController = _combatController.EquipedWeapon.HoldController;

        equipedWeaponHoldController.ChangeHoldMode(_combatController.EquipedWeapon.HoldController.HoldMode);
        equipedWeaponHoldController.MoveHandsToCurrentHoldMode(5, 5);
    }
}
