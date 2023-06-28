using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class WeaponStateMachine : MonoBehaviour
{
    private WeaponBaseState _currentState; public WeaponBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    private WeaponStateFactory _stateFactory;
    [SerializeField] string _currentStateName; public string CurrentStateName { get { return _currentStateName; } set { _currentStateName = value; } }
    [SerializeField] StateSwitchEnum _stateSwitch; public StateSwitchEnum StateSwitch { get { return _stateSwitch; } set { _stateSwitch = value; } }


    [Space(20)]
    [Header("====References====")]
    [SerializeField] Animator _animator; public Animator Animator {  get { return _animator; } }
    [SerializeField] Collider _collider; public Collider Collider { get { return _collider; } }
    [SerializeField] Rigidbody _rigidbody; public Rigidbody Rigidbody { get { return _rigidbody; } }
    [SerializeField] Outline _outline; public Outline Outline { get { return _outline; } }
    [Space(5)]
    [SerializeField] WeaponHoldController _holdModeController; public WeaponHoldController HoldController { get { return _holdModeController; } }
    [SerializeField] WeaponDamageDealingController _damageDealingController; public WeaponDamageDealingController DamageDealingController { get { return _damageDealingController; } }
    [SerializeField] WeaponDataHolder _dataHolder; public WeaponDataHolder DataHolder { get { return _dataHolder; } }
    [SerializeField] WeaponAimIndexHolder _aimIndexHolder; public WeaponAimIndexHolder AimIndexHolder { get { return _aimIndexHolder; } }


    [Space(20)]
    [Header("====References====")]
    [SerializeField] AnimatorOverrideController _animatorOverride; public AnimatorOverrideController AnimatorOverride { get { return _animatorOverride; } }


    private PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    private RangeWeaponSwitchController _switchController; public RangeWeaponSwitchController SwitchController { get { return _switchController; } }

    private Transform _meshes;



    public enum StateSwitchEnum
    {
        Ground, Inventory, Equiped
    }





    private void Awake()
    {
        _meshes = transform.GetChild(0);

        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();

        SetUpStartingState();
        _switchController = new RangeWeaponSwitchController(this);
    }
    private void SetUpStartingState()
    {
        _stateFactory = new WeaponStateFactory(this);
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



    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in _meshes)
        {
            child.gameObject.layer = layer;

            if (child.childCount > 0)
                foreach (Transform smallerChild in child)
                    smallerChild.gameObject.layer = layer;
        }
    }





    public void OnWeaponEquip()
    {
        if (_playerStateMachine.CurrentStateName == "Idle") OnPlayerIdle();
        else if (_playerStateMachine.CurrentStateName == "Walk") OnPlayerWalk();
        else if (_playerStateMachine.CurrentStateName == "Run") OnPlayerCrouch();

        OnWeaponAim(_playerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim);

        _damageDealingController.OnWeaponEquip();
    }
    public void OnWeaponUnEquip()
    {
        _damageDealingController.OnWeaponUnEquip();
    }


    public void OnPlayerIdle()
    {
        _damageDealingController.OnPlayerIdle();
    }
    public void OnPlayerWalk()
    {
        _damageDealingController.OnPlayerWalk();
    }
    public void OnPlayerCrouch()
    {
        _damageDealingController.OnPlayerCrouch();
    }

    public void OnWeaponAim(bool isAim)
    {
        _damageDealingController.OnWeaponAim(isAim);
    }
}





public class WeaponStateFactory
{
    private WeaponStateMachine _ctx;

    public WeaponStateFactory(WeaponStateMachine ctx)
    {
        _ctx = ctx;
    }


    public WeaponBaseState Ground()
    {
        return new WeaponGroundState(_ctx, this, MethodBase.GetCurrentMethod().Name);
    }
    public WeaponBaseState Inventory()
    {
        return new RangeWeaponInventoryState(_ctx, this, MethodBase.GetCurrentMethod().Name);
    }
    public WeaponBaseState Equiped()
    {
        return new WeaponEquipedState(_ctx, this, MethodBase.GetCurrentMethod().Name);
    }
}





public class RangeWeaponSwitchController
{
    private WeaponStateMachine _stateMachine;
    private SwitchToClass _switchToClass; public SwitchToClass SwitchTo { get { return _switchToClass; } }

    public RangeWeaponSwitchController(WeaponStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _switchToClass = new SwitchToClass(stateMachine, this);
    }





    public bool IsStateSwitch(WeaponStateMachine.StateSwitchEnum stateSwitch)
    {
        return _stateMachine.StateSwitch == stateSwitch;
    }



    public class SwitchToClass
    {
        private WeaponStateMachine _stateMachine;
        private RangeWeaponSwitchController _switchController;

        public SwitchToClass(WeaponStateMachine stateMachine, RangeWeaponSwitchController switchController)
        {
            _stateMachine = stateMachine;
            _switchController = switchController;
        }




        public void Ground()
        {
            _stateMachine.StateSwitch = WeaponStateMachine.StateSwitchEnum.Ground;
        }

        public void Inventory()
        {
            _stateMachine.StateSwitch = WeaponStateMachine.StateSwitchEnum.Inventory;
        }

        public void Equiped()
        {
            _stateMachine.StateSwitch = WeaponStateMachine.StateSwitchEnum.Equiped;
        }
    }
}