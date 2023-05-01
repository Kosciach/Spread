using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainCameraController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CinemachineVirtualCamera _playerCineCamera;
    [SerializeField] CinemachineInputProvider _cineInputs;
    [SerializeField] Camera _playerMainCamera;


    private CinemachinePOV _cinePOV;



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cinePOV = _playerCineCamera.GetCinemachineComponent<CinemachinePOV>();
    }





    public void RotatePlayerToCamera()
    {
        transform.rotation = Quaternion.Euler(0, _playerMainCamera.transform.rotation.eulerAngles.y, 0);
    }


    public void SetLadderCamera(Transform ladder, bool set)
    {
        if (set)
        {
            _cinePOV.m_HorizontalAxis.Value = ladder.rotation.eulerAngles.y + 90;
            _cinePOV.m_HorizontalAxis.m_MaxValue = _cinePOV.m_HorizontalAxis.Value + 50;
            _cinePOV.m_HorizontalAxis.m_MinValue = _cinePOV.m_HorizontalAxis.Value - 50;
            _cinePOV.m_HorizontalAxis.m_Wrap = false;
        }
        else
        {
            _cinePOV.m_HorizontalAxis.m_MaxValue = 180;
            _cinePOV.m_HorizontalAxis.m_MinValue = -180;
            _cinePOV.m_HorizontalAxis.m_Wrap = true;
        }
    }


    public void ToggleLockCamera(bool enable)
    {
        _cineInputs.enabled = !enable;
    }
}
