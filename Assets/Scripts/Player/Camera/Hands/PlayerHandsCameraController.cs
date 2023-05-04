using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsCameraController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerHandsCameraMoveController _moveController; public PlayerHandsCameraMoveController MoveController { get { return _moveController; } }
    [SerializeField] PlayerHandsCameraRotateController _rotateController; public PlayerHandsCameraRotateController RotateController { get { return _rotateController; } }
    [SerializeField] PlayerHandsCameraEnableController _enableController; public PlayerHandsCameraEnableController EnableController { get { return _enableController; } }
    [Space(5)]
    [SerializeField] Camera _handsCamera; public Camera HandsCamera { get { return _handsCamera; } }
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
}
