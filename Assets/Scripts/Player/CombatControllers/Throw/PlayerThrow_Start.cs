using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using WeaponAnimatorNamespace;
using static PlayerAnimator.PlayerAnimatorController;
using static IkLayers.PlayerIkLayerController;
using SimpleMan.CoroutineExtensions;

namespace PlayerThrow
{
    public class PlayerThrow_Start : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void StartThrow()
        {
            if (!_throwController.IsState(ThrowableStates.Hold) || _throwController.IsState(ThrowableStates.ExplosionInHands)) return;

            _throwController.SetState(ThrowableStates.StartThrow);


            _throwController.PlayerStateMachine.AnimatingControllers.Animator.SetTrigger("Throw", false);

            this.Delay(0.4f, () => { _throwController.Throw.Throw(); });
        }
    }
}