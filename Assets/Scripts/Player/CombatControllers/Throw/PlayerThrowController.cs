using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using WeaponAnimatorNamespace;
using static PlayerAnimator.PlayerAnimatorController;
using static IkLayers.PlayerIkLayerController;
using LeftHandAnimatorNamespace;


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


        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] ThrowableStateMachine _currentThrowable;       public ThrowableStateMachine CurrentThrowable { get { return _currentThrowable; } set { _currentThrowable = value; } }
        [SerializeField] ThrowableStates _throwableState;


        public void OnExplode(ThrowableStateMachine throwableStateMachine)
        {
            if (_currentThrowable == null) return;
            if (throwableStateMachine != _currentThrowable) return;

            SetState(ThrowableStates.ExplosionInHands);

            PlayerIkLayerController playerIkLayerController = _playerStateMachine.AnimatingControllers.IkLayers;
            playerIkLayerController.ToggleLayer(LayerEnum.BakedWeaponAnimating, false, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersRightHand, false, 0.1f);
            playerIkLayerController.ToggleLayer(LayerEnum.FingersLeftHand, false, 0.1f);

            PlayerAnimatorController playerAnimatorController = _playerStateMachine.AnimatingControllers.Animator;
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowBase, false, 0.1f);
            playerAnimatorController.ToggleLayer(LayersEnum.ThrowAnimating, false, 0.1f);

            _playerStateMachine.CombatControllers.Combat.TemporaryUnEquip.RecoverFromTemporaryUnEquip();
            SetState(ThrowableStates.ReadyToThrow);
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