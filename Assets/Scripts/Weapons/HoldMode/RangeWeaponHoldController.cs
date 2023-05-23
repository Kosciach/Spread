using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHoldController : WeaponHoldController
{
    private RangeWeaponData _rangeWeaponData;


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
        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Dot);

        _playerCombatController.RightHand.localPosition = Vector3.zero;
        _playerCombatController.RightHand.parent.localRotation = Quaternion.Euler(Vector3.zero);

        LeanTween.cancel(_playerCombatController.RightHand.gameObject); 
        LeanTween.rotateLocal(_playerCombatController.RightHand.gameObject, _rangeWeaponData.Rest.RightHand_Rotation, rotateSpeed);
        LeanTween.moveLocal(_playerCombatController.RightHand.parent.gameObject, _rangeWeaponData.Rest.RightHand_Position, moveSpeed).setOnComplete(() =>
        {
            _stateMachine.DamageDealingController.enabled = false;
            _stateMachine.PlayerStateMachine.IkController.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_stateMachine.DataHolder.WeaponData, true);

            _playerCombatController.PlayerStateMachine.WeaponAnimator.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.WeaponAnimator.Sway.Toggle(true);

            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Lines);

        _playerCombatController.RightHand.localPosition = Vector3.zero;
        _playerCombatController.RightHand.parent.localRotation = Quaternion.Euler(Vector3.zero);

        LeanTween.cancel(_playerCombatController.RightHand.gameObject);
        LeanTween.rotateLocal(_playerCombatController.RightHand.gameObject, _rangeWeaponData.Hip.RightHand_Rotation, rotateSpeed);
        LeanTween.moveLocal(_playerCombatController.RightHand.parent.gameObject, _rangeWeaponData.Hip.RightHand_Position, moveSpeed).setOnComplete(() =>
        {
            _stateMachine.DamageDealingController.enabled = true;
            _stateMachine.PlayerStateMachine.IkController.Fingers.TriggerDiscipline.SwitchTriggerDiscipline(_stateMachine.DataHolder.WeaponData, false);

            _playerCombatController.PlayerStateMachine.WeaponAnimator.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.WeaponAnimator.Sway.Toggle(true);

            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
}