using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerThrowController;

public class PlayerThrow_Cancel : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerThrowController _throwController;



    public void Cancel()
    {
        if (!_throwController.IsState(ThrowableStates.Hold)) return;
        _throwController.SetState(ThrowableStates.CancelThrow);



        _throwController.SetState(ThrowableStates.ReadyToThrow);
    }
}
