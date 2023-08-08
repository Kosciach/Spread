using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using WeaponAnimatorNamespace;

public class PlayerEquipedWeapon_Wall : MonoBehaviour
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
        _combatController = _equipedWeaponController.PlayerStateMachine.CombatControllers.Combat;
        _wallMethods[0] = WallDisable;
        _wallMethods[1] = WallEnable;
    }



    public void Wall(bool enable)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)) return;

        int index = enable ? 1 : 0;

        _wallMethods[index]();
    }
    public void ToggleWallBool(bool enable) { _isWall = enable; }




    private void WallEnable()
    {
        _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);
        _combatController.PlayerStateMachine.AnimatingControllers.Fingers.SetUpAllFingers(_combatController.EquipedWeaponSlot.WeaponData.FingersPreset.Base, 0.2f);
        _combatController.EquipedWeaponSlot.Weapon.DamageDealingController.Toggle(false);
        _equipedWeaponController.Aim.ToggleAimBool(false);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        _equipedWeaponController.Run.ToggleRunBool(false);

        WeaponAnimator_MainTransformer mainPositioner = _equipedWeaponController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(_combatController.EquipedWeaponSlot.WeaponData.WeaponTransforms.Wall.RightHand_Rotation, 0.2f);
        mainPositioner.Move(_combatController.EquipedWeaponSlot.WeaponData.WeaponTransforms.Wall.RightHand_Position, 0.2f);

        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.MainTransformer.Move(_combatController.EquipedWeaponSlot.WeaponData.LeftHandTransforms.Base.LeftHand_Position, 0.2f);
        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.MainTransformer.Rotate(_combatController.EquipedWeaponSlot.WeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 0.2f);
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
        WeaponHoldController equipedModeController = _combatController.EquipedWeaponSlot.Weapon.HoldController;
        equipedModeController.MoveHandsToCurrentHoldMode(0.2f, 0.2f);
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