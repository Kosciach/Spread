using LeftHandAnimatorNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponAnimatorNamespace;

public class RangeWeaponHoldController : WeaponHoldController
{
    private RangeWeaponData _rangeWeaponData;
    private WeaponShootingController _shootingController;



    protected override void VirtualAwake()
    {
        _rangeWeaponData = (RangeWeaponData)_stateMachine.DataHolder.WeaponData;
        _shootingController = (WeaponShootingController)_stateMachine.DamageDealingController;
    }

    public override void ChangeHoldMode(HoldModeEnum mode)
    {
        _holdMode = mode;
    }



    public override void RestHoldMode(float rotateSpeed, float moveSpeed)
    {
        PlayerCombatController playerCombatController = _stateMachine.PlayerStateMachine.CombatControllers.Combat;
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(_rangeWeaponData.CrosshairSetting.CrosshairType);


        LeftHandAnimator leftHandAnimator = playerCombatController.PlayerStateMachine.AnimatingControllers.LeftHand;
        leftHandAnimator.MainTransformer.Move(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Position, 0.2f);
        leftHandAnimator.MainTransformer.Rotate(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 0.2f);

        PlayerFingerAnimator fingerAnimator = playerCombatController.PlayerStateMachine.AnimatingControllers.Fingers;
        fingerAnimator.SetUpAllFingers(_rangeWeaponData.FingersPreset.Base, 0.2f);

        WeaponAnimator_MainTransformer mainPositioner = playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(_rangeWeaponData.HoldTransforms.Rest.RightHand_Rotation, rotateSpeed);
        mainPositioner.Move(_rangeWeaponData.HoldTransforms.Rest.RightHand_Position, moveSpeed).SetOnMoveFinish(() =>
        {
            _stateMachine.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);
            playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);
            _stateMachine.DamageDealingController.Toggle(false);
        });
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        PlayerCombatController playerCombatController = _stateMachine.PlayerStateMachine.CombatControllers.Combat;
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(_rangeWeaponData.CrosshairSetting.CrosshairType);


        LeftHandAnimator leftHandAnimator = playerCombatController.PlayerStateMachine.AnimatingControllers.LeftHand;
        leftHandAnimator.MainTransformer.Move(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Position, 0.2f);
        leftHandAnimator.MainTransformer.Rotate(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 0.2f);

        PlayerFingerAnimator fingerAnimator = playerCombatController.PlayerStateMachine.AnimatingControllers.Fingers;
        fingerAnimator.SetUpAllFingers(_rangeWeaponData.FingersPreset.Base, 0.2f);

        WeaponAnimator_MainTransformer mainPositioner = playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(_rangeWeaponData.HoldTransforms.Hip.RightHand_Rotation, rotateSpeed);
        mainPositioner.Move(_rangeWeaponData.HoldTransforms.Hip.RightHand_Position, moveSpeed).SetOnMoveFinish(() =>
        {
            _stateMachine.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(true);
            playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);
            _stateMachine.DamageDealingController.Toggle(true);
        });
    }
}