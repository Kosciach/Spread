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
            if (!_throwController.IsState(ThrowableStates.StartThrow)) return;

            _throwController.SetState(ThrowableStates.Throw);


            Debug.Log("Throw it!");
            _throwController.CurrentThrowable.ChangeState(ThrowableStateMachine.StateLabels.Thrown);
            Vector3 throwVector = _throwController.PlayerStateMachine.CameraControllers.Cine.MainCamera.transform.forward + Vector3.up / 2;
            _throwController.CurrentThrowable.Rigidbody.AddForce(throwVector * _throwController.CurrentThrowable.ThrowableData.ThrowStrenght, ForceMode.Impulse);

            _throwController.End.End();
        }
    }
}