using NaughtyAttributes;
using UnityEngine;

namespace Spread.Player.Movement
{
    using StateMachine;

    public class PlayerSlopeController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;

        [BoxGroup("Settings"), SerializeField] private float _startSlopeSlideAngle;
        [BoxGroup("Settings"), SerializeField] private float _stopSlopeSlideAngle;
        [Space(10)]
        [BoxGroup("Settings"), SerializeField] private float _slopeSlideStartSpeed;
        [BoxGroup("Settings"), SerializeField] private float _slopeSlideStopSpeed;
        [Space(10)]
        [BoxGroup("Settings"), SerializeField] private float _slopeSlideMaxSpeedScale;

        [Foldout("Debug"), SerializeField, ReadOnly] private bool _isSlopeSlide; internal bool IsSlopeSlide => _isSlopeSlide;
        [Foldout("Debug"), SerializeField, ReadOnly] private Vector3 _slopeSlideVelocity;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;
        }

        private void Update()
        {
            SlopeSlide();
        }

        private void SlopeSlide()
        {
            Vector3 targetSlopeSlideVel = Vector3.zero;
            float angle = 0;
            float speed = _slopeSlideStopSpeed;
            _isSlopeSlide = false;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, ~LayerMask.GetMask("Player")))
            {
                angle = Vector3.Angle(Vector3.up, hit.normal);
                if (angle >= _startSlopeSlideAngle)
                {
                    targetSlopeSlideVel = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                    targetSlopeSlideVel = targetSlopeSlideVel * (angle / 90f);
                    speed = _slopeSlideStartSpeed;
                    _isSlopeSlide = true;
                }
                else if (angle < _stopSlopeSlideAngle)
                {
                    targetSlopeSlideVel = Vector3.zero;
                    speed = _slopeSlideStopSpeed;
                    _isSlopeSlide = false;
                }
            }

            _slopeSlideVelocity = Vector3.Lerp(_slopeSlideVelocity, targetSlopeSlideVel * _slopeSlideMaxSpeedScale, speed * Time.deltaTime);
            _ctx.CharacterController.Move(_slopeSlideVelocity * Time.deltaTime);
        }

        internal Vector3 GetSlopeVelocity(Vector3 p_velocity)
        {
            Vector3 velocity = p_velocity;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, ~LayerMask.GetMask("Player")))
            {
                Quaternion slopeRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Vector3 slopeVel = slopeRot * velocity;
                if (slopeVel.y != 0)
                {
                    velocity = slopeVel;
                }
            }

            return velocity;
        }
    }
}
