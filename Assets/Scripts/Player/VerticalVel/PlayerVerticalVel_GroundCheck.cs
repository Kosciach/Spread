using System.Collections.Generic;
using UnityEngine;

namespace PlayerVerticalVel
{
    [System.Serializable]
    public class PlayerVerticalVel_GroundCheck
    {
        private PlayerVerticalVelController _verticalVelController;

        [Header("---Settings---")]
        [SerializeField] Vector3 _originOffset;
        [Space(5)]
        [Range(0.1f, 1)][SerializeField] float _range;
        [Range(0.1f, 2)][SerializeField] float _radius;
        [Range(3, 50)]  [SerializeField] float _precision;
        [Space(5)]
        [SerializeField] LayerMask _mask;
        private Vector3 _origin => _verticalVelController.transform.position + _originOffset;


        [Space(20)]
        [Header("---Debugs---")]
        [SerializeField] bool _isGrounded; public bool IsGrounded { get { return _isGrounded; } }
        [SerializeField] List<bool> _groundChecks;



        public void OnAwake(PlayerVerticalVelController verticalVelController)
        {
            _verticalVelController = verticalVelController;
        }


        public void OnUpdate()
        {
            _groundChecks.Clear();

            ShootGroundCheckRay(_origin);
            
            for(int i=0; i<_precision; i++)
            {
                float angle = i * 360 / _precision;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                Vector3 origin = _origin + direction * _radius;

                ShootGroundCheckRay(origin);
            }

            _isGrounded = _groundChecks.Contains(true);
        }
        private void ShootGroundCheckRay(Vector3 pos)
        {
            Debug.DrawRay(pos, Vector3.down * _range, Color.red);
            _groundChecks.Add(Physics.Raycast(pos, Vector3.down, _range, _mask));
        }
    }
}