using UnityEngine;
using KosciachTools.Tween;

namespace Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;

        [Header("--References--")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Animator _topAnimator;
        [SerializeField] private Animator _bottomAnimator;


        private void Awake()
        {
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
        }

        public void SetMovementSpeed(float p_movementSpeed)
        {
            _topAnimator.SetFloat("MovementSpeed", p_movementSpeed, 0.1f, Time.deltaTime);
            _bottomAnimator.SetFloat("MovementSpeed", p_movementSpeed, 0.1f, Time.deltaTime);
        }
        public void SetMovementXY(Vector2 p_movementXY)
        {
            _bottomAnimator.SetFloat("MovementX", p_movementXY.x, 0.2f, Time.deltaTime);
            _bottomAnimator.SetFloat("MovementY", p_movementXY.y, 0.2f, Time.deltaTime);
        }
        public void SetLookX(float p_lookX)
        {
            _bottomAnimator.SetFloat("LookX", p_lookX, 0.2f, Time.deltaTime);
        }
        public void SetGravity(float p_gravity)
        {
            _topAnimator.SetFloat("Gravity", p_gravity, 0.2f, Time.deltaTime);
            _bottomAnimator.SetFloat("Gravity", p_gravity, 0.2f, Time.deltaTime);
        }
        public void Crouch(bool p_isCrouch)
        {
            _playerAnimator.ResetTrigger("CrouchEnter");
            _playerAnimator.ResetTrigger("CrouchExit");
            _playerAnimator.SetTrigger(p_isCrouch ? "CrouchEnter" : "CrouchExit");
        }
        public void Jump()
        {
            _topAnimator.SetTrigger("Jump");
            _bottomAnimator.SetTrigger("Jump");
        }
        public void Fall()
        {
            _topAnimator.SetTrigger("Fall");
            _bottomAnimator.SetTrigger("Fall");
        }
        public void Land(bool p_hard)
        {
            _playerAnimator.SetFloat("GravityAtLanding", _playerContext.GravityController.CurrentGravityForce);
            _playerAnimator.SetTrigger(p_hard ? "HardLanding" : "Land");
            _topAnimator.SetTrigger(p_hard ? "HardLanding" : "Land");
            _bottomAnimator.SetTrigger(p_hard ? "HardLanding" : "Land");
        }
        public void SlopeSlide(bool p_enter)
        {
            _topAnimator.SetTrigger(p_enter ? "SlopeSlideEnter" : "SlopeSlideExit");
            _bottomAnimator.SetTrigger(p_enter ? "SlopeSlideEnter" : "SlopeSlideExit");
        }
    }
}