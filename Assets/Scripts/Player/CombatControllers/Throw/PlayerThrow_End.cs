using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerThrowController;

public class PlayerThrow_End : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerThrowController _throwController;



    public void End()
    {
        if (!_throwController.IsState(ThrowableStates.Throw)) return;

        _throwController.SetState(ThrowableStates.EndThrow);




        _throwController.SetState(ThrowableStates.ReadyToThrow);
    }
}
