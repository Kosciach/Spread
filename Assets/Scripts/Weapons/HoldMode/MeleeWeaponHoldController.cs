using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHoldController : WeaponHoldController
{
    private MeleeWeaponData _meleeWeaponData;


    protected override void VirtualAwake()
    {
        _holdMode = HoldModeEnum.Hip;
    }
    private void Start()
    {
        _meleeWeaponData = _stateMachine.DataHolder.WeaponData as MeleeWeaponData;
    }

    public override void ChangeHoldMode(HoldModeEnum mode)
    {
        _holdMode = mode;
    }



    public override void RestHoldMode(float rotateSpeed, float moveSpeed)
    {
        _holdMode = HoldModeEnum.Hip;
        HipHoldMode(rotateSpeed, moveSpeed);
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.CrosshairController.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Lines);

        _playerCombatController.PlayerStateMachine.WeaponAnimator.MainPositioner.SetRot(_meleeWeaponData.Hip.RightHand_Rotation, rotateSpeed);
        _playerCombatController.PlayerStateMachine.WeaponAnimator.MainPositioner.SetPos(_meleeWeaponData.Hip.RightHand_Position, moveSpeed).CurrentLerpFinished(() =>
        {
            _stateMachine.DamageDealingController.enabled = true;

            _playerCombatController.PlayerStateMachine.WeaponAnimator.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.WeaponAnimator.Sway.Toggle(true);

            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
}
