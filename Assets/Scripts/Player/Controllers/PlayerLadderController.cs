using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _stateMachine;



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
        _stateMachine.CameraController.SetLadderCamera(_currentLadder, true);
        _stateMachine.GravityController.ToggleApplyGravity(false);

        _stateMachine.AnimatorController.SetBool("LadderLowerEnter", true);
        transform.LeanRotateY(_currentLadder.rotation.eulerAngles.y + 90, 0.5f);
        transform.LeanMove(_currentLadder.GetChild(5).position, 0.2f);
    }
    public void LowerLadderExit()
    {
        _stateMachine.CameraController.SetLadderCamera(null, false);
        _stateMachine.GravityController.ToggleApplyGravity(true);

        _stateMachine.AnimatorController.SetBool("LadderLowerExit", true);
        _stateMachine.ColliderController.ToggleCollider(true);
        _stateMachine.SwitchController.SwitchTo.Idle();
    }




    public void HigherLadderEnter()
    {
        _stateMachine.CameraController.SetLadderCamera(_currentLadder, true);
        _stateMachine.GravityController.ToggleApplyGravity(false);



        _stateMachine.AnimatorController.SetBool("LadderHigherEnter", true);
        _stateMachine.ColliderController.ToggleCollider(false);



        Vector3 firstPosition = _currentLadder.GetChild(3).position;
        firstPosition.y = transform.position.y - 0.5f;

        transform.LeanRotateY(_currentLadder.rotation.eulerAngles.y + 90, 1f);
        transform.LeanMove(firstPosition, 0.5f).setOnComplete(() =>
        {
            transform.LeanMove(_currentLadder.GetChild(3).position, 1f).setOnComplete(() =>
            {
                _stateMachine.ColliderController.ToggleCollider(true);
            });
        });
    }
    public void HigherLadderExit()
    {
        _stateMachine.MovementController.TogglePlayerMovement(false);
        _stateMachine.AnimatorController.SetBool("LadderHigherExit", true);
        _stateMachine.ColliderController.ToggleCollider(false);

        transform.LeanMoveY(_currentLadder.GetChild(4).position.y - 1f, 0.5f).setOnComplete(() =>
        {
            transform.LeanMove(_currentLadder.GetChild(4).position, 0.5f).setOnComplete(() =>
            {
                _stateMachine.ColliderController.ToggleCollider(true);
                _stateMachine.GravityController.ToggleApplyGravity(true);
                _stateMachine.CameraController.SetLadderCamera(null, false);
                _stateMachine.MovementController.TogglePlayerMovement(true);
                _stateMachine.SwitchController.SwitchTo.Idle();
            });
        });
    }


    


    public void ResetBools()
    {
        _stateMachine.AnimatorController.SetBool("LadderHigherEnter", false);
        _stateMachine.AnimatorController.SetBool("LadderHigherExit", false);
        _stateMachine.AnimatorController.SetBool("LadderLowerEnter", false);
        _stateMachine.AnimatorController.SetBool("LadderLowerExit", false);
    }
    public LadderEnum GetLadderType()
    {
        return _ladderType;
    }






    public void CheckLadderLowerExit()
    {
        bool lowerExitLock = !_stateMachine.SwitchController.IsSwitch(PlayerStateMachine.SwitchEnum.Ladder) || !_stateMachine.GravityController.GetIsGrounded() || _stateMachine.InputController.MovementInputVector.z >= 0;
        if (lowerExitLock) return;

        _ladderType = LadderEnum.LowerExit;
        ResetBools();
        LowerLadderExit();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_lowerEnterTag) && _stateMachine.InputController.MovementInputVector.z > 0)
        {
            LadderEnter(LadderEnum.LowerEnter, other.transform);
        }
        else if (other.CompareTag(_higherEnterTag) && _stateMachine.InputController.MovementInputVector.z > 0)
        {
            LadderEnter(LadderEnum.HigherEnter, other.transform);
        }
        else if (other.CompareTag(_higherExitTag) && _stateMachine.InputController.MovementInputVector.z > 0)
        {
            _ladderType = LadderEnum.HigherExit;
            ResetBools();
            HigherLadderExit();
        }
    }
    
    private void LadderEnter(LadderEnum enterType, Transform ladder)
    {
        _currentLadder = ladder.parent;
        _stateMachine.SwitchController.SwitchTo.Ladder();
        _ladderType = enterType;
    }
}
