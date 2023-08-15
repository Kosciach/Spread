using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerVerticalVelocity_Jump : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerVerticalVelocityController _verticalVelocityController;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isJump; public bool IsJump { get { return _isJump; } set { _isJump = value; } }



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _jumpForce;
    [Space(5)]
    [SerializeField] LayerMask _playerMask;






    public void Jump()
    {
        _isJump = true;
        float jumpHeight = _jumpForce * _verticalVelocityController.Gravity.GravityForce / 3;
        _verticalVelocityController.Gravity.CurrentGravityForce = jumpHeight;
    }
    public bool CheckAboveObsticle()
    {
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(transform.position, Vector3.up * 4, Color.cyan, 5);
        return Physics.Raycast(transform.position, Vector3.up, out hit, 4, ~_playerMask);
    }
}
