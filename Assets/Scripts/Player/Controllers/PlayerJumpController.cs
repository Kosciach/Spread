using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerGravityController _gravityController;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] bool _isJump;
    [SerializeField] bool _jumpReloaded;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _jumpForce;
    [Space(5)]
    [SerializeField] LayerMask _playerMask;






    public void Jump()
    {
        _gravityController.SetCurrentGravity(0);

        float jumpHeight = Mathf.Sqrt(_jumpForce / 60);
        _gravityController.SetCurrentGravity(jumpHeight);
    }
    public void AddDownVelocity()
    {
        LeanTween.value(0, 1, 0.2f).setOnUpdate((float val) =>
        {
            _gravityController.SetCurrentGravity(_gravityController.GetCurrentGravity() - 0.005f * _jumpForce);
        });
    }
    public bool CheckAboveObsticle()
    {
        Debug.DrawRay(transform.position, Vector3.up * 4, Color.cyan, 5);
        return Physics.Raycast(transform.position, Vector3.up, 4, ~_playerMask);
    }




    public bool GetIsJump()
    {
        return _isJump;
    }
    public bool GetJumpReloaded()
    {
        return _jumpReloaded;
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
