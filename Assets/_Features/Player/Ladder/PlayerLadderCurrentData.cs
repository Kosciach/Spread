using UnityEngine;
using DG.Tweening;

namespace Spread.Player.Ladder
{
    using Spread.Ladder;

    [System.Serializable]
    internal class PlayerLadderCurrentData
    {
        [SerializeField] internal Ladder CurrentLadder;
        [SerializeField] internal Tween ClimbTween;
        [SerializeField] internal bool UsingLadder;
        [SerializeField] private int _currentStep;
        [SerializeField] private int _lastStep;
        [SerializeField] internal int MaxStep;
        [SerializeField] internal int ExitDirection;
        [SerializeField] internal int ClimbDirection;

        internal int LastStep => _lastStep;
        internal int CurrentStep
        {
            get => _currentStep;
            set
            {
                _lastStep = _currentStep;
                _currentStep = value;
            }
        }


        internal void ClearUp()
        {
            CurrentLadder = null;
            UsingLadder = false;

            if (ClimbTween != null)
            {
                ClimbTween.Kill();
                ClimbTween.onComplete = null;
                ClimbTween.onUpdate = null;
                ClimbTween = null;
            }
        }
    }
}
