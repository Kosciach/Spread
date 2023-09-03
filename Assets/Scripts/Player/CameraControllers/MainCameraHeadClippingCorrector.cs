using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraHeadClippingCorrector : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;

    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isCameraInWall;
    [SerializeField] int _isCameraInWallInt;
    [SerializeField] float _cameraCorrection;

    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _cameraCorrectionSpeed;

    private int _toggle = 1;



    private void Update()
    {
        CorrectCamera();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weapon")) return;
        _isCameraInWall = true;
        _isCameraInWallInt = 1;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weapon")) return;
        _isCameraInWall = false;
        _isCameraInWallInt = -1;
    }



    private void CorrectCamera()
    {
        _cameraCorrection += _isCameraInWallInt * _cameraCorrectionSpeed * _toggle;
        _cameraCorrection = Mathf.Clamp(_cameraCorrection, 0, 45);
        _cineCameraController.Vertical.SetBorderValues(-70, 70 - _cameraCorrection);
    }

    public void Toggle(bool enable)
    {
        _toggle = enable ? 1 : 0;
    }
}
