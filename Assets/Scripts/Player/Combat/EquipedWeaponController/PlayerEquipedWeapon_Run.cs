using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipedWeapon_Run : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isRun; public bool IsRun { get { return _isRun; } }
    [SerializeField] bool _isInput; public bool IsInput { get { return _isInput; } set { _isInput = value; } }


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] AnimationCurve _curve;

    private Action[] _runMethods = new Action[2];
    private Action _transitionFromRun;


    private void Awake()
    {
        _combatController = _equipedWeaponController.PlayerStateMachine.CombatControllers.Combat;
        _runMethods[0] = DisableRun;
        _runMethods[1] = EnableRun;
    }



    public void ToggleRun(bool enable)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _equipedWeaponController.Aim.IsAim || _equipedWeaponController.Block.IsBlock) return;

        int index = enable ? 1 : 0;
        ToggleRunBool(enable);

        _runMethods[index]();
    }
    public void ToggleRunBool(bool enable)
    {
        _isRun = enable;
    }


    private void EnableRun()
    {
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.Dot);

        _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);

        _combatController.EquipedWeapon.DamageDealingController.Toggle(false);

        WeaponAnimator_MainTransformer mainPositioner = _equipedWeaponController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(_combatController.EquipedWeaponData.WeaponTransforms.Run.RightHand_Rotation, 0.5f, _curve);
        mainPositioner.Move(_combatController.EquipedWeaponData.WeaponTransforms.Run.RightHand_Position, 0.5f, _curve);

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
        _combatController.EquipedWeapon.DamageDealingController.Toggle(true);


        WeaponHoldController equipedWeaponHoldController = _combatController.EquipedWeapon.HoldController;

        equipedWeaponHoldController.ChangeHoldMode(_combatController.EquipedWeapon.HoldController.HoldMode);
        equipedWeaponHoldController.MoveHandsToCurrentHoldMode(0.3f, 0.3f);
    }
}
