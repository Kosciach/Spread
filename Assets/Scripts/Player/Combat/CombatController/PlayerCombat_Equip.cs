using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IkLayers;
using WeaponAnimatorNamespace;
using PlayerAnimator;

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
        || choosenWeaponIndex >= _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.WeaponInventorySlots.Count
        ||  !_combatController.PlayerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded) return;

        _combatController.ChoosenWeaponIndex = choosenWeaponIndex;
        WeaponInventorySlot choosenWeaponInventorySlot = _combatController.PlayerStateMachine.InventoryControllers.Inventory.Weapon.WeaponInventorySlots[choosenWeaponIndex];
        if (choosenWeaponInventorySlot.Empty) return;


        if(_combatController.IsState(PlayerCombatController.CombatStateEnum.Equiped))
        {
            if(choosenWeaponIndex != _combatController.EquipedWeaponIndex) Swap();
            return;
        }
        Equip(choosenWeaponIndex, choosenWeaponInventorySlot);
    }




    private void Equip(int choosenWeaponIndex, WeaponInventorySlot choosenWeaponInventorySlot)
    {
        _combatController.SetState(PlayerCombatController.CombatStateEnum.Equip);

        _combatController.IsTemporaryUnEquip = false;
        _combatController.EquipedWeaponIndex = choosenWeaponIndex;
        _combatController.EquipedWeaponSlot = choosenWeaponInventorySlot;

        _combatController.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.LoadAimTypeFromWeapon(_combatController.EquipedWeaponSlot.Weapon);
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.OverrideAnimator(_combatController.EquipedWeaponSlot.Weapon.AnimatorOverride);

        _combatController.EquipedWeaponSlot.Weapon.SwitchController.SwitchTo.Equiped();

        _combatController.PlayerStateMachine.CameraControllers.Hands.Move.SetCameraPosition(PlayerHandsCamera_Move.CameraPositionsEnum.Combat, 5);
        _combatController.PlayerStateMachine.CameraControllers.Hands.Rotate.SetHandsCameraRotation(PlayerHandsCamera_Rotate.HandsCameraRotationsEnum.Combat, 5);

        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersRightHand, true, 0.1f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.FingersLeftHand, true, 0.1f);
        _combatController.PlayerStateMachine.AnimatingControllers.Fingers.SetUpAllFingers(_combatController.EquipedWeaponSlot.WeaponData.FingersPreset.Base, 0.01f);
        _combatController.PlayerStateMachine.AnimatingControllers.Fingers.Discipline.SetDisciplineIk(_combatController.EquipedWeaponSlot.WeaponData.FingersPreset);

        WeaponAnimator_MainTransformer mainTransformer = _combatController.PlayerStateMachine.AnimatingControllers.Weapon.MainTransformer;
        mainTransformer.MoveRaw(_combatController.EquipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Position);
        mainTransformer.RotateRaw(_combatController.EquipedWeaponSlot.WeaponData.WeaponTransforms.Origin.RightHand_Rotation);

        _combatController.PlayerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.CombatBase, true, 0.4f);
        _combatController.PlayerStateMachine.AnimatingControllers.Animator.ToggleLayer(PlayerAnimatorController.LayersEnum.CombatAnimating, false, 0.4f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.SpineLock, false, 0.4f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Body, false, 0.4f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.Head, false, 0.4f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(PlayerIkLayerController.LayerEnum.RangeCombat, true, 0.4f);
        _combatController.PlayerStateMachine.AnimatingControllers.IkLayers.OnLerpFinish(PlayerIkLayerController.LayerEnum.RangeCombat, () =>
        {
            Transform equipedWeaponTransform = _combatController.EquipedWeaponSlot.Weapon.transform;
            WeaponData equipedWeaponData = _combatController.EquipedWeaponSlot.WeaponData;
            _combatController.PlayerStateMachine.AnimatingControllers.WeaponHolder.RightHand(equipedWeaponTransform);
            _combatController.PlayerStateMachine.AnimatingControllers.WeaponHolder.SetWeaponInHandTransform(equipedWeaponTransform, equipedWeaponData.InHandPosition, equipedWeaponData.InHandRotation);

            _combatController.PlayerStateMachine.CombatControllers.WallDetector.ToggleCollider(true);
            _combatController.EquipedWeaponSlot.Weapon.HoldController.MoveHandsToCurrentHoldMode(0.5f, 0.5f);

            _combatController.EquipedWeaponSlot.Weapon.OnWeaponEquip();
        });
    }
    private void Swap()
    {
        Debug.Log("Swap");
        _combatController.Swap = true;
        _combatController.UnEquip.StartUnEquip(1);
    }
    public void ReEquip(int choosenWeaponIndex)
    {
        StartEquip(choosenWeaponIndex);
        _combatController.Swap = false;
    }
}
