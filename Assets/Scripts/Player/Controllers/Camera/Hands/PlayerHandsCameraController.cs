using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsCameraController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerHandsCameraMoveController _moveController; public PlayerHandsCameraMoveController MoveController { get { return _moveController; } }
    [SerializeField] PlayerHandsCameraRotateController _rotateController; public PlayerHandsCameraRotateController RotateController { get { return _rotateController; } }
    [SerializeField] PlayerHandsCameraLeanController _lean; public PlayerHandsCameraLeanController Lean { get { return _lean; } }
    [SerializeField] PlayerHandsCameraEnableController _enableController; public PlayerHandsCameraEnableController EnableController { get { return _enableController; } }
    [Space(5)]
    [SerializeField] Camera _handsCamera; public Camera HandsCamera { get { return _handsCamera; } }
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }



    private Vector3 _pos;
    private Vector3 _rot;


    private void Update()
    {
        CombineVectors();
        ApplyVectors();
    }



    private void CombineVectors()
    {
        _pos = _moveController.CurrentPosition;
        _rot = _rotateController.CurrentRotation + _lean.Rotation;
    }
    private void ApplyVectors()
    {
        _handsCamera.transform.localPosition = _pos;
        _handsCamera.transform.localRotation = Quaternion.Euler(_rot);
    }


}
