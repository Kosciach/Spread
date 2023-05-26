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
        SetAim();
        SetBlock();
        SetChangeAimType();
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
        _playerInputs.Player.Interact.performed += ctx => _stateMachine.CoreControllers.Interaction.Interaction();
    }


    private void SetEquipWeapon()
    {
        _playerInputs.Player.ChooseWeapon.performed += ctx =>
        {
            if (_stateMachine.CombatControllers.Combat.IsTemporaryUnEquip) return;
            _stateMachine.CombatControllers.Combat.EquipWeapon((int)ctx.ReadValue<float>());
        };
    }
    private void SetHideWeapon()
    {
        _playerInputs.Player.UnEquipWeapon.performed += ctx =>
        {
            if (_stateMachine.CombatControllers.Combat.IsTemporaryUnEquip) return;
            _stateMachine.CombatControllers.Combat.UnEquipWeapon(1);
        };
    }
    private void SetDropWeapon()
    {
        _playerInputs.Player.DropWeapon.performed += ctx =>
        {
            if (_stateMachine.CombatControllers.Combat.IsTemporaryUnEquip) return;
            _stateMachine.CombatControllers.Combat.DropWeapon();
        };
    }

    private void SetChangeWeaponEquipedMode()
    {
        _playerInputs.Player.ChangeWeaponHoldMode.performed += ctx => _stateMachine.CombatControllers.Combat.EquipedWeaponController.Hold.ChangeEquipedHoldMode();
    }
    private void SetChangeAimType()
    {
        _playerInputs.Player.ChangeAimType.performed += ctx => _stateMachine.CombatControllers.Combat.EquipedWeaponController.Aim.ChangeAimType();
    }
    private void SetAim()
    {
        _playerInputs.Player.Aim.started += ctx => _stateMachine.CombatControllers.Combat.EquipedWeaponController.Aim.Aim(true);
        _playerInputs.Player.Aim.canceled += ctx => _stateMachine.CombatControllers.Combat.EquipedWeaponController.Aim.Aim(false);
    }
    private void SetBlock()
    {
        _playerInputs.Player.Block.started += ctx => _stateMachine.CombatControllers.Combat.EquipedWeaponController.Block.Block(true);
        _playerInputs.Player.Block.canceled += ctx => _stateMachine.CombatControllers.Combat.EquipedWeaponController.Block.Block(false);
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
