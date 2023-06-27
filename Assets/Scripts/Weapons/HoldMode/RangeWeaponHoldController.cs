using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHoldController : WeaponHoldController
{
    private RangeWeaponData _rangeWeaponData;
    private WeaponShootingController _shootingController => (WeaponShootingController)_stateMachine.DamageDealingController;



    protected override void VirtualAwake()
    {
        _rangeWeaponData = (RangeWeaponData)_stateMachine.DataHolder.WeaponData;
    }

    public override void ChangeHoldMode(HoldModeEnum mode)
    {
        _holdMode = mode;
    }



    public override void RestHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);


        Vector3 rot = _rangeWeaponData.HoldTransforms.Rest.RightHand_Rotation;
        Vector3 pos = _rangeWeaponData.HoldTransforms.Rest.RightHand_Position;

        bool wasEquipAndRun = false;
        if (_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equip)
        && _playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsRun)
        {
            wasEquipAndRun = true;
            rot = _rangeWeaponData.WeaponTransforms.Run.RightHand_Rotation;
            pos = _rangeWeaponData.WeaponTransforms.Run.RightHand_Position;
        }


        WeaponMainPositioner mainPositioner = _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner;
        mainPositioner.Rotate(rot, rotateSpeed);
        mainPositioner.Move(pos, moveSpeed).SetOnMoveFinish(() =>
        {
            _stateMachine.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);

            _stateMachine.DamageDealingController.Toggle(false);

            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);


            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);

            if (_playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsInput) return;
            if (wasEquipAndRun) RestHoldMode(rotateSpeed, moveSpeed);
        });
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Lines);


        Vector3 rot = _rangeWeaponData.HoldTransforms.Hip.RightHand_Rotation;
        Vector3 pos = _rangeWeaponData.HoldTransforms.Hip.RightHand_Position;

        bool wasEquipAndRun = false;
        if (_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equip)
        && _playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsRun)
        {
            wasEquipAndRun = true;
            rot = _rangeWeaponData.WeaponTransforms.Run.RightHand_Rotation;
            pos = _rangeWeaponData.WeaponTransforms.Run.RightHand_Position;
        }


        WeaponMainPositioner mainPositioner = _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner;
        mainPositioner.Rotate(rot, rotateSpeed);
        mainPositioner.Move(pos, moveSpeed).SetOnMoveFinish(() =>
        {
            _stateMachine.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(true);

            _stateMachine.DamageDealingController.Toggle(true);

            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);


            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);

            if (_playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsInput) return;
            if (wasEquipAndRun) HipHoldMode(rotateSpeed, moveSpeed);
        });
    }
}