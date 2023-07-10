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
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.Dot);


        Vector3 rot = _rangeWeaponData.HoldTransforms.Rest.RightHand_Rotation;
        Vector3 pos = _rangeWeaponData.HoldTransforms.Rest.RightHand_Position;

        if (_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equip)
        && _playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsRun)
        {
            rot = _rangeWeaponData.WeaponTransforms.Run.RightHand_Rotation;
            pos = _rangeWeaponData.WeaponTransforms.Run.RightHand_Position;
        }



        PlayerFingerAnimator fingerAnimator = _playerCombatController.PlayerStateMachine.AnimatingControllers.Fingers;
        fingerAnimator.SetUpAllFingers(_rangeWeaponData.FingersPreset.Base, 0.3f);

        LeftHandAnimator leftHandAnimator = _playerCombatController.PlayerStateMachine.AnimatingControllers.LeftHand;
        leftHandAnimator.Move(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Position, 0.2f);
        leftHandAnimator.Rotate(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 0.2f);

        WeaponAnimator_MainTransformer mainPositioner = _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(rot, rotateSpeed);
        mainPositioner.Move(pos, moveSpeed).SetOnMoveFinish(() =>
        {
            _stateMachine.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);

            _stateMachine.DamageDealingController.Toggle(false);

            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);


            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);

            if (_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)
            && _playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsRun)
            {
                mainPositioner.Rotate(_rangeWeaponData.WeaponTransforms.Run.RightHand_Rotation, rotateSpeed);
                mainPositioner.Move(_rangeWeaponData.WeaponTransforms.Run.RightHand_Position, moveSpeed);
                _stateMachine.DamageDealingController.Toggle(false);
            }
        });
    }
    public override void HipHoldMode(float rotateSpeed, float moveSpeed)
    {
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(_rangeWeaponData.CrosshairSetting.CrosshairType);


        Vector3 rot = _rangeWeaponData.HoldTransforms.Hip.RightHand_Rotation;
        Vector3 pos = _rangeWeaponData.HoldTransforms.Hip.RightHand_Position;

        if (_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equip)
        && _playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsRun)
        {
            rot = _rangeWeaponData.WeaponTransforms.Run.RightHand_Rotation;
            pos = _rangeWeaponData.WeaponTransforms.Run.RightHand_Position;
        }




        LeftHandAnimator leftHandAnimator = _playerCombatController.PlayerStateMachine.AnimatingControllers.LeftHand;
        leftHandAnimator.Move(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Position, 0.2f);
        leftHandAnimator.Rotate(_rangeWeaponData.LeftHandTransforms.Base.LeftHand_Rotation, 0.2f);

        PlayerFingerAnimator fingerAnimator = _playerCombatController.PlayerStateMachine.AnimatingControllers.Fingers;
        fingerAnimator.SetUpAllFingers(_rangeWeaponData.FingersPreset.Base, 0.2f);

        WeaponAnimator_MainTransformer mainPositioner = _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainPositioner.Rotate(rot, rotateSpeed);
        mainPositioner.Move(pos, moveSpeed).SetOnMoveFinish(() =>
        {
            _stateMachine.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(true);

            _stateMachine.DamageDealingController.Toggle(true);

            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
            _playerCombatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);


            _playerCombatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);

            if (_playerCombatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)
            && _playerCombatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.IsRun)
            {
                mainPositioner.Rotate(_rangeWeaponData.WeaponTransforms.Run.RightHand_Rotation, rotateSpeed);
                mainPositioner.Move(_rangeWeaponData.WeaponTransforms.Run.RightHand_Position, moveSpeed);
                _stateMachine.DamageDealingController.Toggle(false);
            }
        });
    }
}