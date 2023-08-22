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
    public void StartTemporaryUnEquip(bool overrideStateValidation)
    {
        if (_combatController.IsState(PlayerCombatController.CombatStateEnum.Unarmed)) return;

        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped) && !overrideStateValidation) return;
        if (!_combatController.PlayerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        TemporaryUnEquip();
    }
    private void TemporaryUnEquip()
    {
        _combatController.SetState(PlayerCombatController.CombatStateEnum.UnEquip);

        _isTemporaryUnEquip = true;

        DisableWeaponControllers();
        SetDotCrosshair();
        HandleUnEquip();
        ToggleLayers();
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
    private void ToggleLayers()
    {
        PlayerAnimatorController playerAnimatorController = _combatController.PlayerStateMachine.AnimatingControllers.Animator;
        playerAnimatorController.ToggleLayer(LayersEnum.CombatBase, false, 0.5f);
        playerAnimatorController.ToggleLayer(LayersEnum.CombatAnimating, false, 0.5f);

        PlayerIkLayerController playerIkLayerController = _combatController.PlayerStateMachine.AnimatingControllers.IkLayers;
        playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, false, 0.5f);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, false, 0.5f);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, false, 0.5f);
        playerIkLayerController.ToggleLayer(LayerEnum.TriggerDiscipline, false, 0.5f);
        playerIkLayerController.ToggleLayer(LayerEnum.SpineLock, true, 0.5f);
        playerIkLayerController.ToggleLayer(LayerEnum.Body, true, 0.5f);
        playerIkLayerController.ToggleLayer(LayerEnum.Head, true, 0.5f);
        playerIkLayerController.ToggleLayer(LayerEnum.RangeCombat, false, 0.5f);
        playerIkLayerController.OnLerpFinish(LayerEnum.RangeCombat, () =>
        {
            ResetIksTransform();

            _combatController.PlayerStateMachine.CameraControllers.Hands.Enable.ToggleHandsCamera(false);
            _combatController.SetState(PlayerCombatController.CombatStateEnum.Unarmed);
        });
    }
    private void ResetIksTransform()
    {
        WeaponAnimator_MainTransformer mainTransformer = _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainTransformer.MoveRaw(new Vector3(0, -1, 0));
        mainTransformer.RotateRaw(Vector3.zero);
    }
}
