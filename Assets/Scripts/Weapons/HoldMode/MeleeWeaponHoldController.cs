using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHoldController : WeaponHoldController
{
    private MeleeWeaponData _meleeWeaponData;


    protected override void VirtualAwake()
    {
        _holdMode = HoldModeEnum.Hip;
        _meleeWeaponData = (MeleeWeaponData)_stateMachine.DataHolder.WeaponData;
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
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(CrosshairController.CrosshairTypeEnum.Lines);

        WeaponMainPositioner mainPositioner = _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainPositioner;
        mainPositioner.Rotate(_meleeWeaponData.Hip.RightHand_Rotation, 1);
        mainPositioner.Move(_meleeWeaponData.Hip.RightHand_Position, 1).SetOnMoveFinish(() =>
        {
            _stateMachine.DamageDealingController.Toggle(true);

            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);

            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
        });
    }
}
