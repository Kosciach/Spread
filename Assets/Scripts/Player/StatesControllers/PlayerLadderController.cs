using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLadderController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;


    private bool _isOnTop;  public bool IsOnTop { get { return _isOnTop; } }
    private LadderController _currentLadderController; public LadderController CurrentLadderController { get { return _currentLadderController; } }

    public void OnInteract(bool isOnTop, LadderController currentLadderController)
    {
        _isOnTop = isOnTop;
        _currentLadderController = currentLadderController;
        _playerStateMachine.SwitchController.SwitchTo.Ladder();
    }



    public void RotatePlayerToLadder(float rotationDuration)
    {
        float targetRotation = _currentLadderController.transform.rotation.eulerAngles.y + 90;

        _playerStateMachine.CameraControllers.Cine.ToggleCineInput(false);

        LeanTween.value(_playerStateMachine.CameraControllers.Cine.CinePOV.m_HorizontalAxis.Value, targetRotation, rotationDuration).setOnUpdate((float val) =>
        {
            _playerStateMachine.CameraControllers.Cine.CinePOV.m_HorizontalAxis.Value = val;
            _playerStateMachine.MovementControllers.Rotation.RotateToCanera();
        }).setOnComplete(() =>
        {
            _playerStateMachine.CameraControllers.Cine.Horizontal.ToggleWrap(false);
            _playerStateMachine.CameraControllers.Cine.Horizontal.SetBorderValues(targetRotation - 30, targetRotation + 30);
            this.Delay(0.1f, () => { _playerStateMachine.CameraControllers.Cine.ToggleCineInput(true); });
        });
    }
    public void RestoreCameraSettings()
    {
        _playerStateMachine.CameraControllers.Cine.Horizontal.SetBorderValues(-180, 180);
        _playerStateMachine.CameraControllers.Cine.Horizontal.ToggleWrap(true);
    }

    public void MoveToLadderNormally()
    {
        Vector3 pos = _currentLadderController.transform.GetChild(1).position;
        pos.y = _playerStateMachine.transform.position.y + 1;

        _playerStateMachine.transform.LeanMove(pos, 0.3f);
    }
    public void MoveToLadderFromTop()
    {
        Vector3 pos = _currentLadderController.transform.GetChild(1).position;

        _playerStateMachine.transform.LeanMove(pos, 0.3f);
    }

    public void CheckExit()
    {
        if (_playerStateMachine.CoreControllers.Input.MovementInputVectorNormalized.z > 0)
        {

        }
        else if (_playerStateMachine.CoreControllers.Input.MovementInputVectorNormalized.z < 0 && _playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded)
        {
            _playerStateMachine.AnimatingControllers.Animator.SetTrigger("LadderExitNormal", false);
            _playerStateMachine.SwitchController.SwitchTo.Idle();
        }
    }
}