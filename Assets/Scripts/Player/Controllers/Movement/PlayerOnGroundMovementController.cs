using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundMovementController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerMovementController _movementController;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _currentMovementVector; public Vector3 CurrentMovementVector { get { return _currentMovementVector; } }
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _speed; public float Speed { get { return _speed; } }
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




    private float _animatorMovementSpeed;


    public enum MovementTypeEnum
    {
        Idle, Walk, Run
    }



    public void Movement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVectorNormalized;
        Vector3 desiredMovementVector = (_movementController.PlayerTransform.forward * inputVector.z + _movementController.PlayerTransform.right * inputVector.x) * _speed;

        _currentMovementVector = Vector3.Lerp(_currentMovementVector, desiredMovementVector, _accelarationSpeed);
        _movementController.InAir.CurrentMovementVector = _currentMovementVector;

        _movementController.CharacterController.Move(_currentMovementVector * Time.deltaTime);

        AnimatorMovement();
    }
    private void AnimatorMovement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVector;
        Vector3 animatorMovementVector = inputVector * _animatorMovementSpeed;

        _movementController.PlayerStateMachine.AnimatingControllers.Animator.SetFloat("MovementX", animatorMovementVector.x, 0.1f);
        _movementController.PlayerStateMachine.AnimatingControllers.Animator.SetFloat("MovementZ", animatorMovementVector.z, 0.1f);
    }

    public void CheckMovementType()
    {
        bool isMoveInput = _movementController.PlayerStateMachine.CoreControllers.Input.IsMoveInput;
        bool isRunInput = _movementController.PlayerStateMachine.CoreControllers.Input.IsRunInput;
        Vector3 movementInputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVector;
        bool canSwitch = _movementController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Idle)
                        || _movementController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Walk)
                        || _movementController.PlayerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Run);


        _movementType = MovementTypeEnum.Idle;
        if(canSwitch) _movementController.PlayerStateMachine.SwitchController.SwitchTo.Idle();
        if (!isMoveInput) return;


        _movementType = MovementTypeEnum.Walk;
        if (canSwitch) _movementController.PlayerStateMachine.SwitchController.SwitchTo.Walk();
        if (!isRunInput || movementInputVector.z <= 0
            || _movementController.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim
            || _movementController.PlayerStateMachine.CombatControllers.EquipedWeapon.Block.IsBlock
            || !_movementController.PlayerStateMachine.CoreControllers.Stats.Stats.Stamina.CanUseStamina) return;


        _movementType = MovementTypeEnum.Run;
        if (canSwitch) _movementController.PlayerStateMachine.SwitchController.SwitchTo.Run();
    }


    public Vector3 GetMovementDirection()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVectorNormalized;
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
