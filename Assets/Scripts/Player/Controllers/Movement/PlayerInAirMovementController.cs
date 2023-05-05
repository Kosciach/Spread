using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class PlayerInAirMovementController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerMovementController _movementController;



    [Space(20)]
    [Header("====Debugs====")]
    [Range(0, 1)]
    [SerializeField] int _movementToggle;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _speed;
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] float _accelarationSpeed;




    private Vector3 _currentMovementVector; public Vector3 CurrentMovementVector { get { return _currentMovementVector; } }



    public void Movement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.InputController.MovementInputVectorNormalized;

        Vector3 desiredMovementVector = (_movementController.PlayerTransform.forward * inputVector.z + _movementController.PlayerTransform.right * inputVector.x) * _speed * _movementToggle * Time.deltaTime;
        _currentMovementVector = Vector3.Lerp(_currentMovementVector, desiredMovementVector, _accelarationSpeed * Time.deltaTime);

        _movementController.CharacterController.Move(_currentMovementVector);
    }


    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetCurrentMovementVector(Vector3 currentMovementVector)
    {
        _currentMovementVector = currentMovementVector;
    }

    public void ToggleMovement(bool enable)
    {
        _movementToggle = enable ? 1 : 0;
    }
}
