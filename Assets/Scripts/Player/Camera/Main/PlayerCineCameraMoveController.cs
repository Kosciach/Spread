using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCineCameraMoveController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cameraController;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] CameraPositionsEnum _cameraPositionType;
    [SerializeField] Vector3 _desiredPosition;
    [SerializeField] Vector3 _currentPosition;
    [SerializeField] float _moveSpeed;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3[] _cameraPositions;
    [SerializeField] bool _poseMode;

    public enum CameraPositionsEnum
    {
        OnGround, Swim, Ladder
    }






    private void Update()
    {
        Move();
    }


    private void Move()
    {
        if (_poseMode) return;

        _currentPosition = Vector3.Lerp(_currentPosition, _desiredPosition, _moveSpeed * Time.deltaTime);
        _cameraController.MainCameraHolder.transform.localPosition = _currentPosition;
    }

    public void SetCameraPosition(CameraPositionsEnum cameraPosition, float moveSpeed)
    {
        _desiredPosition = _cameraPositions[(int)cameraPosition];
        _moveSpeed = moveSpeed;

        _cameraPositionType  = cameraPosition;
    }
}
