using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Ladder : MonoBehaviour
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
    [Range(0, 10)]
    [SerializeField] float _accelarationSpeed;




    private Vector3 _currentMovementVector; public Vector3 CurrentMovementController { get { return _currentMovementVector; } }



    public void Movement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVectorNormalized;
        Vector3 desiredLadderMovementVector = new Vector3(0f, inputVector.z * _speed * _movementToggle, 0f);

        _currentMovementVector = Vector3.Lerp(_currentMovementVector, desiredLadderMovementVector, _accelarationSpeed * Time.deltaTime);
        _movementController.CharacterController.Move(_currentMovementVector * Time.deltaTime);

        AnimatorMovement();
    }
    private void AnimatorMovement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVector;
        Vector3 animatorMovementVector = inputVector * 2 * _movementToggle;

        _movementController.PlayerStateMachine.AnimatingControllers.Animator.SetFloat("LadderVelocity", animatorMovementVector.z, 0.1f);
    }




    public void ToggleMovement(bool enable)
    {
        _movementToggle = enable ? 1 : 0;
    }
}