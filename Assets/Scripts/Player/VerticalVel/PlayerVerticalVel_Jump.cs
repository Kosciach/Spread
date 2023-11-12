using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerVerticalVel
{
    [System.Serializable]
    public class PlayerVerticalVel_Jump
    {
        private PlayerVerticalVelController _verticalVelController;

        [Header("---Settings---")]
        [Range(3, 20)][SerializeField] float _jumpForce;


        [Space(20)]
        [Header("---Debugs---")]
        [SerializeField] bool _isJump; public bool IsJump { get { return _isJump; } }


        public void OnAwake(PlayerVerticalVelController verticalVelController)
        {
            _verticalVelController = verticalVelController;
        }
        public void OnEnable() => PlayerInputController.OnJump += SetIsJump;
        public void OnDisable() => PlayerInputController.OnJump -= SetIsJump;


        private void SetIsJump()
        {
            if (!_verticalVelController.GroundCheck.IsGrounded) return;

            _isJump = true;
            _verticalVelController.StartCoroutine(ResetIsJump());
        }
        private IEnumerator ResetIsJump()
        {
            yield return new WaitForSeconds(0.1f);
            _isJump = false;
        }
        public void ApplyJumpForce()
        {
            _verticalVelController.Gravity.AddGravityForce(-_verticalVelController.Gravity.CurrentGravityForce + _jumpForce);
        }
    }
}