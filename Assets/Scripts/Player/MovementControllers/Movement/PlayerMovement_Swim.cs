using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Swim : MonoBehaviour
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




    private Vector3 _currentMovementVector;



    public void Movement()
    {
        Transform mainCameraTransform = _movementController.PlayerStateMachine.CameraControllers.Cine.MainCamera.transform;
        Vector3 inputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVectorNormalized;

        Vector3 desiredSwimMovementVector = (mainCameraTransform.forward  * inputVector.z + _movementController.PlayerTransform.right * inputVector.x) * _speed * _movementToggle;

        _currentMovementVector = Vector3.Lerp(_currentMovementVector, desiredSwimMovementVector, _accelarationSpeed * Time.deltaTime);
        _movementController.CharacterController.Move(_currentMovementVector * Time.deltaTime);

        _movementController.PlayerStateMachine.StateControllers.Swim.ClampPosition();

        AnimatorMovement();
    }
    private void AnimatorMovement()
    {
        Vector3 inputVector = _movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVector;
        Vector3 animatorMovementVector = inputVector * 3;

        _movementController.PlayerStateMachine.AnimatingControllers.Animator.SetFloat("SwimVelocity", animatorMovementVector.z, 0.3f);
    }



    public void ToggleMovement(bool enable)
    {
        _movementToggle = enable ? 1 : 0;
    }
}
