using UnityEngine;
using KosciachTools;
using KosciachTools.Tween;

namespace PlayerStateMachineSystem
{
    public class PlayerState_Crouch : PlayerBaseState
    {
        public PlayerState_Crouch(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) { }


        public override void Enter()
        {
            _ctx.Animator.SetTrigger("EnterCrouch");
            KosciachTween.Position(_ctx.transform.GetChild(0), new Vector3(0, -0.4f, 0), 0.2f).SetLocal().SetAxis(TweenAxis.Y).Run();
        }
        public override void Update()
        {
            _ctx.Movement.Crouch.Movement();
        }
        public override void LateUpdate()
        {
            _ctx.Camera.Look.Look();
        }
        public override void CheckStateChange()
        {
            if (!_ctx.Movement.Crouch.IsCrouch) ChangeState(_factory.Idle());
            else if (_ctx.Input.IsRun)
            {
                _ctx.Movement.Crouch.DisableIsCrouch();
                ChangeState(_factory.Run());
            }
            else if (!_ctx.VerticalVel.GroundCheck.IsGrounded) ChangeState(_factory.Fall());
            else if (_ctx.VerticalVel.Jump.IsJump) ChangeState(_factory.Jump());

            _ctx.SetStateEmblem(StateEmblems.Crouch);
        }
        public override void Exit()
        {
            KosciachTween.Position(_ctx.transform.GetChild(0), new Vector3(0, 0, 0), 0.2f).SetLocal().SetAxis(TweenAxis.Y).Run();
            _ctx.Animator.SetTrigger("ExitCrouch");
        }
    }
}