using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerState_SlopeSlide : PlayerBaseState
    {
        private float _timer;
        private Vector3 _oldVelocity;

        public PlayerState_SlopeSlide(PlayerStateMachineContext p_ctx) : base(p_ctx) { }

        public override void InitializeSubState()
        {

        }


        public override void Enter()
        {
            _timer = 0.1f;
            _ctx.AnimatorController.SlopeSlide(true);
        }

        public override void Update()
        {
            _ctx.CameraController.Look();

            if (_ctx.SlopeController.ShouldSlide)
            {
                _oldVelocity = _ctx.SlopeController.Slide();
            }
            else
            {
                _ctx.SlopeController.AfterSlide(_oldVelocity);
                _timer -= Time.deltaTime;
            }
        }

        public override void Exit()
        {
            _ctx.AnimatorController.SlopeSlide(false);
        }

        public override Type GetNextState()
        {
            if (_timer <= 0)
            {
                return typeof(PlayerState_Idle);
            }

            return GetType();
        }
    }
}