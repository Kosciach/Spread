using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;





    public void RotateToCanera()
    {
        float yRotation = _playerStateMachine.CineCameraController.MainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void RotateAnimation()
    {
        _playerStateMachine.AnimatorController.SetFloat("RotateDirection", _playerStateMachine.InputController.MouseInputVector.x);
        _playerStateMachine.AnimatorController.SetFloat("RotationSpeed", Mathf.Abs(_playerStateMachine.InputController.MouseInputVector.x/20));
        _playerStateMachine.AnimatorController.SetBool("IsRotating", _playerStateMachine.InputController.MouseInputVector.x != 0);
    }
}
