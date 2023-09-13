using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;

public class PlayerLadderController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;


    private bool _isEnterTop;  public bool IsEnterTop { get { return _isEnterTop; } }
    private bool _isExit;  public bool IsExit { get { return _isExit; } set { _isExit = value; } }

    private LadderController _currentLadderController; public LadderController CurrentLadderController { get { return _currentLadderController; } }





    public void OnInteract(bool isEnterTop, LadderController currentLadderController)
    {
        _isEnterTop = isEnterTop;
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

        _playerStateMachine.transform.LeanMove(pos, 0.2f);
    }

    public void CheckExit()
    {
        if (_playerStateMachine.CoreControllers.Input.MovementInputVectorNormalized.z > 0 && _playerStateMachine.transform.position.y >= _currentLadderController.transform.GetChild(2).position.y)
        {
            _isExit = true;

            _playerStateMachine.MovementControllers.Movement.Ladder.ToggleMovement(false);
            _playerStateMachine.AnimatingControllers.Animator.ToggleLayer(LayersEnum.TopBodyStabilizer, false, 0.1f);
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, false, 0.1f);
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, false, 0.1f);
            _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Head, false, 0.1f);


            _playerStateMachine.AnimatingControllers.Animator.SetTrigger("LadderExitTop", false);
            _playerStateMachine.transform.LeanMove(_currentLadderController.transform.GetChild(3).position, 1).setOnComplete(() =>
            {
                _playerStateMachine.MovementControllers.Movement.Ladder.ToggleMovement(true);
                _playerStateMachine.SwitchController.SwitchTo.Idle();
            });
        }
        else if (_playerStateMachine.CoreControllers.Input.MovementInputVectorNormalized.z < 0 && _playerStateMachine.MovementControllers.VerticalVelocity.Gravity.IsGrounded)
        {
            _isExit = true;

            _playerStateMachine.AnimatingControllers.Animator.SetTrigger("LadderExitNormal", false);
            _playerStateMachine.SwitchController.SwitchTo.Idle();
        }
    }
}