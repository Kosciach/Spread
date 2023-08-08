using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleMan.CoroutineExtensions;

namespace PlayerThrow
{
    public class PlayerThrow_Throw : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void Throw()
        {
            _throwController.SetState(ThrowableStates.Throw);

            ThrowThrowable();
            _throwController.CurrentThrowable = null;

            this.Delay(0.3f, () => { _throwController.End.End(); });
        }


        private void ThrowThrowable()
        {
            _throwController.CurrentThrowable.ChangeState(ThrowableStateMachine.StateLabels.Thrown);
            Vector3 throwVector = _throwController.PlayerStateMachine.CameraControllers.Cine.MainCamera.transform.forward + Vector3.up / 2;
            _throwController.CurrentThrowable.Rigidbody.AddForce(throwVector * _throwController.CurrentThrowable.ThrowableData.ThrowStrenght, ForceMode.Impulse);
        }
    }
}