using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isBlock; public bool IsBlock { get { return _isBlock; } }
    [SerializeField] bool _isInput; public bool IsInput { get { return _isInput; } set { _isInput = value; } }


    private Action[] _blockMethods = new Action[2];




    private void Awake()
    {
        _combatController = _equipedWeaponController.CombatController;
        _blockMethods[0] = BlockDisable;
        _blockMethods[1] = BlockEnable;
    }





    public void Block(bool block)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _equipedWeaponController.Aim.IsAim || _equipedWeaponController.Wall.IsWall) return;



        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(!block);

        ToggleBlockBool(block);
        int enableBlock = _isBlock ? 1 : 0;

        _blockMethods[enableBlock]();
    }
    public void ToggleBlockBool(bool block)
    {
        _isBlock = block;
    }



    private void BlockEnable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = false;
        _equipedWeaponController.Run.ToggleRunWeaponLockBool(false);

        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetPos(_combatController.EquipedWeaponData.WeaponTransforms.Block.RightHand_Position, 6);
        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetRot(_combatController.EquipedWeaponData.WeaponTransforms.Block.RightHand_Rotation, 6);
    }
    private void BlockDisable()
    {
        _combatController.EquipedWeapon.DamageDealingController.enabled = true;

        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.HoldController;
        equipedModeController.MoveHandsToCurrentHoldMode(6, 6);
    }
}
