using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableStateMachine : MonoBehaviour
{
    private ThrowableBaseState[] _states;
    private ThrowableBaseState _currentState;

    [Header("====Debugs====")]
    [SerializeField] StateLabels _currentStateLabel;


    private ThrowableData _throwableData;                       public ThrowableData ThrowableData { get { return _throwableData; } }
    private Rigidbody _rigidbody;                               public Rigidbody Rigidbody { get { return _rigidbody; } }
    private BaseThrowableController _throwableController;       public BaseThrowableController ThrowableController { get { return _throwableController; } }
    private PlayerStateMachine _playerStateMachine;             public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }

    public enum StateLabels
    {
        Safe, InHand, Thrown
    }




    private void Awake()
    {
        _throwableData = (ThrowableData)GetComponent<ItemDataHolder>().ItemData;
        _rigidbody = GetComponent<Rigidbody>();
        _throwableController = GetComponent<BaseThrowableController>();
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        PrepareStates();
    }



    private void PrepareStates()
    {
        _states = new ThrowableBaseState[3];
        _states[(int)StateLabels.Safe] = new ThrowableState_Safe(this);
        _states[(int)StateLabels.InHand] = new ThrowableState_InHand(this);
        _states[(int)StateLabels.Thrown] = new ThrowableState_Thrown(this);
        _currentState = _states[(int)_currentStateLabel];

        _currentState.StateEnter();
    }

    public void ChangeState(StateLabels stateLabel)
    {
        _currentState.StateExit();

        _currentState = _states[(int)stateLabel];
        _currentStateLabel = stateLabel;

        _currentState.StateEnter();
    }



    public void ChangeLayer(Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        if (parent.childCount <= 0) return;

        for(int i=0; i<parent.childCount; i++)
        {
            ChangeLayer(parent.GetChild(i), layer);
        }
    }
}