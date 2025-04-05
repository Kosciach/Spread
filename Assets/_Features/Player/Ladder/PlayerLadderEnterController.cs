using System;
using UnityEngine;
using DG.Tweening;

namespace Spread.Player.Ladder
{
    using Tools;
    using Player.StateMachine;
    using NaughtyAttributes;

    [System.Serializable]
    public class PlayerLadderEnterController
    {
        private PlayerStateMachineContext _ctx;
        private PlayerLadderCurrentData _currentData;
        private Action _setIksPos;
        private Action _syncIks;

        [Header("Enter"), HorizontalLine(color: EColor.Gray)]
        [SerializeField] private int _ladderStepIndexTopOffset;
        [SerializeField] private float _enterLadderTopPart1Duration = 0.5f;
        [SerializeField] private float _enterLadderTopPart2Duration = 2f;
        [SerializeField] private float _enterLadderDuration = 0.2f;

        internal void Setup(PlayerStateMachineContext p_ctx, PlayerLadderCurrentData p_currentData, Action p_setIksPos, Action p_syncIks)
        {
            _ctx = p_ctx;
            _currentData = p_currentData;
            _setIksPos = p_setIksPos;
            _syncIks = p_syncIks;
        }

        internal void EnterLadder()
        {
            float closestDistance = 1000;
            _currentData.CurrentStep = 0;

            for (int i = 0; i < _currentData.CurrentLadder.Steps.Count; i++)
            {
                Vector3 step = _currentData.CurrentLadder.Steps[i];
                float distance = Vector3.Distance(_ctx.Transform.position, step);
                if (distance <= closestDistance)
                {
                    _currentData.CurrentStep = i;
                    closestDistance = distance;
                }
            }

            _currentData.MaxStep = _currentData.CurrentLadder.Steps.Count - 1 - _ladderStepIndexTopOffset;
            _currentData.CurrentStep = Mathf.Clamp(_currentData.CurrentStep, 0, _currentData.MaxStep);

            if (_currentData.CurrentLadder.IsTop(_ctx.Transform))
            {
                EnterFromTop();
            }
            else
            {
                EnterFromBottom();
            }
        }

        private void EnterFromTop()
        {
            _ctx.InteractionsController.SetInteractable(null);
            _ctx.MovementController.RootMotionMove = false;

            float angle = _currentData.CurrentLadder.transform.eulerAngles.y;
            _ctx.RotToYAxis(angle, _enterLadderTopPart1Duration);
            _ctx.CameraController.RotToYAxis(angle, _enterLadderTopPart1Duration);
            _ctx.CameraController.RotToXAxis(25, _enterLadderTopPart1Duration);
            _ctx.CameraController.EnableInput = false;

            _currentData.ClimbTween = _ctx.Transform.DOMove(_currentData.CurrentLadder.TopExitPoint.position - _currentData.CurrentLadder.transform.forward / 2, _enterLadderTopPart1Duration);
            _currentData.ClimbTween.onComplete += () =>
            {
                _ctx.AnimatorController.ToggleFootIk(false);
                _ctx.AnimatorController.LadderEnter(true);
                _ctx.GravityController.ToggleGravity(false);
                _ctx.GravityController.ToggleIkCrouch(false);
                _setIksPos?.Invoke();

                Helpers.SimpleTimer(_enterLadderTopPart2Duration * 0.8f, () =>
                {
                    _ctx.AnimatorController.SetLadderRig(1, 0.5f);
                });

                Vector3 start = _ctx.Transform.position;
                Vector3 middle = start - _ctx.Transform.forward * 0.5f;
                Vector3 end = _currentData.CurrentLadder.Steps[_currentData.CurrentStep];
                Vector3[] path = new Vector3[] { start, middle, end };

                _currentData.ClimbTween = _ctx.Transform.DOPath(path, _enterLadderTopPart2Duration).SetEase(Ease.InCubic);
                _currentData.ClimbTween.onComplete += () =>
                {
                    _ctx.CameraController.ToggleWrap(false);
                    _ctx.CameraController.SetMinMax(angle, 110);
                    _ctx.CameraController.EnableInput = true;

                    _currentData.ClimbTween = _ctx.Transform.DOMove(_currentData.CurrentLadder.Steps[_currentData.CurrentStep], _enterLadderDuration);
                    _currentData.ClimbTween.onComplete += () =>
                    {
                        _currentData.UsingLadder = true;

                        _currentData.ClimbTween.onComplete = null;
                        _currentData.ClimbTween = null;
                    };
                    _currentData.ClimbTween.onUpdate += () =>
                    {
                        _syncIks?.Invoke();
                    };
                };
                _currentData.ClimbTween.onUpdate += () =>
                {
                    _syncIks?.Invoke();
                };
            };
        }

        private void EnterFromBottom()
        {
            _ctx.InteractionsController.SetInteractable(null);
            _ctx.AnimatorController.ToggleFootIk(false);
            _ctx.AnimatorController.SetLadderRig(1);
            _ctx.AnimatorController.LadderEnter(false);
            _ctx.GravityController.ToggleGravity(false);
            _ctx.GravityController.ToggleIkCrouch(false);
            _ctx.MovementController.RootMotionMove = false;

            float angle = _currentData.CurrentLadder.transform.eulerAngles.y;
            _ctx.RotToYAxis(angle, _enterLadderDuration);
            _ctx.CameraController.RotToYAxis(angle, _enterLadderDuration);

            _currentData.ClimbTween = _ctx.Transform.DOMove(_currentData.CurrentLadder.Steps[_currentData.CurrentStep], _enterLadderDuration);
            _currentData.ClimbTween.onComplete += () =>
            {
                _ctx.CameraController.ToggleWrap(false);
                _ctx.CameraController.SetMinMax(angle, 110);
                _currentData.UsingLadder = true;

                _currentData.ClimbTween.onComplete = null;
                _currentData.ClimbTween = null;
            };
            _currentData.ClimbTween.onUpdate += () =>
            {
                _syncIks?.Invoke();
            };

            _setIksPos?.Invoke();
        }

    }
}
