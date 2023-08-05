using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerThrowController;

public class PlayerThrow_Throw : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerThrowController _throwController;



    public void Throw()
    {
        if (!_throwController.IsState(ThrowableStates.StartThrow)) return;

        _throwController.SetState(ThrowableStates.Throw);


        _throwController.End.End();
    }
}
