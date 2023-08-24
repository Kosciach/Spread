using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IkLayers;
using WeaponAnimatorNamespace;
using PlayerAnimator;
using PlayerHandsCamera;
using LeftHandAnimatorNamespace;
using SimpleMan.CoroutineExtensions;


public class PlayerCombat_Equip : MonoBehaviour
{
    private PlayerCombatController _combatController;



    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();
    }



    public void StartEquip(int choosenWeaponIndex)
    {
        if (!_combatController.IsState(PlayerCombatController.CombatStateEnum.Unarmed) && !_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped)
        || _combatController.PlayerStateMachine.CombatControllers.Throw.IsThrow
        || choosenWeaponIndex >= _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.WeaponInventorySlots.Count
        ||  !_combatController.PlayerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        _combatController.ChoosenWeaponIndex = choosenWeaponIndex;
        WeaponInventorySlot choosenWeaponInventorySlot = _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.WeaponInventorySlots[choosenWeaponIndex];
        if (choosenWeaponInventorySlot.Empty)
        {
            _combatController.EquipedWeaponSlot = null;
            return;
        }

        if (_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped))
        {
            if(choosenWeaponIndex != _combatController.EquipedWeaponIndex) Swap();
            return;
        }


        Equip(choosenWeaponIndex, choosenWeaponInventorySlot);
    }




    private void Equip(int choosenWeaponIndex, WeaponInventorySlot choosenWeaponInventorySlot)
    {
        _combatController.SetState(PlayerCombatController.CombatStateEnum.Equip);

        SetWeaponSlotAndIndex(choosenWeaponIndex, choosenWeaponInventorySlot);
        LoadAimType();
        OverrideAnimator();
        _combatController.EquipedWeaponSlot.Weapon.SwitchController.SwitchTo.Equiped();
        PrepareHandsCamera();
        SetAllIks();
        ToggleLayers();

        PlayAnimation();
        this.Delay(0.5f, PutWeaponIntoRightHand);
    }

    private void SetWeaponSlotAndIndex(int choosenWeaponIndex, WeaponInventorySlot choosenWeaponInventorySlot)
    {
        _combatController.EquipedWeaponSlot = choosenWeaponInventorySlot;
        _combatController.EquipedWeaponIndex = choosenWeaponIndex;
    }
    private void LoadAimType()
    {
        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.LoadAimTypeFromWeapon(_combatController.EquipedWeaponSlot.Weapon);
    }
    private void OverrideAnimator()
    {
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.OverrideAnimator(_combatController.EquipedWeaponSlot.Weapon.AnimatorOverride);
    }
    private void PrepareHandsCamera()
    {
        _combatController.PlayerStateMachine.CameraControllers.Hands.Enable.ToggleHandsCamera(true);
        _combatController.PlayerStateMachine.CameraControllers.Hands.Move.ChangePreset(PositionsPresetsLabels.Combat, 0.2f);
        _combatController.PlayerStateMachine.CameraControllers.Hands.Rotate.ChangePreset(RotationPresetsLabels.Combat, 0.2f);
    }
    private void SetAllIks()
    {
        RangeWeaponData weaponData = (RangeWeaponData)_combatController.EquipedWeaponSlot.WeaponData;

        LeftHandAnimator leftHandAnimator = _combatController.PlayerStateMachine.AnimatingControllers.LeftHand;
        leftHandAnimator.MainTransformer.MoveRaw(weaponData.LeftHandTransforms.Base.LeftHand_Position);
        leftHandAnimator.MainTransformer.RotateRaw(weaponData.LeftHandTransforms.Base.LeftHand_Rotation);

        PlayerFingerAnimator fingerAnimator = _combatController.PlayerStateMachine.AnimatingControllers.Fingers;
        fingerAnimator.SetUpAllFingers(weaponData.FingersPreset.Base, 0.01f);
        fingerAnimator.Discipline.SetDisciplineIk(_combatController.EquipedWeaponSlot.WeaponData.FingersPreset);


        bool isHipMode = _combatController.EquipedWeaponSlot.Weapon.HoldController.HoldMode == WeaponHoldController.HoldModeEnum.Hip;
        Vector3 mainTransformerRotation = isHipMode ? weaponData.HoldTransforms.Hip.RightHand_Rotation : weaponData.HoldTransforms.Rest.RightHand_Rotation;
        Vector3 mainTransformerPosition = isHipMode ? weaponData.HoldTransforms.Hip.RightHand_Position : weaponData.HoldTransforms.Rest.RightHand_Position;
        WeaponAnimator_MainTransformer mainTransformer = _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainTransformer.RotateRaw(mainTransformerRotation);
        mainTransformer.MoveRaw(mainTransformerPosition);
    }
    private void ToggleLayers()
    {
        PlayerAnimatorController playerAnimatorController = _combatController.PlayerStateMachine.AnimatingControllers.Animator;
        playerAnimatorController.ToggleLayer(LayersEnum.CombatBase, true, 0.4f);
        playerAnimatorController.ToggleLayer(LayersEnum.CombatAnimating, true, 0.4f);

        PlayerIkLayerController playerIkLayerController = _combatController.PlayerStateMachine.AnimatingControllers.IkLayers;
        playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, true, 0.1f);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, true, 0.1f);
        playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, true, 0.1f);
        playerIkLayerController.ToggleLayer(LayerEnum.SpineLock, false, 0.4f);
        playerIkLayerController.ToggleLayer(LayerEnum.Body, false, 0.4f);
        playerIkLayerController.ToggleLayer(LayerEnum.Head, false, 0.4f);
        playerIkLayerController.ToggleLayer(LayerEnum.RangeCombat, true, 0.4f);
    }
    private void PlayAnimation()
    {
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.SetInt("WeaponHoldType", (int)_combatController.EquipedWeaponSlot.Weapon.HoldController.HoldMode);
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.SetBool("EquipWeapon", true);
    }
    private void PutWeaponIntoRightHand()
    {
        Transform equipedWeaponTransform = _combatController.EquipedWeaponSlot.Weapon.transform;
        WeaponData equipedWeaponData = _combatController.EquipedWeaponSlot.WeaponData;
        _combatController.PlayerStateMachine.AnimatingControllers.WeaponHolder.RightHand(equipedWeaponTransform);
        _combatController.PlayerStateMachine.AnimatingControllers.WeaponHolder.SetWeaponInHandTransform(equipedWeaponTransform, equipedWeaponData.InHandPosition, equipedWeaponData.InHandRotation);
    }



    public void OnEquipAnimationEnd()
    {
        SwitchCrosshair();
        ToggleWeaponControllers();
        DisableBakedLayer();
        ResetEquipAnimBool();
        _combatController.OnWeaponEquip();
        _combatController.SetState(PlayerCombatController.CombatStateEnum.Equiped);
    }

    private void SwitchCrosshair()
    {
        RangeWeaponData weaponData = (RangeWeaponData)_combatController.EquipedWeaponSlot.WeaponData;
        CanvasController.Instance.HudControllers.Crosshair.SwitchCrosshair(weaponData.CrosshairSetting.CrosshairType);
    }
    private void ToggleWeaponControllers()
    {
        _combatController.PlayerStateMachine.CoreControllers.Stats.Stats.RangeWeaponStamina.ToggleUseStamina(false);
        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Toggle(true);
        _combatController.PlayerStateMachine.AnimatingControllers.Weapon.Sway.Toggle(true);
        _combatController.PlayerStateMachine.CombatControllers.WallDetector.ToggleCollider(true);
        _combatController.EquipedWeaponSlot.Weapon.DamageDealingController.Toggle(_combatController.EquipedWeaponSlot.Weapon.HoldController.HoldMode == WeaponHoldController.HoldModeEnum.Hip);
    }
    private void DisableBakedLayer()
    {
        PlayerIkLayerController playerIkLayerController = _combatController.PlayerStateMachine.AnimatingControllers.IkLayers;
        playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, false, 0.1f);
    }
    private void ResetEquipAnimBool()
    {
        this.Delay(0.5f, () => { _combatController.PlayerStateMachine.AnimatingControllers.Animator.SetBool("EquipWeapon", false); });
    }



    private void Swap()
    {
        _combatController.Swap = true;
        _combatController.UnEquip.StartUnEquip(1);
    }
    public void ReEquip(int choosenWeaponIndex)
    {
        StartEquip(choosenWeaponIndex);
        _combatController.Swap = false;
    }
}
