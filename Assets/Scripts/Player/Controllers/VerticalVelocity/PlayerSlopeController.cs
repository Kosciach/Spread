using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerVerticalVelocityController _verticalVelocityController;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] float _slopeAngle; public float SlopeAngle { get { return _slopeAngle; } }
    [Range(0, 1)]
    [SerializeField] int _slopeAngleToggle;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] LayerMask _groundMask;



    private void Update()
    {
        EdgeDetection();
        CheckSlope();
    }




    private void CheckSlope()
    {
        RaycastHit slopeInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out slopeInfo, 1, _groundMask))
        {
            _slopeAngle = Vector3.Angle(slopeInfo.normal, Vector3.up) * _slopeAngleToggle;
        }
    }
    private void EdgeDetection()
    {
        bool walkingToEdge = false;
        Vector3 inputVector = _verticalVelocityController.PlayerStateMachine.CoreControllers.Input.MovementInputVectorNormalized;
        Vector3 rayPosition = transform.position + (transform.forward / 2 * inputVector.z + transform.right / 2 * inputVector.x);

        Debug.DrawRay(rayPosition + Vector3.up, Vector3.down * 2, Color.red, 1);
        walkingToEdge = Physics.Raycast(rayPosition + Vector3.up, Vector3.down, 2, _groundMask);

        _slopeAngleToggle = walkingToEdge ? 1 : 0;
    }


    public void ToggleSlopeAngle(bool enable)
    {
        _slopeAngleToggle = enable ? 1 : 0;
    }
}
