using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerVerticalVelocity_Gravity : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerVerticalVelocityController _verticalVelocityController;
    [SerializeField] Transform _groundCheckPoint;




    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] float _currentGravityForce; public float CurrentGravityForce { get { return _currentGravityForce; } set { _currentGravityForce = value; } }
    [SerializeField] float _fallingTime; public float FallingTime { get { return _fallingTime; } }
    [SerializeField] bool _isGrounded; public bool IsGrounded { get { return _isGrounded;} }
    [SerializeField] bool _applyGravity;
    [Space(5)]
    [Range(0, 1)]
    [SerializeField] int _applyGravityToggle;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 100)]
    [SerializeField] float _gravityForce; public float GravityForce { get { return _gravityForce; } }
    [Range(0, 0.5f)]
    [SerializeField] float _groundCheckRadius;
    [Space(5)]
    [SerializeField] LayerMask _groundMask;



    private float _notGroundedTime;


    private void Update()
    {
        GravityForceControll();
        CheckGround();
        ApplyGravity();
    }



    private void GravityForceControll()
    {
        bool resetGravity = _isGrounded && !_verticalVelocityController.Jump.IsJump;

        if (resetGravity)
        {
            _fallingTime = 0;
            _currentGravityForce = -0.01f - _verticalVelocityController.Slope.SlopeAngle;
        }
        else
        {
            _fallingTime += 2 * Time.deltaTime;
            _currentGravityForce -= _gravityForce * Time.deltaTime;
        }

        _currentGravityForce *= _applyGravityToggle;
    }
    private void ApplyGravity()
    {
        if (!_applyGravity) return;

        _verticalVelocityController.CharacterController.Move(new Vector3(0f, _currentGravityForce * Time.deltaTime, 0f));
    }
    private void CheckGround()
    {
        bool groundDetected = Physics.CheckSphere(_groundCheckPoint.position, _groundCheckRadius, _groundMask);
        if (groundDetected)
        {
            _notGroundedTime = 0;
            _isGrounded = true;
        }
        else
        {
            _notGroundedTime += 10 * Time.deltaTime;
            if (_notGroundedTime > 0.5f) _isGrounded = false;
        }
    }



    public float GetCurrentGravity()
    {
        return _currentGravityForce;
    }



    public void SetCurrentGravity(float gravity)
    {
        _currentGravityForce = gravity;
    }
    public void ToggleApplyGravity(bool enabled)
    {
        _applyGravity = enabled;
        _applyGravityToggle = _applyGravity ? 1 : 0;
    }
}
