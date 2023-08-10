using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

namespace PlayerThrow
{
    public class PlayerThrow_Start : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void StartThrow()
        {
            if (!_throwController.IsHeld) return;
            _throwController.CanThrow = false;
            _throwController.CanCancel = false;
            _throwController.IsHeld = false;

            _throwController.PlayerStateMachine.AnimatingControllers.Animator.SetTrigger("Throw", false);
        }
    }
}