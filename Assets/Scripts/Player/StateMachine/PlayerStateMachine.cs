using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerBaseState _currentState; public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    private PlayerStateFactory _factory; public PlayerStateFactory Factory { get { return _factory; } set { _factory = value; } }


    [Header("====StateMachine====")]
    [SerializeField] string _currentStateName; public string CurrentStateName { get { return _currentStateName; } set { _currentStateName = value; } }
    [SerializeField] SwitchEnum _stateSwitch; public SwitchEnum StateSwitch { get { return _stateSwitch; } set { _stateSwitch = value; } }


    [Space(20)]
    [Header("====PlayerScripts====")]

    [SerializeField] PlayerInputController _inputController; public PlayerInputController InputController { get { return _inputController; } }
    [SerializeField] PlayerCombatController _combatController; public PlayerCombatController CombatController { get { return _combatController; } }
    [SerializeField] PlayerMainCameraController _cameraController; public PlayerMainCameraController CameraController { get { return _cameraController; } }
    [SerializeField] PlayerMovementController _movementController; public PlayerMovementController MovementController { get { return _movementController; } }
    [SerializeField] PlayerGravityController _gravityController; public PlayerGravityController GravityController { get { return _gravityController; } }
    [SerializeField] PlayerJumpController _jumpController; public PlayerJumpController JumpController { get { return _jumpController; } }
    [SerializeField] PlayerSlopeController _slopeController; public PlayerSlopeController SlopeController { get { return _slopeController; } }
    [SerializeField] PlayerColliderController _colliderController; public PlayerColliderController ColliderController { get { return _colliderController; } }
    [SerializeField] PlayerClimbController _climbController; public PlayerClimbController ClimbController { get { return _climbController; } }
    [SerializeField] PlayerSwimController _swimController; public PlayerSwimController SwimController { get { return _swimController; } }
    [SerializeField] PlayerLadderController _ladderController; public PlayerLadderController LadderController { get { return _ladderController; } }
    [SerializeField] PlayerDashController _dashController; public PlayerDashController DashController { get { return _dashController; } }
    [SerializeField] PlayerInteractionController _interactionController; public PlayerInteractionController InteractionController { get { return _interactionController; } }
    [SerializeField] PlayerAnimatorController _animatorController; public PlayerAnimatorController AnimatorController { get { return _animatorController; } }
    [SerializeField] PlayerIkLayerController _ikLayerController; public PlayerIkLayerController IkLayerController { get { return _ikLayerController; } }
    [SerializeField] PlayerFootStepAudioController _footStepAudioController; public PlayerFootStepAudioController FootStepAudioController { get { return _footStepAudioController; } }
    [SerializeField] PlayerInventory _inventory; public PlayerInventory Inventory { get { return _inventory; } }

    [SerializeField] PlayerCameraFovController _cameraFovController; public PlayerCameraFovController CameraFovController { get { return _cameraFovController; } }
    [SerializeField] PlayerCameraMoveController _cameraMoveController; public PlayerCameraMoveController CameraMoveController { get { return _cameraMoveController; } }
    [SerializeField] PlayerCameraRotateController _cameraRotateController; public PlayerCameraRotateController CameraRotateController { get { return _cameraRotateController; } }
    [SerializeField] PlayerHandsCameraEnableController _handsCameraEnableController; public PlayerHandsCameraEnableController HandsCameraEnableController { get { return _handsCameraEnableController; } }

    private PlayerSwitchController _switchController; public PlayerSwitchController SwitchController { get { return _switchController; } }




    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _wasHardLanding; public bool WasHardLanding { get { return _wasHardLanding; } set { _wasHardLanding = value; } }





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
        _switchController = new PlayerSwitchController(this, _gravityController, _inputController, _climbController, _jumpController);
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
        _stateSwitch = _inputController.GetPlayerBaseMovementType();
    }
}
