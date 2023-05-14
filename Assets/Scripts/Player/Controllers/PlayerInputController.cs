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
    [SerializeField] Vector2 _mouseInputVector; public Vector2 MouseInputVector { get { return _mouseInputVector; } }
    [SerializeField] Vector3 _movementInputVector; public Vector3 MovementInputVector { get { return _movementInputVector; } }
    [SerializeField] Vector3 _movementInputVectorNormalized; public Vector3 MovementInputVectorNormalized { get { return _movementInputVectorNormalized; } }
    [SerializeField] bool _isMoveInput; public bool IsMoveInput { get { return _isMoveInput; } }
    [SerializeField] bool _isRunInput; public bool IsRunInput { get { return _isRunInput; } }




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
        SetEquipWeapon();
        SetHideWeapon();
        SetDropWeapon();
        SetChangeWeaponEquipedMode();
        SetADS();
    }
    private void Update()
    {
        SetMovementInputVector();
        SetMouseInputVector();
    }







    private void SetMovementInputVector()
    {
        Vector2 inputVector = _playerInputs.Player.Move.ReadValue<Vector2>();
        _movementInputVector = new Vector3(inputVector.x, 0f, inputVector.y);
        _movementInputVectorNormalized = _movementInputVector.normalized;
    }
    private void SetMouseInputVector()
    {
        _mouseInputVector = _playerInputs.Player.Look.ReadValue<Vector2>();
    }



    private void SetMoving()
    {
        _playerInputs.Player.Move.started += ctx => { _isMoveInput = true; };
        _playerInputs.Player.Move.canceled += ctx => { _isMoveInput = false;};
    }
    private void SetRunning()
    {
        _playerInputs.Player.Run.started += ctx => { _isRunInput = true; };
        _playerInputs.Player.Run.canceled += ctx => { _isRunInput = false; };
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


    private void SetEquipWeapon()
    {
        _playerInputs.Player.ChooseWeapon.performed += ctx => _stateMachine.CombatController.EquipWeapon((int)ctx.ReadValue<float>());
    }
    private void SetHideWeapon()
    {
        _playerInputs.Player.HideWeapon.performed += ctx => _stateMachine.CombatController.HideWeapon();
    }
    private void SetDropWeapon()
    {
        _playerInputs.Player.DropWeapon.performed += ctx => _stateMachine.CombatController.DropWeapon();
    }


    private void SetChangeWeaponEquipedMode()
    {
        _playerInputs.Player.ChangeWeaponEquipedMode.performed += ctx => _stateMachine.CombatController.EquipedWeaponController.ChangeEquipedHoldMode();
    }

    private void SetADS()
    {
        _playerInputs.Player.ADS.started += ctx => _stateMachine.CombatController.EquipedWeaponController.ADS(true);
        _playerInputs.Player.ADS.canceled += ctx => _stateMachine.CombatController.EquipedWeaponController.ADS(false);
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
