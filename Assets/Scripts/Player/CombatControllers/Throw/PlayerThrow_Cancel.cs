using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerThrow
{
    public class PlayerThrow_Cancel : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void Cancel()
        {
            if (!_throwController.IsState(ThrowableStates.Hold)) return;
            _throwController.SetState(ThrowableStates.CancelThrow);


            _throwController.CurrentThrowable.ChangeState(ThrowableStateMachine.StateLabels.Safe);
            _throwController.CurrentThrowable = null;
            _throwController.SetState(ThrowableStates.ReadyToThrow);
        }
    }
}