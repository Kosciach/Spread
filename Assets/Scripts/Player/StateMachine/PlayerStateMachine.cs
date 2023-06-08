using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using UnityEngine.VFX;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerBaseState _currentState; public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    private PlayerStateFactory _factory; public PlayerStateFactory Factory { get { return _factory; } set { _factory = value; } }


    [Header("====StateMachine====")]
    [SerializeField] string _currentStateName; public string CurrentStateName { get { return _currentStateName; } set { _currentStateName = value; } }
    [SerializeField] SwitchEnum _stateSwitch; public SwitchEnum StateSwitch { get { return _stateSwitch; } set { _stateSwitch = value; } }


    [Space(20)]
    [Header("====Controllers====")]
    [SerializeField] CoreControllersStruct _coreControllers; public CoreControllersStruct CoreControllers { get { return _coreControllers; } }
    [SerializeField] AnimatingControllersStruct _animatingControllers; public AnimatingControllersStruct AnimatingControllers { get { return _animatingControllers; } }
    [SerializeField] CameraControllersStruct _cameraControllers; public CameraControllersStruct CameraControllers { get { return _cameraControllers; } }
    [SerializeField] MovementControllersStruct _movementControllers; public MovementControllersStruct MovementControllers { get { return _movementControllers; } }
    [SerializeField] StateControllersStruct _stateControllers; public StateControllersStruct StateControllers { get { return _stateControllers; } }
    [SerializeField] CombatControllersStruct _combatControllers; public CombatControllersStruct CombatControllers { get { return _combatControllers; } }
    [SerializeField] InventoryControllersStruct _inventoryControllers; public InventoryControllersStruct InventoryControllers { get { return _inventoryControllers; } }
    [SerializeField] AudioControllersStruct _audioControllers; public AudioControllersStruct AudioControllers { get { return _audioControllers; } }


    private PlayerSwitchController _switchController; public PlayerSwitchController SwitchController { get { return _switchController; } }




    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _wasHardLanding; public bool WasHardLanding { get { return _wasHardLanding; } set { _wasHardLanding = value; } }




    [System.Serializable]
    public struct CoreControllersStruct
    {
        public PlayerInputController Input;
        public PlayerColliderController Collider;
        public PlayerInteractionController Interaction;
    }

    [System.Serializable]
    public struct AnimatingControllersStruct
    {
        public PlayerAnimatorController Animator;
        public WeaponAnimator Weapon;
        public LeftHandAnimator LeftHand;
        public PlayerFingerAnimator Fingers;
        public PlayerIkLayerController IkLayers;
        public WeaponReloadAnimator Reload;
    }

    [System.Serializable]
    public struct CameraControllersStruct
    {
        public PlayerCineCameraController Cine;
        public PlayerHandsCameraController Hands;
    }

    [System.Serializable]
    public struct MovementControllersStruct
    {
        public PlayerMovementController Movement;
        public PlayerVerticalVelocityController VerticalVelocity;
        public PlayerVelocity Velocity;
        public PlayerRotationController Rotation;
    }

    [System.Serializable]
    public struct StateControllersStruct
    {
        public PlayerClimbController Climb;
        public PlayerSwimController Swim;
        public PlayerLadderController Ladder;
        public PlayerDashController Dash;
    }

    [System.Serializable]
    public struct CombatControllersStruct
    {
        public PlayerCombatController Combat;
        public PlayerEquipedWeaponController EquipedWeapon;
        public PlayerLeaningController Leaning;
    }

    [System.Serializable]
    public struct InventoryControllersStruct
    {
        public PlayerInventory Inventory;
    }

    [System.Serializable]
    public struct AudioControllersStruct
    {
        public PlayerFootStepAudioController FootStep;
    }



    public enum SwitchEnum
    {
        Idle, Walk, Run,
        Jump, Fall, Land,
        Crouch,
        Climb, InAirClimb,
        Ladder,
        Swim, UnderWater,
        Dash
    }





    private void Awake()
    {
        _switchController = new PlayerSwitchController(this);
        SetStartingState();
    }
    private void SetStartingState()
    {
        _factory = new PlayerStateFactory(this);
        _currentState = _factory.Idle();
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






    public void RecoverFromLanding()
    {
        _switchController.SwitchTo.Idle();
    }
}
