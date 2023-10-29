using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    public static Action OnJump;


    [Header("---Movement----")]
    [SerializeField] Vector2 _movementInputVector; public Vector2 MovementInputVector { get { return _movementInputVector; } }
    [SerializeField] bool _isWalk; public bool IsWalk { get { return _isWalk; } }
    [SerializeField] bool _isRun; public bool IsRun { get { return _isRun; } }


    [Space(20)]
    [Header("---Mouse----")]
    [SerializeField] Vector2 _mouseInputVector; public Vector2 MouseInputVector { get { return _mouseInputVector; } }




    private void Awake() => _playerInputs = new PlayerInputs();
    private void Update()
    {
        SetMovementInputVector();
        SetIsWalk();
        SetIsRun();
        TriggerJump();

        SetMouseInputVector();
    }
    private void OnEnable() => _playerInputs.Enable();
    private void OnDisable() => _playerInputs.Disable();


    #region ---Movement---
    private void SetMovementInputVector()
    {
        _movementInputVector = _playerInputs.Movement.Move.ReadValue<Vector2>();
    }
    private void SetIsWalk()
    {
        _isWalk = _movementInputVector.magnitude > 0;
    }
    private void SetIsRun()
    {
        _playerInputs.Movement.Run.started += ctx => { _isRun = true; };
        _playerInputs.Movement.Run.canceled += ctx => { _isRun = false; };
    }
    private void TriggerJump()
    {
        _playerInputs.Movement.Jump.performed += ctx => OnJump?.Invoke();
    }
    #endregion

    #region ---Mouse---
    private void SetMouseInputVector()
    {
        _mouseInputVector = _playerInputs.Mouse.Look.ReadValue<Vector2>();
    }
    #endregion
}
