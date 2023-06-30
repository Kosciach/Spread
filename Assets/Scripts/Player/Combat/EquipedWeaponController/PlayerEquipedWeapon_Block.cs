using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipedWeapon_Block : MonoBehaviour
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
        _combatController = _equipedWeaponController.PlayerStateMachine.CombatControllers.Combat;
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
        _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(true);

        _combatController.EquipedWeapon.DamageDealingController.Toggle(false);
        _equipedWeaponController.Run.ToggleRunBool(false);


        WeaponAnimator_MainTransformer mainPositioner = _equipedWeaponController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(_combatController.EquipedWeaponData.WeaponTransforms.Block.RightHand_Rotation, 0.3f);
        mainPositioner.Move(_combatController.EquipedWeaponData.WeaponTransforms.Block.RightHand_Position, 0.3f);

        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetPos(_combatController.EquipedWeaponData.LeftHandTransforms.Block.LeftHand_Position, 6);
        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetRot(_combatController.EquipedWeaponData.LeftHandTransforms.Block.LeftHand_Rotation, 6);
    }
    private void BlockDisable()
    {
        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetPos(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Position, 8);
        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetRot(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 8);

        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.HoldController;
        equipedModeController.MoveHandsToCurrentHoldMode(0.3f, 0.3f);
    }
}
