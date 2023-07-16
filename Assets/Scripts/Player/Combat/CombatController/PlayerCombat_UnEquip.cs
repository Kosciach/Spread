using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IkLayers;
using WeaponAnimatorNamespace;
using PlayerAnimator;

public class PlayerCombat_UnEquip : MonoBehaviour
{
    private PlayerCombatController _combatController;




    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();
    }



    public void StartUnEquip(float unEquipSpeed)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)
        || !_combatController.PlayerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        UnEquip();
    }



    private void UnEquip()
    {
        _combatController.SetState(PlayerCombatController.CombatStateEnum.UnEquip);

        _combatController.EquipedWeaponSlot.Weapon.DamageDealingController.Toggle(false);

        _combatController.PlayerStateMachine.CombatControllers.WallDetector.ToggleCollider(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.ToggleRunBool(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Wall.ToggleWallBool(false);

        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(false);
        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(false);

        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.Dot);

        WeaponAnimator_MainTransformer mainTransformer = _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainTransformer.Rotate(_combatController.EquipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Rotation, 0.3f);
        mainTransformer.Move(_combatController.EquipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Position,  0.5f).SetOnMoveFinish(() =>
        {
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, false, 0.2f);
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, false, 0.2f);

            _combatController.PlayerStateMachine.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.IdleWalkRun, 5);
            _combatController.PlayerStateMachine.CameraControllers.Hands.Move.SetCameraPosition(PlayerHandsCamera_Move.CameraPositionsEnum.Idle, 5);

            _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);

            _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_combatController.EquipedWeaponSlot.Weapon, _combatController.EquipedWeaponSlot.WeaponData);

            if(_combatController.Swap)
            {
                _combatController.SetState(PlayerCombatController.CombatStateEnum.Unarmed);
                _combatController.Equip.ReEquip(_combatController.ChoosenWeaponIndex);
                return;
            }

            _combatController.EquipedWeaponSlot.Weapon.OnWeaponUnEquip();
            _combatController.EquipedWeaponSlot = null;

            _combatController.PlayerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.CombatBase, false, 0.4f);
            _combatController.PlayerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.CombatAnimating, true, 0.4f);
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.TriggerDiscipline, false, 0.2f);
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.SpineLock, true, 0.4f);
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, true, 0.4f);
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Head, true, 0.4f);
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.RangeCombat, false, 0.4f);
            _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(PlayerIkLayerController.LayerEnum.RangeCombat, () =>
            {
                mainTransformer.MoveRaw(Vector3.zero);
                mainTransformer.RotateRaw(Vector3.zero);
                _combatController.SetState(PlayerCombatController.CombatStateEnum.Unarmed);
            });
        });
    }
}
