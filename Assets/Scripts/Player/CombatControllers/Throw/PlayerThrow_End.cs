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
    public class PlayerThrow_End : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void End()
        {
            if (!_throwController.IsState(ThrowableStates.Throw)) return;

            _throwController.SetState(ThrowableStates.EndThrow);


            PlayerAnimatorController playerAnimatorController = _throwController.PlayerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.ToggleLayer(LayersEnum.CombatBase, false, 0.4f);
            playerAnimatorController.ToggleLayer(LayersEnum.CombatAnimating, false, 0.4f);

            PlayerIkLayerController playerIkLayerController = _throwController.PlayerStateMachine.AnimatingControllers.IkLayers;
            playerIkLayerController.ToggleLayer(LayerEnum.SpineLock, true, 0.4f);
            playerIkLayerController.ToggleLayer(LayerEnum.Body, true, 0.2f);
            playerIkLayerController.ToggleLayer(LayerEnum.Head, true, 0.2f);
            playerIkLayerController.ToggleLayer(LayerEnum.RangeCombat, false, 0.2f);

            LeftHandAnimator leftHandAnimator = _throwController.PlayerStateMachine.AnimatingControllers.LeftHand;
            leftHandAnimator.RightHandFollow.Toggle(true);

            _throwController.CurrentThrowable = null;
            _throwController.SetState(ThrowableStates.ReadyToThrow);
        }
    }
}