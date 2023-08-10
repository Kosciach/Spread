using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

namespace PlayerThrow
{
    public class PlayerThrow_End : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void End()
        {
            ToggleLayers();

            _throwController.PlayerStateMachine.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
            _throwController.CanThrow = true;
            _throwController.IsThrow = false;
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