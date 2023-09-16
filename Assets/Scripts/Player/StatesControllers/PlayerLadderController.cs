using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using System;

public class PlayerLadderController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;

    private LadderController _currentLadderController;      public LadderController CurrentLadderController { get { return _currentLadderController; } }
    private Action _enter;                                  public Action Enter { get { return _enter; } }
    private bool _isEnterTop;                               public bool IsEnterTop { get { return _isEnterTop; } }
    private bool _isExit;                                   public bool IsExit { get { return _isExit; } set { _isExit = value; } }
    private int _currentStepIndex;                          public int CurrentStepIndex { get { return _currentStepIndex; } set { _currentStepIndex = value; } }



    public void OnInteract(bool isEnterTop, LadderController currentLadderController)
    {
        _isEnterTop = isEnterTop;
        _currentLadderController = currentLadderController;
        _enter = isEnterTop ? EnterTop : EnterNormally;
        _playerStateMachine.SwitchController.SwitchTo.Ladder();
    }

    private void EnterNormally()
    {
        _playerStateMachine.CoreControllers.Collider.ToggleCollider(false);
        _playerStateMachine.MovementControllers.VerticalVelocity.Gravity.ToggleApplyGravity(false);
        _playerStateMachine.CameraControllers.HeadClippingCorrector.Toggle(false);

        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.SpineLock, false, 0.3f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Body, false, 0.3f);
        _playerStateMachine.AnimatingControllers.IkLayers.ToggleLayer(LayerEnum.Head, false, 0.3f);

        _currentStepIndex = GetClosestStepIndex();
        Vector3 moveToPosition = _currentLadderController.Parts.Steps[_currentStepIndex].position - _currentLadderController.transform.right;
        _playerStateMachine.transform.LeanMove(moveToPosition, 0.5f).setEaseInOutCubic();

        float targetPlayerRotationY = _currentLadderController.transform.eulerAngles.y + 90;
        _playerStateMachine.CameraControllers.Cine.ToggleCineInput(false);
        _playerStateMachine.CameraControllers.Cine.Vertical.RotateToAngleLT(-50, 0.3f);
        LeanTween.value(_playerStateMachine.CameraControllers.Cine.CinePOV.m_HorizontalAxis.Value, targetPlayerRotationY, 0.4f).setEaseInOutCubic().setOnUpdate((float val) =>
        {
            _playerStateMachine.CameraControllers.Cine.CinePOV.m_HorizontalAxis.Value = val;
            _playerStateMachine.MovementControllers.Rotation.RotateToCanera();
        }).setOnComplete(() =>
        {
            _playerStateMachine.CameraControllers.Cine.Horizontal.ToggleWrap(false);
            _playerStateMachine.CameraControllers.Cine.Horizontal.SetBorderValues(targetPlayerRotationY-45, targetPlayerRotationY+45);
            _playerStateMachine.CameraControllers.Cine.Vertical.SetBorderValues(-70, 45);
            _playerStateMachine.CameraControllers.Cine.ToggleCineInput(true);
        });
    }
    private void EnterTop()
    {
        
    }



    private int GetClosestStepIndex()
    {
        int closestStepIndex = 0;
        float smallestStepDiffrence = 1000;

        for(int i=0; i<_currentLadderController.Parts.Steps.Count; i++)
        {
            Transform currentStep = _currentLadderController.Parts.Steps[i];
            float currentStepDiff = Vector3.Distance(_playerStateMachine.transform.position, currentStep.position);

            if(currentStepDiff < smallestStepDiffrence)
            {
                smallestStepDiffrence = currentStepDiff;
                closestStepIndex = i;
            }
        }

        return closestStepIndex;
    }
}