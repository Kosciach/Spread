using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using PlayerHandsCamera;

namespace PlayerThrow
{
    public class PlayerThrow_Hold : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void Hold()
        {
            PlayerInventoryController playerInventory = _throwController.PlayerStateMachine.InventoryControllers.Inventory;

            if (!_throwController.CanThrow) return;
            if (!IsCorrectPlayerState()) return;
            if (!IsCorrectCombatState()) return;
            if (playerInventory.Throwables.GetFirstNotEmptySlot() < 0) return;

            _throwController.CanCancel = true;
            _throwController.IsHeld = true;
            _throwController.IsThrow = true;
            HandleWeapons();
            PrepareHandsCamera();

            SpawnThrowable(playerInventory);

            SetThrowableTypeAnims();
            ToggleLayers();
            PlayEquip();
        }



        private bool IsCorrectPlayerState()
        {
            return _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
                    || _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
                    || _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run)
                    || _throwController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Jump);
        }
        private bool IsCorrectCombatState()
        {
            return _throwController.PlayerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Equiped)
                    || _throwController.PlayerStateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed);
        }

        private void HandleWeapons()
        {
            _throwController.PlayerStateMachine.CombatControllers.Combat.TemporaryUnEquip.StartTemporaryUnEquip(false);
        }
        private void PrepareHandsCamera()
        {
            _throwController.PlayerStateMachine.CameraControllers.Hands.Move.ChangePreset(PositionsPresetsLabels.Throw, 0.1f);
            _throwController.PlayerStateMachine.CameraControllers.Hands.Rotate.ChangePreset(RotationPresetsLabels.Throw, 0.1f);
        }
        private void SpawnThrowable(PlayerInventoryController playerInventory)
        {
            //Get index and data
            int notEmptySlotIndex = playerInventory.Throwables.GetFirstNotEmptySlot();
            ThrowableData currentThrowableData = (ThrowableData)playerInventory.Throwables.ThrowableInventorySlots[notEmptySlotIndex].ItemData;

            //Spawn and get stateMachine
            GameObject currentThrowable = Instantiate(currentThrowableData.ItemPrefab, _throwController.ThrowableHolder);
            _throwController.CurrentThrowable = currentThrowable.GetComponent<ThrowableStateMachine>();

            //Change throwable state
            _throwController.CurrentThrowable.ChangeState(ThrowableStateMachine.StateLabels.InHand);
        }
        private void SetThrowableTypeAnims()
        {
            int throwableTypeInt = (int)_throwController.CurrentThrowable.ThrowableData.ThrowableType;
            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.SetFloat("ThrowableType", throwableTypeInt);
        }
        private void ToggleLayers()
        {
            PlayerIkLayerController playerIkLayerController = _throwController.PlayerStateMachine.AnimatingControllers.IkLayers;
            playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, true, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, true, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, true, 0.1f);

            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowBase, true, 0.4f);
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowAnimating, true, 0.4f);
        }
        private void PlayEquip()
        {
            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.SetTrigger("EquipThrow", false);
        }
    }
}