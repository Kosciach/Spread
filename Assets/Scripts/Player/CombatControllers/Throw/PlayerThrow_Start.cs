using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAnimator;
using IkLayers;
using WeaponAnimatorNamespace;
using static PlayerAnimator.PlayerAnimatorController;
using static IkLayers.PlayerIkLayerController;

namespace PlayerThrow
{
    public class PlayerThrow_Start : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerThrowController _throwController;



        public void StartThrow()
        {
            if (!_throwController.IsState(ThrowableStates.Hold)) return;

            _throwController.SetState(ThrowableStates.StartThrow);

            WeaponAnimator weaponAnimator = _throwController.PlayerStateMachine.AnimatingControllers.Weapon;
            weaponAnimator.MainTransformer.Rotate(_throwController.CurrentThrowable.ThrowableData.RightHandThrow.Rot, 0.1f);
            weaponAnimator.MainTransformer.Move(_throwController.CurrentThrowable.ThrowableData.RightHandThrow.Pos, 0.1f).SetOnMoveFinish(_throwController.Throw.Throw);

            weaponAnimator.Sway.SetWeight(0);
            weaponAnimator.Sway.Toggle(false);
            weaponAnimator.Bobbing.Toggle(false);
        }
    }
}