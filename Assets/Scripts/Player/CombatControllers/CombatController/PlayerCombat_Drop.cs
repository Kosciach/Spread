using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IkLayers;
using WeaponAnimatorNamespace;
using PlayerAnimator;

public class PlayerCombat_Drop : MonoBehaviour
{
    private PlayerCombatController _combatController;




    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();
    }



    public void StartDrop()
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)
        || !_combatController.PlayerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        Drop();
    }
    private void Drop()
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

        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.FingersRightHand, false, 0.2f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.FingersLeftHand, false, 0.2f);

        _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);

        _combatController.OnWeaponUnEquip();

        _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.DropWeapon(_combatController.EquipedWeaponIndex);
        _combatController.EquipedWeaponSlot = null;

        _combatController.PlayerStateMachine.AnimatingControllers.Animator.ToggleLayer(LayersEnum.CombatBase, false, 0.2f);
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.ToggleLayer(LayersEnum.CombatAnimating, true, 0.2f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.TriggerDiscipline, false, 0.2f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, true, 0.1f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, true, 0.1f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Head, true, 0.1f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.RangeCombat, false, 0.2f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(LayerEnum.RangeCombat, () =>
        {
            WeaponAnimator_MainTransformer mainTransformer = _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
            mainTransformer.MoveRaw(Vector3.zero);
            mainTransformer.RotateRaw(Vector3.zero);

            _combatController.PlayerStateMachine.CameraControllers.Hands.Enable.ToggleHandsCamera(false);
            _combatController.SetState(PlayerCombatController.CombatStateEnum.Unarmed);
        });

    }
}
