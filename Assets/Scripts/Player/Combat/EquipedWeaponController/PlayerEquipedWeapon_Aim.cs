using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipedWeapon_Aim : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerEquipedWeaponController _equipedWeaponController;
    private PlayerCombatController _combatController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isAim; public bool IsAim { get { return _isAim; } }
    [SerializeField] bool _isInput; public bool IsInput { get { return _isInput; } set { _isInput = value; } }
    [SerializeField] int _aimTypeIndex;
    [SerializeField] AimTypeEnum _aimType;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] AnimationCurve _curve;


    public enum AimTypeEnum
    {
        ADS, Left
    }

    private Action[] _aimMethods = new Action[2];




    private void Awake()
    {
        _combatController = _equipedWeaponController.PlayerStateMachine.CombatControllers.Combat;
        _aimMethods[0] = AimDisable;
        _aimMethods[1] = AimEnable;
    }





    public void Aim(bool aim)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) || _equipedWeaponController.Wall.IsWall) return;
        if (_combatController.EquipedWeaponData.WeaponTransforms.Aim.Length <= 0) return;

        ToggleAimBool(aim);
        int enableADS = _isAim ? 1 : 0;

        _aimMethods[enableADS]();
    }
    public void ToggleAimBool(bool aim)
    {
        _isAim = aim;
    }


    private void AimEnable()
    {
        _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(true);
        _combatController.EquipedWeapon.DamageDealingController.Toggle(true);
        _equipedWeaponController.Block.ToggleBlockBool(false);
        _equipedWeaponController.Run.ToggleRunBool(false);

        CheckCrosshair();

        MoveHandsToAimTransform();
    }
    private void AimDisable()
    {
        _combatController.EquipedWeapon.OnWeaponAim(false);

        WeaponHoldController equipedModeController = _combatController.EquipedWeapon.HoldController;
        equipedModeController.MoveHandsToCurrentHoldMode(0.2f, 0.2f);
    }


    public void ResetAimType(int index)
    {
        _aimTypeIndex = index;
        _aimType = (AimTypeEnum)_aimTypeIndex;

    }
    public void ChangeAimType()
    {
        if (!_isAim) return;
        if (_combatController.EquipedWeaponData.WeaponTransforms.Aim.Length <= 1) return;


        _aimTypeIndex++;
        _aimTypeIndex = _aimTypeIndex >= _combatController.EquipedWeaponData.WeaponTransforms.Aim.Length ? 0 : _aimTypeIndex;
        _aimType = (AimTypeEnum)_aimTypeIndex;
        _combatController.EquipedWeapon.AimIndexHolder.WeaponAimIndex = _aimTypeIndex;

        CheckCrosshair();

        MoveHandsToAimTransform();
    }


    private void MoveHandsToAimTransform()
    {
        WeaponMainPositioner mainPositioner = _equipedWeaponController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner;
        mainPositioner.Rotate(_combatController.EquipedWeaponData.WeaponTransforms.Aim[_aimTypeIndex].RightHand_Rotation, 0.2f);
        mainPositioner.Move(_combatController.EquipedWeaponData.WeaponTransforms.Aim[_aimTypeIndex].RightHand_Position, 0.2f, _curve);

        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetPos(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Position, 6);
        _combatController.PlayerStateMachine.AnimatingControllers.LeftHand.SetRot(_combatController.EquipedWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 6);

        if (_aimTypeIndex == 0) _combatController.EquipedWeapon.OnWeaponAim(true);
        else _combatController.EquipedWeapon.OnWeaponAim(false);
    }

    private void CheckCrosshair()
    {
        if (_aimType == AimTypeEnum.Left) CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.Dot);
        else CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.None);
    }
}
