using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHoldController : WeaponHoldController
{
    private RangeWeaponData _rangeWeaponData;
    private WeaponShootingController _shootingController => (WeaponShootingController)_stateMachine.DamageDealingController;

    protected override void VirtualAwake()
    { }
    private void Start()
    {
        _rangeWeaponData = _stateMachine.DataHolder.WeaponData as RangeWeaponData;
    }

    public override void ChangeHoldMode(HoldModeEnum mode)
    {
        _holdMode = mode;
    }


    public override void RestHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

        _playerCombatController.RightHand.localPosition = Vector3.zero;
        _playerCombatController.RightHand.parent.localRotation = Quaternion.Euler(Vector3.zero);


        _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetRot(_rangeWeaponData.HoldTransforms.Rest.RightHand_Rotation, rotateSpeed);
        _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetPos(_rangeWeaponData.HoldTransforms.Rest.RightHand_Position, moveSpeed).CurrentLerpFinished(() =>
        {
            _stateMachine.DamageDealingController.enabled = false;
            _stateMachine.PlayerStateMachine.AnimatingControllers.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_stateMachine.DataHolder.WeaponData, true);

            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);


            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Lines);

        _playerCombatController.RightHand.localPosition = Vector3.zero;
        _playerCombatController.RightHand.parent.localRotation = Quaternion.Euler(Vector3.zero);


        _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetRot(_rangeWeaponData.HoldTransforms.Hip.RightHand_Rotation, rotateSpeed);
        _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner.SetPos(_rangeWeaponData.HoldTransforms.Hip.RightHand_Position, moveSpeed).CurrentLerpFinished(() =>
        {
            _stateMachine.DamageDealingController.enabled = true;
            _stateMachine.PlayerStateMachine.AnimatingControllers.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_stateMachine.DataHolder.WeaponData, false);

            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);


            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
}