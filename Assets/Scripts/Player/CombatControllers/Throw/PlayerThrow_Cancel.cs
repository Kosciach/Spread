using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

namespace PlayerThrow
{
    public class PlayerThrow_Cancel : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void StartCancel()
        {
            if (!_throwController.CanCancel) return;
            _throwController.CanCancel = false;
            _throwController.IsHeld = false;

            PlayUnEquip();
        }
        public void EndCancel()
        {
            ToggleLayers();
            RemoveThrowable();

            _throwController.PlayerStateMachine.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
            _throwController.CanThrow = true;
            _throwController.IsThrow = false;
        }


        private void PlayUnEquip()
        {
            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.SetTrigger("UnEquipThrow", false);
        }
        private void ToggleLayers()
        {
            PlayerIkLayerController playerIkLayerController = _throwController.PlayerStateMachine.AnimatingControllers.IkLayers;
            playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, false, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, false, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, false, 0.1f);

            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowBase, false, 0.1f);
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowAnimating, false, 0.1f);
        }
        private void RemoveThrowable()
        {
            _throwController.CurrentThrowable.ChangeState(ThrowableStateMachine.StateLabels.Safe);
            Destroy(_throwController.CurrentThrowable.gameObject);
            _throwController.CurrentThrowable = null;
        }
    }
}