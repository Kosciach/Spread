using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerThrow
{
    public class PlayerThrow_Throw : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void Throw()
        {
            ThrowThrowable();
            _throwController.CurrentThrowable = null;
        }


        private void ThrowThrowable()
        {
            _throwController.CurrentThrowable.ChangeState(ThrowableStateMachine.StateLabels.Thrown);
            Vector3 throwVector = _throwController.PlayerStateMachine.CameraControllers.Cine.MainCamera.transform.forward + Vector3.up / 4;
            _throwController.CurrentThrowable.Rigidbody.AddForce(throwVector * _throwController.CurrentThrowable.ThrowableData.ThrowStrenght, ForceMode.Impulse);
        }
    }
}