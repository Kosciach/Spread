using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsCameraMoveController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerHandsCameraController _cameraController;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] CameraPositionsEnum _cameraPositionType;
    [SerializeField] Vector3 _currentPosition; public Vector3 CurrentPosition { get { return _currentPosition; } }
    [SerializeField] Vector3 _desiredPosition;
    [SerializeField] float _moveSpeed;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3[] _cameraPositions;
    [SerializeField] bool _poseMode;

    public enum CameraPositionsEnum
    {
        Idle, Walk, Run, Combat
    }






    private void Update()
    {
        Move();
    }


    private void Move()
    {
        if (_poseMode) return;

        _currentPosition = Vector3.Lerp(_currentPosition, _desiredPosition, _moveSpeed * Time.deltaTime);
    }

    public void SetCameraPosition(CameraPositionsEnum cameraPosition, float moveSpeed)
    {
        _desiredPosition = _cameraPositions[(int)cameraPosition];
        _moveSpeed = moveSpeed;

        _cameraPositionType = cameraPosition;
    }
}
