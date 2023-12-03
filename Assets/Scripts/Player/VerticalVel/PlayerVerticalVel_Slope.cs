using UnityEngine;

namespace PlayerVerticalVel
{
    [System.Serializable]
    public class PlayerVerticalVel_Slope
    {
        private PlayerVerticalVelController _verticalVelController;
        private Transform _transform => _verticalVelController.transform;

        [Header("---Debugs---")]
        [SerializeField] float _slopeAngle; public float SlopeAngle { get => _slopeAngle; }
        [SerializeField, Range(0, 1)] int _slopeAngleToggle;

        public void OnAwake(PlayerVerticalVelController verticalVelController)
        {
            _verticalVelController = verticalVelController;
        }




        public void OnUpdate()
        {
            CalculateSlope();
            CheckSlopeEdge();
        }

        private void CalculateSlope()
        {
            if (!Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hit, 1, LayerMask.GetMask("Ground"))) return;

            _slopeAngle = Vector3.Angle(hit.normal, Vector3.up) * _slopeAngleToggle;
        }
        private void CheckSlopeEdge()
        {
            bool walkingToEdge = false;

            Vector3 inputVector = _verticalVelController.PlayerStateMachine.Input.MovementInputVector;
            Vector3 rayPosition = _transform.position + (_transform.forward / 2 * inputVector.z + _transform.right / 2 * inputVector.x);

            Debug.DrawRay(rayPosition + Vector3.up, Vector3.down * 2, Color.red);
            walkingToEdge = Physics.Raycast(rayPosition + Vector3.up, Vector3.down, 2, LayerMask.GetMask("Ground"));

            _slopeAngleToggle = walkingToEdge ? 1 : 0;
        }
    }
}