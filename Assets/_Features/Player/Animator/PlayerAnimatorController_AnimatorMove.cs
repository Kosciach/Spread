using System;
using UnityEngine;

namespace Spread.Player.Animating
{
    public class PlayerAnimatorController_AnimatorMove : MonoBehaviour
    {
        internal Action OnAnimatorMoveEvent;

        private void OnAnimatorMove()
        {
            OnAnimatorMoveEvent?.Invoke();
        }
    }
}