using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerVerticalVelocityController _verticalVelocityController;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] bool _isJump; public bool IsJump { get { return _isJump; } }
    [SerializeField] bool _jumpReloaded; public bool JumpReloaded { get { return _jumpReloaded; } }



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _jumpForce;
    [Space(5)]
    [SerializeField] LayerMask _playerMask;






    public void Jump()
    {
        _verticalVelocityController.GravityController.CurrentGravityForce = 0;

        float jumpHeight = (_verticalVelocityController.GravityController.GravityForce + _jumpForce) / 100;
        _verticalVelocityController.GravityController.CurrentGravityForce = jumpHeight;
    }
    public void AddDownVelocity()
    {
        LeanTween.value(0, 1, 0.2f).setOnUpdate((float val) =>
        {
            _verticalVelocityController.GravityController.SetCurrentGravity(_verticalVelocityController.GravityController.GetCurrentGravity() - 0.005f * _jumpForce);
        });
    }
    public bool CheckAboveObsticle()
    {
        Debug.DrawRay(transform.position, Vector3.up * 4, Color.cyan, 5);
        return Physics.Raycast(transform.position, Vector3.up, 4, ~_playerMask);
    }



    public void ToggleIsJump(bool enable)
    {
        _isJump = enable;
    }
    public void ToggleJumpReloaded(bool enable)
    {
        _jumpReloaded = enable;
    }
}
