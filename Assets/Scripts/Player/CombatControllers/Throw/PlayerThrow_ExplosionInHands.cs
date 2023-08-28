using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using SimpleMan.CoroutineExtensions;

namespace PlayerThrow
{
    public class PlayerThrow_ExplosionInHands : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void ExplosionInHands()
        {
            _throwController.CanThrow = false;
            _throwController.CanCancel = false;
            _throwController.IsHeld = false;

            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.SetTrigger("ExplosionInHands", false);
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowBase, false, 0.1f);
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowAnimating, false, 0.1f);

            PlayerIkLayerController playerIkLayerController = _throwController.PlayerStateMachine.AnimatingControllers.IkLayers;
            playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, false, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, false, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, false, 0.1f);


            _throwController.CanThrow = true;
            _throwController.IsThrow = false;
            _throwController.PlayerStateMachine.CameraControllers.Hands.Enable.ToggleHandsCamera(false);
            this.Delay(0.2f, () => { _throwController.PlayerStateMachine.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip(); });
        }
    }
}