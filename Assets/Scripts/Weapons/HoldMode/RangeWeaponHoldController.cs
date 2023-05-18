using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHoldController : WeaponHoldController
{
    private RangeWeaponData _rangeWeaponData;


    protected override void VirtualAwake()
    {
        _rangeWeaponData = GetComponent<WeaponDataHolder>().WeaponData as RangeWeaponData;
    }


    public override void ChangeHoldMode(HoldModeEnum mode)
    {
        _holdMode = mode;
    }


    public override void RestHoldMode(float rotateSpeed, float moveSpeed)
    {
        _playerCombatController.PlayerStateMachine.IkController.WeaponAnimator.Sway.enabled = false;

        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

        LeanTween.rotateLocal(_playerCombatController.RightHand.gameObject, _rangeWeaponData.Rest.RightHand_Rotation, rotateSpeed);
        LeanTween.moveLocal(_playerCombatController.RightHand.parent.gameObject, _rangeWeaponData.Rest.RightHand_Position, moveSpeed).setOnComplete(() =>
        {
            _stateMachine.DamageDealingController.enabled = false;

            _playerCombatController.PlayerStateMachine.IkController.WeaponAnimator.DesiredRightHandPosition = _rangeWeaponData.Rest.RightHand_Position;
            _playerCombatController.PlayerStateMachine.IkController.WeaponAnimator.Sway.enabled = true;

            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        _playerCombatController.PlayerStateMachine.IkController.WeaponAnimator.Sway.enabled = false;

        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Lines);

        LeanTween.rotateLocal(_playerCombatController.RightHand.gameObject, _rangeWeaponData.Hip.RightHand_Rotation, rotateSpeed);
        LeanTween.moveLocal(_playerCombatController.RightHand.parent.gameObject, _rangeWeaponData.Hip.RightHand_Position, moveSpeed).setOnComplete(() =>
        {
            _stateMachine.DamageDealingController.enabled = true;

            _playerCombatController.PlayerStateMachine.IkController.WeaponAnimator.DesiredRightHandPosition = _rangeWeaponData.Hip.RightHand_Position;
            _playerCombatController.PlayerStateMachine.IkController.WeaponAnimator.Sway.enabled = true;

            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
}