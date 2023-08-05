using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Transform _currentLadder;
    [SerializeField] bool _exitedTopLadder;
    [SerializeField] LadderEnum _ladderType;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] string _higherEnterTag;
    [SerializeField] string _higherExitTag;
    [SerializeField] string _lowerEnterTag;


    public enum LadderEnum
    { 
        LowerEnter, LowerExit,
        HigherEnter, HigherExit
    }





    public void LowerLadderEnter()
    {
        SetLadderCamera();
        _playerStateMachine.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(false);

        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderLowerEnter", true);
        transform.LeanRotateY(_currentLadder.rotation.eulerAngles.y + 90, 0.5f);
        transform.LeanMove(_currentLadder.GetChild(5).position, 0.2f);
    }
    public void LowerLadderExit()
    {
        ResetCamera();
        _playerStateMachine.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(true);

        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderLowerExit", true);
        _playerStateMachine.CoreControllers.Collider.ToggleCollider(true);
        _playerStateMachine.SwitchController.SwitchTo.Idle();
    }




    public void HigherLadderEnter()
    {
        SetLadderCamera();
        _playerStateMachine.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(false);



        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderHigherEnter", true);
        _playerStateMachine.CoreControllers.Collider.ToggleCollider(false);



        Vector3 firstPosition = _currentLadder.GetChild(3).position;
        firstPosition.y = transform.position.y - 0.5f;

        transform.LeanRotateY(_currentLadder.rotation.eulerAngles.y + 90, 1f);
        transform.LeanMove(firstPosition, 0.5f).setOnComplete(() =>
        {
            transform.LeanMove(_currentLadder.GetChild(3).position, 1f).setOnComplete(() =>
            {
                _playerStateMachine.CoreControllers.Collider.ToggleCollider(true);
            });
        });
    }
    public void HigherLadderExit()
    {
        _playerStateMachine.CameraControllers.Cine.ToggleCineInput(false);
        SetLadderCamera();

        _playerStateMachine.MovementControllers.Movement.Ladder.ToggleMovement(false);
        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderHigherExit", true);
        _playerStateMachine.CoreControllers.Collider.ToggleCollider(false);

        transform.LeanMoveY(_currentLadder.GetChild(4).position.y - 1f, 0.5f).setOnComplete(() =>
        {
            transform.LeanMove(_currentLadder.GetChild(4).position, 0.5f).setOnComplete(() =>
            {
                _playerStateMachine.CoreControllers.Collider.ToggleCollider(true);
                _playerStateMachine.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(true);
                ResetCamera();
                _playerStateMachine.MovementControllers.Movement.Ladder.ToggleMovement(true);
                _playerStateMachine.SwitchController.SwitchTo.Idle();
                _playerStateMachine.CameraControllers.Cine.ToggleCineInput(true);
            });
        });
    }


    


    public void ResetBools()
    {
        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderHigherEnter", false);
        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderHigherExit", false);
        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderLowerEnter", false);
        _playerStateMachine.AnimatingControllers.Animator.SetBool("LadderLowerExit", false);
    }
    public LadderEnum GetLadderType()
    {
        return _ladderType;
    }









    public void CheckLadderLowerExit()
    {
        bool lowerExitLock = !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder) || !_playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded || _playerStateMachine.CoreControllers.Input.MovementInputVector.z >= 0;
        if (lowerExitLock) return;

        _ladderType = LadderEnum.LowerExit;
        ResetBools();
        LowerLadderExit();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_lowerEnterTag) && _playerStateMachine.CoreControllers.Input.MovementInputVector.z > 0)
        {
            LadderEnter(LadderEnum.LowerEnter, other.transform);
        }
        else if (other.CompareTag(_higherEnterTag) && _playerStateMachine.CoreControllers.Input.MovementInputVector.z > 0)
        {
            LadderEnter(LadderEnum.HigherEnter, other.transform);
        }
        else if (other.CompareTag(_higherExitTag) && _playerStateMachine.MovementControllers.Movement.Ladder.CurrentMovementController.y > 0)
        {
            _ladderType = LadderEnum.HigherExit;
            ResetBools();
            HigherLadderExit();
        }
    }
    
    private void LadderEnter(LadderEnum enterType, Transform ladder)
    {
        _currentLadder = ladder.parent;
        _playerStateMachine.SwitchController.SwitchTo.Ladder();
        _ladderType = enterType;
    }
    private void SetLadderCamera()
    {
        float angle = 0;
        float minAngle = 0; float maxAngle = 0;

        if (_currentLadder == null) return;

        angle = _currentLadder.rotation.eulerAngles.y + 90;
        minAngle = angle - 50;
        maxAngle = angle + 50;


        _playerStateMachine.CameraControllers.Cine.Horizontal.RotateToAngle(angle, 0.2f);
        _playerStateMachine.CameraControllers.Cine.Horizontal.SetBorderValues(minAngle, maxAngle);
        _playerStateMachine.CameraControllers.Cine.Horizontal.ToggleWrap(false);
    }
    private void ResetCamera()
    {
        _playerStateMachine.CameraControllers.Cine.Horizontal.SetBorderValues(-180, 180);
        _playerStateMachine.CameraControllers.Cine.Horizontal.ToggleWrap(true);
    }
}
