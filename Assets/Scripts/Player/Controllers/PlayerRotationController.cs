using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;





    public void RotateToCanera()
    {
        float yRotation = _playerStateMachine.CameraControllers.Cine.MainCamera.transform.rotation.eulerAngles.y;
        transform.parent.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void RotateAnimation()
    {
        _playerStateMachine.AnimatingControllers.Animator.SetFloat("RotateDirection", _playerStateMachine.CoreControllers.Input.MouseInputVector.x);
        _playerStateMachine.AnimatingControllers.Animator.SetFloat("RotationSpeed", Mathf.Abs(_playerStateMachine.CoreControllers.Input.MouseInputVector.x/20));
        _playerStateMachine.AnimatingControllers.Animator.SetBool("IsRotating", _playerStateMachine.CoreControllers.Input.MouseInputVector.x != 0);
    }
}
