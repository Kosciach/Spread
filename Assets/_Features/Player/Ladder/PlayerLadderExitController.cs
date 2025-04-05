using System;
using UnityEngine;
using DG.Tweening;

namespace Spread.Player.Ladder
{
    using NaughtyAttributes;
    using Player.StateMachine;

    [System.Serializable]
    public class PlayerLadderExitController
    {
        private PlayerStateMachineContext _ctx;
        private PlayerLadderCurrentData _currentData;
        private Action _setIksPos;
        private Action _syncIks;

        [Header("Exit"), HorizontalLine(color: EColor.Gray)]
        [SerializeField] private float _exitLadderTopDuration = 2.5f;
        [SerializeField] private float _exitLadderBottomDuration = 0.5f;

        internal void Setup(PlayerStateMachineContext p_ctx, PlayerLadderCurrentData p_currentData, Action p_setIksPos, Action p_syncIks)
        {
            _ctx = p_ctx;
            _currentData = p_currentData;
            _setIksPos = p_setIksPos;
            _syncIks = p_syncIks;
        }

        internal void ExitLadder()
        {
            if (_currentData.ExitDirection == 1)
            {
                ExitToTop();
            }
            else
            {
                ExitToBottom();
            }
        }

        private void ExitToTop()
        {
            _ctx.CameraController.RotToXAxis(0, _exitLadderBottomDuration);
            _ctx.CameraController.RotToYAxis(_currentData.CurrentLadder.transform.eulerAngles.y, _exitLadderBottomDuration);

            Vector3 start = _ctx.Transform.position;
            Vector3 middle = _currentData.CurrentLadder.TopExitPoint.position - _ctx.Transform.forward;
            Vector3 end = _currentData.CurrentLadder.TopExitPoint.position;
            Vector3[] path = new Vector3[] { start, middle, end };

            _currentData.ClimbTween = _ctx.Transform.DOPath(path, _exitLadderTopDuration).SetEase(Ease.OutCubic);
            _currentData.ClimbTween.onComplete += () =>
            {
                _ctx.CameraController.ToggleWrap(true);
                _ctx.CameraController.ResetMinMax();
                _ctx.AnimatorController.ToggleFootIk(true);
                _ctx.GravityController.ToggleGravity(true);
                _ctx.GravityController.ToggleIkCrouch(true);
                _ctx.MovementController.RootMotionMove = true;
                _ctx.CameraController.EnableInput = true;

                _currentData.ClearUp();
            };
            _currentData.ClimbTween.onUpdate += () =>
            {
                _ctx.MovementController.RestrainNormalMovement();
            };

            _ctx.CameraController.EnableInput = false;
            _ctx.AnimatorController.LadderExit(true);
            _ctx.AnimatorController.SetLadderRig(0, _exitLadderBottomDuration);
        }

        private void ExitToBottom()
        {
            _ctx.CameraController.RotToXAxis(0, _exitLadderBottomDuration);
            _ctx.CameraController.RotToYAxis(_currentData.CurrentLadder.transform.eulerAngles.y, _exitLadderBottomDuration);

            _currentData.ClimbTween = _ctx.Transform.DOMove(_currentData.CurrentLadder.BottomExitPoint.position, _exitLadderBottomDuration);
            _currentData.ClimbTween.onComplete += () =>
            {
                _ctx.CameraController.ToggleWrap(true);
                _ctx.CameraController.ResetMinMax();
                _ctx.AnimatorController.ToggleFootIk(true);
                _ctx.GravityController.ToggleGravity(true);
                _ctx.GravityController.ToggleIkCrouch(true);
                _ctx.MovementController.RootMotionMove = true;

                _currentData.ClearUp();
            };

            _ctx.AnimatorController.LadderExit(false);
            _ctx.AnimatorController.SetLadderRig(0, _exitLadderBottomDuration);
        }
    }
}
