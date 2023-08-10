using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;


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
        [SerializeField] ThrowableStateMachine _currentThrowable;           public ThrowableStateMachine CurrentThrowable { get { return _currentThrowable; } set { _currentThrowable = value; } }
        [SerializeField] bool _canThrow;                                    public bool CanThrow { get { return _canThrow; } set { _canThrow = value; } }
        [SerializeField] bool _canCancel;                                   public bool CanCancel { get { return _canCancel; } set { _canCancel = value; } }
        [SerializeField] bool _isHeld;                                      public bool IsHeld { get { return _isHeld; } set { _isHeld = value; } }
        [SerializeField] bool _isThrow;                                     public bool IsThrow { get { return _isThrow; } set { _isThrow = value; } }



        public void OnExplode(ThrowableStateMachine throwableStateMachine)
        {
            if (_currentThrowable == null) return;
            if (throwableStateMachine != _currentThrowable) return;

            _explosionInHands.ExplosionInHands();
        }
    }
}