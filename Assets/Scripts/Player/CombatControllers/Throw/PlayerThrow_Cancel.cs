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
    public class PlayerThrow_Cancel : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void Cancel()
        {
            _throwController.SetState(ThrowableStates.CancelThrow);

            ToggleLayers();

            _throwController.CurrentThrowable.ChangeState(ThrowableStateMachine.StateLabels.Safe);
            Destroy(_throwController.CurrentThrowable.gameObject);
            _throwController.CurrentThrowable = null;

            _throwController.PlayerStateMachine.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
            _throwController.SetState(ThrowableStates.ReadyToThrow);
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
    }
}