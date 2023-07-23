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
    [SerializeField] bool _enabled;
    [Space(5)]
    [SerializeField] Vector2 _mouseInputVector;                 public Vector2 MouseInputVector { get { return _mouseInputVector; } }
    [SerializeField] Vector3 _movementInputVector;              public Vector3 MovementInputVector { get { return _movementInputVector; } }
    [SerializeField] Vector3 _movementInputVectorNormalized;    public Vector3 MovementInputVectorNormalized { get { return _movementInputVectorNormalized; } }
    [SerializeField] bool _isMoveInput;                         public bool IsMoveInput { get { return _isMoveInput; } }
    [SerializeField] bool _isRunInput;                          public bool IsRunInput { get { return _isRunInput; } }




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
        SetUnEquipWeapon();
        SetDropWeapon();
        SetChangeWeaponEquipedMode();
        SetAim();
        SetBlock();
        SetThrow();
        SetChangeAimType();

        SetInventory();


        SetLean();
    }
    private void Update()
    {
        SetMovementInputVector();
        SetMouseInputVector();
    }



    public void Toggle(bool enable)
    {
        _enabled = enable;

        if (enable) _playerInputs.Enable();
        else _playerInputs.Disable();
    }


    #region InputVectors
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
    #endregion

    #region Movement
    private void SetMoving()
    {
        _playerInputs.Player.Move.started += ctx => { _isMoveInput = true; };
        _playerInputs.Player.Move.canceled += ctx => { _isMoveInput = false; };
    }
    private void SetRunning()
    {
        _playerInputs.Player.Run.started += ctx =>
        {
            _isRunInput = true;
            _stateMachine.CombatControllers.EquipedWeapon.Run.IsInput = true;
        };
        _playerInputs.Player.Run.canceled += ctx =>
        {
            _isRunInput = false;
            _stateMachine.CombatControllers.EquipedWeapon.Run.IsInput = false;
        };
    }
    private void SetJump()
    {
        _playerInputs.Player.Jump.performed += ctx => _stateMachine.SwitchController.SwitchTo.Jump();
    }
    private void SetCrouch()
    {
        _playerInputs.Player.Crouch.performed += ctx => _stateMachine.SwitchController.SwitchTo.Crouch();
    }
    #endregion

    private void SetInteraction()
    {
        _playerInputs.Player.Interact.performed += ctx => _stateMachine.CoreControllers.Interaction.Interaction();
    }

    #region Combat
    private void SetEquipWeapon()
    {
        _playerInputs.Player.ChooseWeapon.performed += ctx =>
        {
            if (_stateMachine.CombatControllers.Combat.TemporaryUnEquip.IsTemporaryUnEquip) return;
            _stateMachine.CombatControllers.Combat.Equip.StartEquip((int)ctx.ReadValue<float>());
        };
    }
    private void SetUnEquipWeapon()
    {
        _playerInputs.Player.UnEquipWeapon.performed += ctx =>
        {
            if (_stateMachine.CombatControllers.Combat.TemporaryUnEquip.IsTemporaryUnEquip) return;
            _stateMachine.CombatControllers.Combat.UnEquip.StartUnEquip(1);
        };
    }
    private void SetDropWeapon()
    {
        _playerInputs.Player.DropWeapon.performed += ctx =>
        {
            if (_stateMachine.CombatControllers.Combat.TemporaryUnEquip.IsTemporaryUnEquip) return;
            _stateMachine.CombatControllers.Combat.Drop.StartDrop();
        };
    }

    private void SetChangeWeaponEquipedMode()
    {
        _playerInputs.Player.ChangeWeaponHoldMode.performed += ctx => _stateMachine.CombatControllers.EquipedWeapon.Hold.ChangeEquipedHoldMode();
    }
    private void SetChangeAimType()
    {
        _playerInputs.Player.ChangeAimType.performed += ctx => _stateMachine.CombatControllers.EquipedWeapon.Aim.ChangeAimType();
    }
    private void SetAim()
    {
        _playerInputs.Player.Aim.started += ctx =>
        {
            _stateMachine.CombatControllers.EquipedWeapon.Aim.Aim(true);
            _stateMachine.CombatControllers.EquipedWeapon.Aim.IsInput = true;
        };
        _playerInputs.Player.Aim.canceled += ctx =>
        {
            _stateMachine.CombatControllers.EquipedWeapon.Aim.Aim(false);
            _stateMachine.CombatControllers.EquipedWeapon.Aim.IsInput = false;
        };
    }
    private void SetBlock()
    {
        _playerInputs.Player.Block.started += ctx =>
        {
            _stateMachine.CombatControllers.EquipedWeapon.Block.Block(true);
            _stateMachine.CombatControllers.EquipedWeapon.Block.IsInput = true;
        };
        _playerInputs.Player.Block.canceled += ctx =>
        {
            _stateMachine.CombatControllers.EquipedWeapon.Block.Block(false);
            _stateMachine.CombatControllers.EquipedWeapon.Block.IsInput = false;
        };
    }

    private void SetThrow()
    {
        _playerInputs.Player.Throw.performed += ctx => _stateMachine.CombatControllers.Throw.ManageThrow();
    }
    #endregion

    private void SetLean()
    {
        _playerInputs.Player.LeanRight.started += ctx => _stateMachine.CombatControllers.Leaning.LeanRight();
        _playerInputs.Player.LeanLeft.started += ctx => _stateMachine.CombatControllers.Leaning.LeanLeft();


        _playerInputs.Player.LeanRight.canceled += ctx => _stateMachine.CombatControllers.Leaning.StopLeanRight();
        _playerInputs.Player.LeanLeft.canceled += ctx => _stateMachine.CombatControllers.Leaning.StopLeanLeft();
    }

    private void SetInventory()
    {
        _playerInputs.Player.Inventory.performed += ctx => _stateMachine.SwitchController.SwitchTo.Inventory();
    }






    private void OnEnable()
    {
        Toggle(true);
    }
    private void OnDisable()
    {
        Toggle(false);
    }
}
