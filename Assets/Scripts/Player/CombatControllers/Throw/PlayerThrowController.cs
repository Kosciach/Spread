using IkLayers;
using SimpleMan.CoroutineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerThrow
{
    public class PlayerThrowController : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerStateMachine _playerStateMachine;            public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
        [SerializeField] Transform _throwableHolder;                        public Transform ThrowableHolder { get { return _throwableHolder; } }
        [Space(5)]
        [SerializeField] PlayerThrow_Hold _hold;                            public PlayerThrow_Hold Hold { get { return _hold; } }
        [SerializeField] PlayerThrow_Start _start;                          public PlayerThrow_Start Start { get { return _start; } }
        [SerializeField] PlayerThrow_Throw _throw;                          public PlayerThrow_Throw Throw { get { return _throw; } }
        [SerializeField] PlayerThrow_End _end;                              public PlayerThrow_End End { get { return _end; } }
        [SerializeField] PlayerThrow_Cancel _cancel;                        public PlayerThrow_Cancel Cancel { get { return _cancel; } }
        [SerializeField] PlayerThrow_ExplosionInHands _explosionInHands;    public PlayerThrow_ExplosionInHands ExplosionInHands { get { return _explosionInHands; } }


        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] ThrowableStateMachine _currentThrowable;       public ThrowableStateMachine CurrentThrowable { get { return _currentThrowable; } set { _currentThrowable = value; } }
        [SerializeField] ThrowableStates _throwableState;


        public void OnExplode()
        {
            if (IsState(ThrowableStates.ReadyToThrow) || IsState(ThrowableStates.Throw) || IsState(ThrowableStates.EndThrow) || IsState(ThrowableStates.CancelThrow)) return;

            SetState(ThrowableStates.ExplosionInHands);
        }

        public void SetState(ThrowableStates state)
        {
            _throwableState = state;
        }
        public bool IsState(ThrowableStates state)
        {
            return _throwableState.Equals(state);
        }
    }

    public enum ThrowableStates
    {
        ReadyToThrow, Hold, StartThrow, Throw, EndThrow, CancelThrow, ExplosionInHands
    }
}