using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableStateMachine : MonoBehaviour
{
    private ThrowableBaseState[] _states;
    private ThrowableBaseState _currentState;


    private ThrowableData _throwableData;                       public ThrowableData ThrowableData { get { return _throwableData; } }
    private BaseThrowableController _throwableController;       public BaseThrowableController ThrowableController { get { return _throwableController; } }


    public enum StateLabels
    {
        Safe, Activated
    }




    private void Awake()
    {
        _throwableData = (ThrowableData)GetComponent<ItemDataHolder>().ItemData;
        _throwableController = GetComponent<BaseThrowableController>();
        PrepareStates();
    }



    private void PrepareStates()
    {
        _states = new ThrowableBaseState[2];
        _states[(int)StateLabels.Safe] = new ThrowableState_Safe(this);
        _states[(int)StateLabels.Activated] = new ThrowableState_Activated(this);
        _currentState = _states[(int)StateLabels.Safe];

        _currentState.StateEnter();
    }

    public void ChangeState(StateLabels stateLabel)
    {
        _currentState.StateExit();

        _currentState = _states[(int)stateLabel];

        _currentState.StateEnter();
    }
}