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
        _playerStateMachine.VerticalVelocityController.GravityController.ToggleApplyGravity(false);

        _playerStateMachine.AnimatorController.SetBool("LadderLowerEnter", true);
        transform.LeanRotateY(_currentLadder.rotation.eulerAngles.y + 90, 0.5f);
        transform.LeanMove(_currentLadder.GetChild(5).position, 0.2f);
    }
    public void LowerLadderExit()
    {
        ResetCamera();
        _playerStateMachine.VerticalVelocityController.GravityController.ToggleApplyGravity(true);

        _playerStateMachine.AnimatorController.SetBool("LadderLowerExit", true);
        _playerStateMachine.ColliderController.ToggleCollider(true);
        _playerStateMachine.SwitchController.SwitchTo.Idle();
    }




    public void HigherLadderEnter()
    {
        SetLadderCamera();
        _playerStateMachine.VerticalVelocityController.GravityController.ToggleApplyGravity(false);



        _playerStateMachine.AnimatorController.SetBool("LadderHigherEnter", true);
        _playerStateMachine.ColliderController.ToggleCollider(false);



        Vector3 firstPosition = _currentLadder.GetChild(3).position;
        firstPosition.y = transform.position.y - 0.5f;

        transform.LeanRotateY(_currentLadder.rotation.eulerAngles.y + 90, 1f);
        transform.LeanMove(firstPosition, 0.5f).setOnComplete(() =>
        {
            transform.LeanMove(_currentLadder.GetChild(3).position, 1f).setOnComplete(() =>
            {
                _playerStateMachine.ColliderController.ToggleCollider(true);
            });
        });
    }
    public void HigherLadderExit()
    {
        _playerStateMachine.MovementController.Ladder.ToggleMovement(false);
        _playerStateMachine.AnimatorController.SetBool("LadderHigherExit", true);
        _playerStateMachine.ColliderController.ToggleCollider(false);

        transform.LeanMoveY(_currentLadder.GetChild(4).position.y - 1f, 0.5f).setOnComplete(() =>
        {
            transform.LeanMove(_currentLadder.GetChild(4).position, 0.5f).setOnComplete(() =>
            {
                _playerStateMachine.ColliderController.ToggleCollider(true);
                _playerStateMachine.VerticalVelocityController.GravityController.ToggleApplyGravity(true);
                ResetCamera();
                _playerStateMachine.MovementController.Ladder.ToggleMovement(true);
                _playerStateMachine.SwitchController.SwitchTo.Idle();
            });
        });
    }


    


    public void ResetBools()
    {
        _playerStateMachine.AnimatorController.SetBool("LadderHigherEnter", false);
        _playerStateMachine.AnimatorController.SetBool("LadderHigherExit", false);
        _playerStateMachine.AnimatorController.SetBool("LadderLowerEnter", false);
        _playerStateMachine.AnimatorController.SetBool("LadderLowerExit", false);
    }
    public LadderEnum GetLadderType()
    {
        return _ladderType;
    }









    public void CheckLadderLowerExit()
    {
        bool lowerExitLock = !_playerStateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder) || !_playerStateMachine.VerticalVelocityController.GravityController.IsGrounded || _playerStateMachine.InputController.MovementInputVector.z >= 0;
        if (lowerExitLock) return;

        _ladderType = LadderEnum.LowerExit;
        ResetBools();
        LowerLadderExit();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_lowerEnterTag) && _playerStateMachine.InputController.MovementInputVector.z > 0)
        {
            LadderEnter(LadderEnum.LowerEnter, other.transform);
        }
        else if (other.CompareTag(_higherEnterTag) && _playerStateMachine.InputController.MovementInputVector.z > 0)
        {
            LadderEnter(LadderEnum.HigherEnter, other.transform);
        }
        else if (other.CompareTag(_higherExitTag) && _playerStateMachine.InputController.MovementInputVector.z > 0)
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

        _playerStateMachine.CineCameraController.HorizontalController.ToggleWrap(false);
        _playerStateMachine.CineCameraController.HorizontalController.RotateToAngle(angle, 0.2f);
        _playerStateMachine.CineCameraController.HorizontalController.SetBorderValues(minAngle, maxAngle);
    }
    private void ResetCamera()
    {
        _playerStateMachine.CineCameraController.HorizontalController.SetBorderValues(-180, 180);
        _playerStateMachine.CineCameraController.HorizontalController.ToggleWrap(true);
    }
}
