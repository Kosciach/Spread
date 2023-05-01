using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RangeWeaponStateMachine : MonoBehaviour
{
    private RangeWeaponBaseState _currentState; public RangeWeaponBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    private RangeWeaponStateFactory _stateFactory;
    [SerializeField] string _currentStateName; public string CurrentStateName { get { return _currentStateName; } set { _currentStateName = value; } }
    [SerializeField] StateSwitchEnum _stateSwitch; public StateSwitchEnum StateSwitch { get { return _stateSwitch; } set { _stateSwitch = value; } }


    [Space(20)]
    [Header("====References====")]
    [SerializeField] BoxCollider _collider; public BoxCollider Collider { get { return _collider; } }
    [SerializeField] Rigidbody _rigidbody; public Rigidbody Rigidbody { get { return _rigidbody; } }
    private RangeWeaponSwitchController _switchController; public RangeWeaponSwitchController SwitchController { get { return _switchController; } }





    public enum StateSwitchEnum
    {
        Ground, Inventory, Equiped
    }





    private void Awake()
    {
        SetUpStartingState();
        _switchController = new RangeWeaponSwitchController(this);
    }
    private void SetUpStartingState()
    {
        _stateFactory = new RangeWeaponStateFactory(this);
        _currentState = _stateFactory.Ground();
        _currentState.StateEnter();
    }



    private void Update()
    {
        _currentState.StateUpdate();
        _currentState.StateCheckChange();
    }
    private void FixedUpdate()
    {
        _currentState.StateFixedUpdate();
    }


}





public class RangeWeaponStateFactory
{
    private RangeWeaponStateMachine _ctx;

    public RangeWeaponStateFactory(RangeWeaponStateMachine ctx)
    {
        _ctx = ctx;
    }


    public RangeWeaponBaseState Ground()
    {
        return new RangeWeaponGroundState(_ctx, this, MethodBase.GetCurrentMethod().Name);
    }
    public RangeWeaponBaseState Inventory()
    {
        return new RangeWeaponInventoryState(_ctx, this, MethodBase.GetCurrentMethod().Name);
    }
    public RangeWeaponBaseState Equiped()
    {
        return new RangeWeaponEquipedState(_ctx, this, MethodBase.GetCurrentMethod().Name);
    }
}





public class RangeWeaponSwitchController
{
    private RangeWeaponStateMachine _stateMachine;
    private SwitchToClass _switchToClass; public SwitchToClass SwitchTo { get { return _switchToClass; } }

    public RangeWeaponSwitchController(RangeWeaponStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _switchToClass = new SwitchToClass(stateMachine, this);
    }





    public bool IsStateSwitch(RangeWeaponStateMachine.StateSwitchEnum stateSwitch)
    {
        return _stateMachine.StateSwitch == stateSwitch;
    }



    public class SwitchToClass
    {
        private RangeWeaponStateMachine _stateMachine;
        private RangeWeaponSwitchController _switchController;

        public SwitchToClass(RangeWeaponStateMachine stateMachine, RangeWeaponSwitchController switchController)
        {
            _stateMachine = stateMachine;
            _switchController = switchController;
        }




        public void Ground()
        {
            _stateMachine.StateSwitch = RangeWeaponStateMachine.StateSwitchEnum.Ground;
        }

        public void Inventory()
        {
            _stateMachine.StateSwitch = RangeWeaponStateMachine.StateSwitchEnum.Inventory;
        }

        public void Equiped()
        {
            _stateMachine.StateSwitch = RangeWeaponStateMachine.StateSwitchEnum.Equiped;
        }
    }
}