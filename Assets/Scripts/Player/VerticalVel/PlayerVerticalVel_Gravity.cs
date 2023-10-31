using System.Collections.Generic;
using UnityEngine;

namespace PlayerVerticalVel
{
    [System.Serializable]
    public class PlayerVerticalVel_Gravity
    {
        private PlayerVerticalVelController _verticalVelController;

        [Header("---Settings---")]
        [Range(-20, 0)][SerializeField] float _gravityForce;
        [Range(-1, 0)] [SerializeField] float _groundedGravityForce;


        [Space(20)]
        [Header("---Toggles---")]
        [SerializeField] bool _calculateGravity;
        [SerializeField] bool _applyGravity;


        [Space(20)]
        [Header("---Debugs---")]
        [SerializeField] float _currentGravityForce; public float CurrentGravityForce { get { return _currentGravityForce; } }



        public void OnAwake(PlayerVerticalVelController verticalVelController)
        {
            _verticalVelController = verticalVelController;
        }
        public void OnUpdate()
        {
            CalculateGravity();
            ApplyGravity();
        }

        private void CalculateGravity()
        {
            if (_verticalVelController.Jump.IsJump) return;

            if(!_calculateGravity)
            {
                _currentGravityForce = 0;
                return;
            }
            if (_verticalVelController.GroundCheck.IsGrounded)
            {
                _currentGravityForce = _groundedGravityForce;
                return;
            }

            _currentGravityForce += _gravityForce * Time.deltaTime;
        }
        private void ApplyGravity()
        {
            int toggleWeight = _applyGravity ? 1 : 0;
            _verticalVelController.PlayerStateMachine.CharacterController.Move(new Vector3(0, _currentGravityForce, 0) * Time.deltaTime * toggleWeight);
        }

        public void AddGravityForce(float force)
        {
            _currentGravityForce += force;
        }
    }
}