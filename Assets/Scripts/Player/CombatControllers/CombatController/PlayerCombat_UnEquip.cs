using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IkLayers;
using WeaponAnimatorNamespace;
using PlayerAnimator;
using SimpleMan.CoroutineExtensions;

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
;
        PlayUnEquipAnimation();
        EnableBakedLayer(0.4f);

        this.Delay(0.4f, () =>
        {
            DisableWeaponControllers();
            SetDotCrosshair();
            ResetIksTransform();
        });
    }

    private void EnableBakedLayer(float unEquipSmoothTime)
    {
        PlayerIkLayerController playerIkLayerController = _combatController.PlayerStateMachine.AnimatingControllers.IkLayers;
        playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, true, unEquipSmoothTime);
    }
    private void PlayUnEquipAnimation()
    {
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.SetBool("UnEquipWeapon", true);
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
    private void ResetIksTransform()
    {
        WeaponAnimator_MainTransformer mainTransformer = _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainTransformer.MoveRaw(new Vector3(0, -1, 0));
        mainTransformer.RotateRaw(Vector3.zero);
    }




    public void OnUnEquipAnimationEnd()
    {
        ResetUnEquipAnimBool();
        HolsterWeapon();

        if (_combatController.Swap)
        {
            Swap(); return;
        }

        ClearWeaponSlot();
        ToggleLayers();
    }

    private void ResetUnEquipAnimBool()
    {
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.SetBool("UnEquipWeapon", false);
    }
    private void HolsterWeapon()
    {
        _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.HolsterWeapon(_combatController.EquipedWeaponSlot.Weapon, _combatController.EquipedWeaponSlot.WeaponData);
    }
    private void Swap()
    {
        _combatController.SetState(PlayerCombatController.CombatStateEnum.Unarmed);
        _combatController.Equip.ReEquip(_combatController.ChoosenWeaponIndex);
    }
    private void ClearWeaponSlot()
    {
        _combatController.OnWeaponUnEquip();
        _combatController.EquipedWeaponSlot = null;
    }
    private void ToggleLayers()
    {
        PlayerAnimatorController playerAnimatorController = _combatController.PlayerStateMachine.AnimatingControllers.Animator;
        playerAnimatorController.ToggleLayer(LayersEnum.CombatBase, false, 0.4f);
        playerAnimatorController.ToggleLayer(LayersEnum.CombatAnimating, false, 0.4f);

        PlayerIkLayerController playerIkLayerController = _combatController.PlayerStateMachine.AnimatingControllers.IkLayers;
        playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, false, 0.1f);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, false, 0.1f);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, false, 0.1f);
        playerIkLayerController.ToggleLayer(LayerEnum.TriggerDiscipline, false, 0.1f);
        playerIkLayerController.ToggleLayer(LayerEnum.SpineLock, true, 0.4f);
        playerIkLayerController.ToggleLayer(LayerEnum.Body, true, 0.4f);
        playerIkLayerController.ToggleLayer(LayerEnum.Head, true, 0.4f);
        playerIkLayerController.ToggleLayer(LayerEnum.RangeCombat, false, 0.4f);
        playerIkLayerController.OnLerpFinish(LayerEnum.RangeCombat, () =>
        {
            _combatController.PlayerStateMachine.CameraControllers.Hands.Enable.ToggleHandsCamera(false);
            _combatController.SetState(PlayerCombatController.CombatStateEnum.Unarmed);
        });
    }
}
