using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsCameraRotateController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerHandsCameraController _cameraController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] HandsCameraRotationsEnum _handsCameraRotationType;
    [SerializeField] Vector3 _desiredRotation;
    [SerializeField] Vector3 _currentRotation;
    [SerializeField] float _rotateSpeed;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3[] _handsCameraRotations = new Vector3[4];
    [SerializeField] bool _poseMode;


    public enum HandsCameraRotationsEnum
    {
        IdleWalkRun, Crouch, Climb, Combat
    }






    private void Update()
    {
        Rotate();
    }


    private void Rotate()
    {
        if (_poseMode) return;

        _currentRotation = Vector3.Lerp(_currentRotation, _desiredRotation, _rotateSpeed * Time.deltaTime);
        _cameraController.HandsCamera.transform.localRotation = Quaternion.Euler(_currentRotation);
    }


    public void SetHandsCameraRotation(HandsCameraRotationsEnum cameraRotation, float rotateSpeed)
    {
        _desiredRotation = _handsCameraRotations[(int)cameraRotation];
        _rotateSpeed = rotateSpeed;
        _handsCameraRotationType = cameraRotation;
    }
}
