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
    [SerializeField] HandsCameraPositionsEnum _handsCameraPositionType;
    [SerializeField] Vector3 _desiredPosition;
    [SerializeField] Vector3 _currentPosition;
    [SerializeField] float _moveSpeed;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3[] _handsCameraPositions;
    [SerializeField] bool _poseMode;

    public enum HandsCameraPositionsEnum
    {
        Idle, Walk, Run, Combat, Swim
    }






    private void Update()
    {
        Move();
    }


    private void Move()
    {
        if (_poseMode) return;

        _currentPosition = Vector3.Lerp(_currentPosition, _desiredPosition, _moveSpeed * Time.deltaTime);
        _cameraController.HandsCamera.transform.localPosition = _currentPosition;
    }

    public void SetHandsCameraPosition(HandsCameraPositionsEnum cameraPosition, float moveSpeed)
    {
        if (!_cameraController.PlayerStateMachine.CombatController.IsState(PlayerCombatController.CombatStateEnum.Unarmed)) return;

        _desiredPosition = _handsCameraPositions[(int)cameraPosition];
        _moveSpeed = moveSpeed;

        _handsCameraPositionType = cameraPosition;
    }
}
