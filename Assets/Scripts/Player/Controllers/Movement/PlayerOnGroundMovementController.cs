using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundMovementController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerMovementController _movementController;



    [Space(20)]
    [Header("====Debugs====")]
    [Range(0, 10)]
    [SerializeField] float _speed;
    [Range(0, 1)]
    [SerializeField] int _movementToggle;
    [Space(5)]
    [SerializeField] MovementTypeEnum _movementType;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _crouchSpeed;
    [Range(0, 10)]
    [SerializeField] float _walkSpeed;
    [Range(0, 10)]
    [SerializeField] float _runSpeed;
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _accelarationSpeed;


    [SerializeField] Vector3 _currentMovementVector; public Vector3 CurrentMovementVector { get { return _currentMovementVector; } }
    private float _animatorMovementSpeed;


    public enum MovementTypeEnum
    {
        Idle, Walk, Run
    }



    public void Movement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.InputController.MovementInputVectorNormalized;
        Vector3 desiredMovementVector = (_movementController.PlayerTransform.forward * inputVector.z + _movementController.PlayerTransform.right * inputVector.x) * _speed * Time.deltaTime;

        _currentMovementVector = Vector3.Lerp(_currentMovementVector, desiredMovementVector, _accelarationSpeed);
        _movementController.InAir.SetCurrentMovementVector(_currentMovementVector);

        _movementController.CharacterController.Move(_currentMovementVector);

        AnimatorMovement();
    }
    private void AnimatorMovement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.InputController.MovementInputVector;
        Vector3 animatorMovementVector = inputVector * _animatorMovementSpeed;

        _movementController.PlayerStateMachine.AnimatorController.SetFloat("MovementX", animatorMovementVector.x, 0.1f);
        _movementController.PlayerStateMachine.AnimatorController.SetFloat("MovementZ", animatorMovementVector.z, 0.2f);
    }

    public void CheckMovementType()
    {
        bool isMoveInput = _movementController.PlayerStateMachine.InputController.IsMoveInput;
        bool isRunInput = _movementController.PlayerStateMachine.InputController.IsRunInput;
        Vector3 movementInputVector = _movementController.PlayerStateMachine.InputController.MovementInputVector;
        bool canSwitch = _movementController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
                        || _movementController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
                        || _movementController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run);


        _movementType = MovementTypeEnum.Idle;
        if(canSwitch) _movementController.PlayerStateMachine.SwitchController.SwitchTo.Idle();
        if (!isMoveInput) return;

        _movementType = MovementTypeEnum.Walk;
        if (canSwitch) _movementController.PlayerStateMachine.SwitchController.SwitchTo.Walk();
        if (!isRunInput || movementInputVector.z <= 0) return;

        _movementType = MovementTypeEnum.Run;
        if (canSwitch) _movementController.PlayerStateMachine.SwitchController.SwitchTo.Run();
    }


    public Vector3 GetMovementDirection()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.InputController.MovementInputVectorNormalized;
        return (_movementController.PlayerTransform.forward * inputVector.z + _movementController.PlayerTransform.right * inputVector.x);
    }





    public void SetCrouchSpeed()
    {
        _speed = _crouchSpeed;
        _animatorMovementSpeed = 3;
        _movementController.InAir.SetSpeed(_speed);
    }
    public void SetWalkSpeed()
    {
        _speed = _walkSpeed;
        _animatorMovementSpeed = 3;
        _movementController.InAir.SetSpeed(_speed);
    }
    public void SetRunSpeed()
    {
        _speed = _runSpeed;
        _animatorMovementSpeed = 6;
        _movementController.InAir.SetSpeed(_speed);
    }

    public void ToggleMovement(bool enable)
    {
        _movementToggle = enable ? 1 : 0;
    }
}
