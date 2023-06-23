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
    [SerializeField] float _cameraCorrection;

    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _cameraCorrectionSpeed;




    private void Update()
    {
        CorrectCamera();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weapon")) return;
        _isCameraInWall = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weapon")) return;
        _isCameraInWall = false;
    }



    private void CorrectCamera()
    {
        _cameraCorrection += (_isCameraInWall ? 1 : -1) * _cameraCorrectionSpeed;
        _cameraCorrection = Mathf.Clamp(_cameraCorrection, 0, 45);
        _cineCameraController.VerticalController.SetBorderValues(-70, 70 - _cameraCorrection);
    }
}
