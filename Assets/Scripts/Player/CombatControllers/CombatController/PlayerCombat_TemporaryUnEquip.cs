using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IkLayers;
using WeaponAnimatorNamespace;
using PlayerAnimator;

public class PlayerCombat_TemporaryUnEquip : MonoBehaviour
{
    private PlayerCombatController _combatController;

    [SerializeField] bool _isTemporaryUnEquip; public bool IsTemporaryUnEquip { get { return _isTemporaryUnEquip; } set { _isTemporaryUnEquip = value; } }




    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();
    }




    public void RecoverFromTemporaryUnEquip()
    {
        if (!_isTemporaryUnEquip) return;

        _isTemporaryUnEquip = false;
        _combatController.Equip.StartEquip(_combatController.EquipedWeaponIndex);
    }
    public void StartTemporaryUnEquip(bool overrideStateValidation, float unEquipDuration)
    {
        if (_combatController.IsState(PlayerCombatController.CombatStateEnum.Unarmed)) return;

        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) && !overrideStateValidation) return;
        if (!_combatController.PlayerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        TemporaryUnEquip(unEquipDuration);
    }
    private void TemporaryUnEquip(float unEquipDuration)
    {
        _combatController.SetState(PlayerCombatController.CombatStateEnum.UnEquip);

        _isTemporaryUnEquip = true;

        DisableWeaponControllers();
        SetDotCrosshair();
        HandleUnEquip();
        ToggleLayers(unEquipDuration);
    }


    private void DisableWeaponControllers()
    {
        _combatController.EquipedWeaponSlot.Weapon.DamageDealingController.Toggle(false);

        _combatController.PlayerStateMachine.CombatControllers.WallDetector.ToggleCollider(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.ToggleAimBool(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Block.ToggleBlockBool(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Run.ToggleRunBool(false);
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Wall.ToggleWallBool(false);

        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(false);
        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(false);

        _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);
    }
    private void SetDotCrosshair()
    {
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(HudController_Crosshair.CrosshairTypeEnum.Dot);
    }
    private void HandleUnEquip()
    {
        _combatController.OnWeaponUnEquip();
        _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_combatController.EquipedWeaponSlot.Weapon, _combatController.EquipedWeaponSlot.WeaponData);
    }
    private void ToggleLayers(float unEquipDuration)
    {
        PlayerAnimatorController playerAnimatorController = _combatController.PlayerStateMachine.AnimatingControllers.Animator;
        playerAnimatorController.ToggleLayer(LayersEnum.CombatBase, false, unEquipDuration);
        playerAnimatorController.ToggleLayer(LayersEnum.CombatAnimating, false, unEquipDuration);

        PlayerIkLayerController playerIkLayerController = _combatController.PlayerStateMachine.AnimatingControllers.IkLayers;
        playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, false, unEquipDuration);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, false, unEquipDuration);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, false, unEquipDuration);
        playerIkLayerController.ToggleLayer(LayerEnum.TriggerDiscipline, false, unEquipDuration);
        playerIkLayerController.ToggleLayer(LayerEnum.SpineLock, true, unEquipDuration);
        playerIkLayerController.ToggleLayer(LayerEnum.Body, true, unEquipDuration);
        playerIkLayerController.ToggleLayer(LayerEnum.Head, true, unEquipDuration);
        playerIkLayerController.ToggleLayer(LayerEnum.RangeCombat, false, unEquipDuration);
        playerIkLayerController.OnLerpFinish(LayerEnum.RangeCombat, () =>
        {
            ResetIksTransform();

            _combatController.PlayerStateMachine.CameraControllers.Hands.Enable.ToggleHandsCamera(false);
            _combatController.SetState(PlayerCombatController.CombatStateEnum.UnarmedTemporary);
        });
    }
    private void ResetIksTransform()
    {
        WeaponAnimator_MainTransformer mainTransformer = _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainTransformer.MoveRaw(new Vector3(0, -1, 0));
        mainTransformer.RotateRaw(Vector3.zero);
    }
}
