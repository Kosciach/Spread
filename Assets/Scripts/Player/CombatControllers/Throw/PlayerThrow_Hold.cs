using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using WeaponAnimatorNamespace;
using static PlayerAnimator.PlayerAnimatorController;
using static IkLayers.PlayerIkLayerController;
using LeftHandAnimatorNamespace;

namespace PlayerThrow
{
    public class PlayerThrow_Hold : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void Hold()
        {
            PlayerInventoryController playerInventory = _throwController.PlayerStateMachine.InventoryControllers.Inventory;

            if (!_throwController.IsState(ThrowableStates.ReadyToThrow)) return;
            if (!IsCorrectPlayerState()) return;
            if (!IsCorrectCombatState()) return;
            if (playerInventory.Throwables.GetFirstNotEmptySlot() < 0) return;

            _throwController.SetState(ThrowableStates.Hold);


            SpawnThrowable(playerInventory);

            LeftHandControll();
            MoveIk();
            ToggleLayers();
            ToggleSwayBob();
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

        private void LeftHandControll()
        {
            LeftHandAnimator leftHandAnimator = _throwController.PlayerStateMachine.AnimatingControllers.LeftHand;
            leftHandAnimator.RightHandFollow.Toggle(false);

            leftHandAnimator.MainTransformer.MoveRaw(Vector3.zero);
        }
        private void MoveIk()
        {
            WeaponAnimator weaponAnimator = _throwController.PlayerStateMachine.AnimatingControllers.Weapon;
            weaponAnimator.MainTransformer.MoveRaw(_throwController.CurrentThrowable.ThrowableData.RightHandHold.Pos);
            weaponAnimator.MainTransformer.RotateRaw(_throwController.CurrentThrowable.ThrowableData.RightHandHold.Rot);
        }
        private void ToggleLayers()
        {
            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.ToggleLayer(LayersEnum.CombatBase, true, 0.4f);
            playerAnimatorController.ToggleLayer(LayersEnum.CombatAnimating, false, 0.4f);

            PlayerIkLayerController playerIkLayerController = _throwController.PlayerStateMachine.AnimatingControllers.IkLayers;
            playerIkLayerController.ToggleLayer(LayerEnum.SpineLock, false, 0.4f);
            playerIkLayerController.ToggleLayer(LayerEnum.Body, false, 0.2f);
            playerIkLayerController.ToggleLayer(LayerEnum.Head, false, 0.2f);
            playerIkLayerController.ToggleLayer(LayerEnum.RangeCombat, true, 0.2f);
        }
        private void ToggleSwayBob()
        {
            WeaponAnimator weaponAnimator = _throwController.PlayerStateMachine.AnimatingControllers.Weapon;
            weaponAnimator.Sway.SetWeight(1);
            weaponAnimator.Sway.Toggle(true);
            weaponAnimator.Bobbing.Toggle(true);
        }
    }
}