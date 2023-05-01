using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInputController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _stateMachine;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _movementInputVector; public Vector3 MovementInputVector { get { return _movementInputVector; } }
    [SerializeField] Vector3 _movementInputVectorNormalized; public Vector3 MovementInputVectorNormalized { get { return _movementInputVectorNormalized; } }
    [SerializeField] bool _isMoving; public bool IsMoving { get { return _isMoving; } }
    [SerializeField] bool _isRunning; public bool IsRunning { get { return _isRunning; } }




    private PlayerInputs _playerInputs;





    private void Awake()
    {
        _playerInputs = new PlayerInputs();
    }
    private void Start()
    {
        SetMoving();
        SetRunning();
        SetJump();
        SetCrouch();
        SetInteraction();
    }
    private void Update()
    {
        SetMovementVector();
    }







    private void SetMovementVector()
    {
        Vector2 inputVector = _playerInputs.Player.Move.ReadValue<Vector2>();
        _movementInputVector = new Vector3(inputVector.x, 0f, inputVector.y);
        _movementInputVectorNormalized = _movementInputVector.normalized;
    }


    private void SetMoving()
    {
        _playerInputs.Player.Move.started += ctx => { _isMoving = true; };
        _playerInputs.Player.Move.canceled += ctx => { _isMoving = false; _isRunning = false; };
    }
    private void SetRunning()
    {
        _playerInputs.Player.Run.started += ctx => { if (_isMoving) { _isRunning = true; } };
        _playerInputs.Player.Run.canceled += ctx => { _isRunning = false; };
    }
    private void SetJump()
    {
        _playerInputs.Player.Jump.performed += ctx => _stateMachine.SwitchController.SwitchTo.Jump();
    }
    private void SetCrouch()
    {
        _playerInputs.Player.Crouch.performed += ctx => _stateMachine.SwitchController.SwitchTo.Crouch();
    }
    private void SetInteraction()
    {
        _playerInputs.Player.Interact.performed += ctx => _stateMachine.InteractionController.Interaction();
    }




    public PlayerStateMachine.SwitchEnum GetPlayerBaseMovementType()
    {
        if (!_isMoving) return PlayerStateMachine.SwitchEnum.Idle;

        if (!_isRunning) return PlayerStateMachine.SwitchEnum.Walk;

        return PlayerStateMachine.SwitchEnum.Run;
    }


    private void OnEnable()
    {
        _playerInputs.Enable();
    }
    private void OnDisable()
    {
        _playerInputs.Disable();
    }
}
