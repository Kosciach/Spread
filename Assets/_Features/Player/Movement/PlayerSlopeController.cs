using UnityEngine;
using SaintsField;
using SaintsField.Playa;

namespace Spread.Player.Movement
{
    public class PlayerSlopeController : PlayerControllerBase
    {
        [LayoutStart("Settings", ELayout.TitleBox)]
        [LayoutStart("Settings/Angle", ELayout.TitleBox)]
        [SerializeField] private float _startSlopeSlideAngle;
        [SerializeField] private float _stopSlopeSlideAngle;
        [LayoutStart("Settings/Speed", ELayout.TitleBox)]
        [SerializeField] private float _slopeSlideStartSpeed;
        [SerializeField] private float _slopeSlideStopSpeed;
        [SerializeField] private float _slopeSlideMaxSpeedScale;

        [LayoutStart("Debug", ELayout.TitleBox | ELayout.Foldout)]
        [SerializeField, ReadOnly] private bool _isSlopeSlide; internal bool IsSlopeSlide => _isSlopeSlide;
        [SerializeField, ReadOnly] private Vector3 _slopeSlideVelocity;
        
        
        internal void SlopeSlide()
        {
            Vector3 targetSlopeSlideVel = Vector3.zero;
            float speed = _slopeSlideStopSpeed;
            _isSlopeSlide = false;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, ~LayerMask.GetMask("Player")))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                if (angle >= _startSlopeSlideAngle)
                {
                    targetSlopeSlideVel = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                    targetSlopeSlideVel *= (angle / 90f);
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

        // Go down faster, Go up slower
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
