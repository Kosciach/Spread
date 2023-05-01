using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CharacterController _characterController;
    [SerializeField] PlayerInputController _inputController;
    [SerializeField] PlayerAnimatorController _animatorController;
    [SerializeField] PlayerStateMachine _stateMachine;
    [SerializeField] Camera _mainCamera;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _stopPlayerMovement = false;
    [SerializeField] Vector3 _movementVector;




    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _walkSpeed;
    [Range(0, 10)]
    [SerializeField] float _runSpeed;
    [Range(0, 10)]
    [SerializeField] float _crouchSpeed;
    [Range(0, 10)]
    [SerializeField] float _speed;
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _ladderSpeed;
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _swimSpeed;
    [Space(5)]
    [Range(0, 2)]
    [SerializeField] float _accelarationSpeed;



    public enum MovementDirectionEnum
    {
        Forward, Right, Backward, Left, Idle
    }
    private MovementDirectionEnum[] _directionX = { MovementDirectionEnum.Left, MovementDirectionEnum.Idle, MovementDirectionEnum.Right };
    private MovementDirectionEnum[] _directionZ = { MovementDirectionEnum.Backward, MovementDirectionEnum.Idle, MovementDirectionEnum.Forward };


    private Vector3 _animatorMovementVector; public Vector3 AnimatorMovementVector { get { return _animatorMovementVector; } }
    private float _animatorMovementSpeed;


    private Vector3 _ladderMovementVector;
    private Vector3 _swimMovementVector;


    private Vector3 _inAirMovementVector;




    public void OnGroundMovement()
    {
        if (_stopPlayerMovement) return;

        Vector3 desiredMovementVector = (transform.forward * _inputController.MovementInputVectorNormalized.z + transform.right * _inputController.MovementInputVectorNormalized.x) * _speed  * Time.deltaTime;
        _movementVector = Vector3.Lerp(_movementVector, desiredMovementVector, _accelarationSpeed);

        _characterController.Move(_movementVector);

        OnGroundAnimatorMovement();
    }
    private void OnGroundAnimatorMovement()
    {
        _animatorMovementVector = _inputController.MovementInputVector * _animatorMovementSpeed;

        _animatorController.SetFloat("MovementX", _animatorMovementVector.x, 0.2f);
        _animatorController.SetFloat("MovementZ", _animatorMovementVector.z, 0.2f);
    }



    public void LadderMovement()
    {
        Vector3 desiredLadderMovementVector = new Vector3(0f, _inputController.MovementInputVector.z * _ladderSpeed * Time.deltaTime, 0f);

        _ladderMovementVector = Vector3.Lerp(_ladderMovementVector, desiredLadderMovementVector, 10 * Time.deltaTime);
        _characterController.Move(_ladderMovementVector);

        LadderAnimatorMovement();
    }
    private void LadderAnimatorMovement()
    {
        _animatorController.SetFloat("LadderVelocity", _inputController.MovementInputVector.z * 2, 0.1f);
    }



    public void SwimMovement()
    {
        Vector3 desiredSwimMovementVector = (_mainCamera.transform.forward * _inputController.MovementInputVectorNormalized.z + transform.right * _inputController.MovementInputVectorNormalized.x) * _swimSpeed * Time.deltaTime;

        _swimMovementVector = Vector3.Lerp(_swimMovementVector, desiredSwimMovementVector, 5 * Time.deltaTime);
        _characterController.Move(_swimMovementVector);

        _stateMachine.SwimController.ClampPosition();

        SwimAnimatorMovement();
    }
    private void SwimAnimatorMovement()
    {
        _animatorController.SetFloat("SwimVelocity", _inputController.MovementInputVectorNormalized.z * 3, 0.3f);
    }


    public void InAirMovement()
    {
        Vector3 desiredMovementVector = (transform.forward * _inputController.MovementInputVectorNormalized.z + transform.right * _inputController.MovementInputVectorNormalized.x) * _speed * Time.deltaTime;
        _movementVector = Vector3.Lerp(_movementVector, desiredMovementVector, 0.01f);

        _characterController.Move(_movementVector);
    }





    public bool IsDashDirection()
    {
        if (_inputController.MovementInputVector.z < 0) return true;
        else if (_inputController.MovementInputVector.z == 0)
        {
            return _inputController.MovementInputVector.x != 0;
        }
        else return false;
    }
    public Vector3 GetOnGrroundMovementDirection()
    {
        return (transform.forward * _inputController.MovementInputVectorNormalized.z + transform.right * _inputController.MovementInputVectorNormalized.x);
    }
    public string GetOnGrroundMovementDirectionString()
    {
        string movementDirection = _directionX[(int)_inputController.MovementInputVector.x + 1].ToString() + "_" + _directionZ[(int)_inputController.MovementInputVector.z + 1].ToString();

        return movementDirection;
    }


    public void TogglePlayerMovement(bool enable)
    {
        _stopPlayerMovement = !enable;
    }
    public void SetAccelaration(float acceleration)
    {
        _accelarationSpeed = acceleration;
    }
    public void SetWalkSpeed()
    {
        _speed = _walkSpeed;
        _animatorMovementSpeed = 2;
    }
    public void SetRunSpeed()
    {
        _speed = _runSpeed;
        _animatorMovementSpeed = 6;
    }
    public void SetCrouchSpeed()
    {
        _speed = _crouchSpeed;
        _animatorMovementSpeed = 3;
    }
}
