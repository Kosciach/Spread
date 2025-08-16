using UnityEngine;

namespace Spread.Player.Movement
{
    public class SlopeData
    {
        private float _angle;
        private Vector3 _direction;
        private Vector3 _normal;

        internal float Angle => _angle;
        internal Vector3 Direction => _direction;
        internal Vector3 Normal => _normal;

        public SlopeData(float p_angle, Vector3 p_direction, Vector3 p_normal)
        {
            _angle = p_angle;
            _direction = p_direction;
            _normal = p_normal;
        }
    }
}
