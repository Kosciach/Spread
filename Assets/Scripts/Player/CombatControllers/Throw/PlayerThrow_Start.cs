using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerThrowController;

public class PlayerThrow_Start : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerThrowController _throwController;



    public void StartThrow()
    {
        if (!_throwController.IsState(ThrowableStates.Hold)) return;

        _throwController.SetState(ThrowableStates.StartThrow);


        _throwController.Throw.Throw();
    }
}
