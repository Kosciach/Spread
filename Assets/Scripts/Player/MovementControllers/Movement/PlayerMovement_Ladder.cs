using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Ladder : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerMovementController _movementController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isMoving;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 1)]
    [SerializeField] float _climbDuration;


    private Vector3 _currentMovementVector; public Vector3 CurrentMovementVector { get { return _currentMovementVector; } }



    public void Movement()
    {
        if ((int)_movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVector.z == 0) return;

        if (_isMoving) return;
        _isMoving = true;


        List<Transform> ladderSteps = _movementController.PlayerStateMachine.StateControllers.Ladder.CurrentLadderController.Parts.Steps;

        _movementController.PlayerStateMachine.StateControllers.Ladder.CurrentStepIndex += (int)_movementController.PlayerStateMachine.CoreControllers.Input.MovementInputVector.z;
        if (_movementController.PlayerStateMachine.StateControllers.Ladder.CurrentStepIndex < 0 || _movementController.PlayerStateMachine.StateControllers.Ladder.CurrentStepIndex >= ladderSteps.Count)
        {
            _movementController.PlayerStateMachine.StateControllers.Ladder.CurrentStepIndex = Mathf.Clamp(_movementController.PlayerStateMachine.StateControllers.Ladder.CurrentStepIndex, 0, ladderSteps.Count - 1);
            _isMoving = false;
            return;
        }

        Vector3 moveToPosition = ladderSteps[_movementController.PlayerStateMachine.StateControllers.Ladder.CurrentStepIndex].position - _movementController.PlayerStateMachine.StateControllers.Ladder.CurrentLadderController.transform.right;
        _movementController.PlayerStateMachine.transform.LeanMove(moveToPosition, _climbDuration).setOnComplete(() =>
        {
            _isMoving = false;
        });
    }
}
