using System;
using UnityEngine;
using DG.Tweening;

namespace Spread.Player.Ladder
{
    using NaughtyAttributes;
    using Player.StateMachine;

    [System.Serializable]
    public class PlayerLadderClimbController
    {
        private PlayerStateMachineContext _ctx;
        private PlayerLadderCurrentData _currentData;
        private Action _setIksPos;
        private Action _syncIks;

        [Header("Climb"), HorizontalLine(color: EColor.Gray)]
        [SerializeField] private Ease _ikMoveEase = Ease.InOutSine;
        [SerializeField] private Ease _climbEase = Ease.InOutSine;
        [SerializeField] private float _climbDelay = 0.1f;
        [SerializeField] private float _ikMoveDuration = 0.2f;
        [SerializeField] private float _climbDuration = 0.2f;
        [SerializeField] private float _stepYLegOffset = 0.1f;
        [SerializeField] private float _stepYArmOffset = -0.2f;

        internal float StepYLegOffset => _stepYLegOffset;
        internal float StepYArmOffset => _stepYArmOffset;

        private float _climbDelayTimer;

        internal void Setup(PlayerStateMachineContext p_ctx, PlayerLadderCurrentData p_currentData, Action p_setIksPos, Action p_syncIks)
        {
            _ctx = p_ctx;
            _currentData = p_currentData;
            _setIksPos = p_setIksPos;
            _syncIks = p_syncIks;
        }

        internal void ClimbLadder()
        {
            _syncIks?.Invoke();

            _climbDelayTimer = Mathf.Max(0, _climbDelayTimer - Time.deltaTime);

            if (_currentData.ClimbTween != null || _currentData.ClimbDirection == 0 || _climbDelayTimer > 0) return;

            _climbDelayTimer = _climbDelay;

            int step = _currentData.CurrentStep + _currentData.ClimbDirection;
            _currentData.CurrentStep = Mathf.Clamp(step, 0, _currentData.MaxStep);

            if (_currentData.ClimbDirection == 1)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
        }

        private void MoveUp()
        {
            if (_currentData.CurrentStep >= _currentData.MaxStep && _currentData.LastStep >= _currentData.MaxStep)
            {
                _currentData.ExitDirection = 1;
                _currentData.UsingLadder = false;
                return;
            }

            Transform leg = _currentData.CurrentStep % 2 == 0 ? _currentData.CurrentLadder.IkLegL : _currentData.CurrentLadder.IkLegR;
            _currentData.ClimbTween = leg.DOMoveY(_currentData.CurrentLadder.Handles[_currentData.CurrentStep + 1].y + _stepYLegOffset, _ikMoveDuration);
            _currentData.ClimbTween.SetEase(_ikMoveEase);

            bool rightArm = _currentData.CurrentStep % 2 == 0;
            Transform arm = rightArm ? _currentData.CurrentLadder.IkArmR : _currentData.CurrentLadder.IkArmL;
            float yPos = (_currentData.CurrentLadder.Handles[_currentData.CurrentStep + 5].y + _stepYArmOffset);

            Vector3 start = arm.position;
            Vector3 middle = new Vector3(arm.position.x, (arm.position.y + yPos) / 2f, arm.position.z) + (_ctx.Transform.right * 0.2f * (rightArm ? 1 : -1));
            Vector3 end = new Vector3(arm.position.x, yPos, arm.position.z);
            Vector3[] path = new Vector3[] { start, middle, end };

            _currentData.ClimbTween = arm.DOPath(path, _ikMoveDuration);
            _currentData.ClimbTween.SetEase(_ikMoveEase);

            _currentData.ClimbTween.onComplete += () =>
            {
                Vector3 target = _currentData.CurrentLadder.Steps[_currentData.CurrentStep];
                _currentData.ClimbTween = _ctx.Transform.DOMove(target, _climbDuration);
                _currentData.ClimbTween.SetEase(_climbEase);
                _currentData.ClimbTween.onComplete += () =>
                {
                    _currentData.ClimbTween.onComplete = null;
                    _currentData.ClimbTween = null;
                };
            };
        }

        private void MoveDown()
        {
            if(_currentData.CurrentStep == 0 && _currentData.LastStep == 0)
            {
                _currentData.ExitDirection = -1;
                _currentData.UsingLadder = false;
                return;
            }

            Vector3 target = _currentData.CurrentLadder.Steps[_currentData.CurrentStep];
            _currentData.ClimbTween = _ctx.Transform.DOMove(target, _climbDuration);
            _currentData.ClimbTween.SetEase(_climbEase);
            _currentData.ClimbTween.onComplete += () =>
            {
                Transform leg = _currentData.CurrentStep % 2 == 0 ? _currentData.CurrentLadder.IkLegR : _currentData.CurrentLadder.IkLegL;
                _currentData.ClimbTween = leg.DOMoveY(_currentData.CurrentLadder.Handles[_currentData.CurrentStep].y + _stepYLegOffset, _ikMoveDuration);
                _currentData.ClimbTween.SetEase(_ikMoveEase);

                bool rightArm = _currentData.CurrentStep % 2 != 0;
                Transform arm = rightArm ? _currentData.CurrentLadder.IkArmR : _currentData.CurrentLadder.IkArmL;
                float yPos = (_currentData.CurrentLadder.Handles[_currentData.CurrentStep + 4].y + _stepYArmOffset);

                Vector3 start = arm.position;
                Vector3 middle = new Vector3(arm.position.x, (arm.position.y + yPos) / 2f, arm.position.z) + (_ctx.Transform.right * 0.2f * (rightArm ? 1 : -1));
                Vector3 end = new Vector3(arm.position.x, yPos, arm.position.z);
                Vector3[] path = new Vector3[] { start, middle, end };

                _currentData.ClimbTween = arm.DOPath(path, _ikMoveDuration);
                _currentData.ClimbTween.SetEase(_ikMoveEase);

                _currentData.ClimbTween.onComplete += () =>
                {
                    _currentData.ClimbTween.onComplete = null;
                    _currentData.ClimbTween = null;
                };
            };
        }
    }
}
