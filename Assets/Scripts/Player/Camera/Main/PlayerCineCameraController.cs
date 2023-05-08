using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCameraController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerMainCameraFovController _fov; public PlayerMainCameraFovController Fov { get { return _fov; } }
    [SerializeField] PlayerCineCameraMoveController _move; public PlayerCineCameraMoveController Move { get { return _move; } }
    [SerializeField] PlayerCineCameraHorizontalController _horizontal; public PlayerCineCameraHorizontalController HorizontalController { get { return _horizontal; } }
    [SerializeField] PlayerCineCameraVerticalController _vertical; public PlayerCineCameraVerticalController VerticalController { get { return _vertical; } }
    [Space(5)]
    [SerializeField] CinemachineVirtualCamera _cineCamera; public CinemachineVirtualCamera CineCamera { get { return _cineCamera; } }
    [SerializeField] CinemachinePOV _cinePOV; public CinemachinePOV CinePOV { get { return _cinePOV; } }
    [SerializeField] CinemachineInputProvider _cineInputs; public CinemachineInputProvider InputProvider { get { return _cineInputs; } }
    [SerializeField] Camera _mainCamera; public Camera MainCamera { get { return _mainCamera; } }
    [SerializeField] Transform _mainCameraHolder; public Transform MainCameraHolder { get { return _mainCameraHolder; } }
    [SerializeField] Transform _playerTransform; public Transform PlayerTransform { get { return _playerTransform; } }





    private void Awake()
    {
        _cinePOV = _cineCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    private void Start()
    {
        SetCursorState(CursorLockMode.Locked, false);
    }




    public void SetCursorState(CursorLockMode cursorLockMode, bool visible)
    {
        Cursor.lockState = cursorLockMode;
        Cursor.visible = visible;
    }
    public void RotatePlayerToCamera()
    {
        _playerTransform.rotation = Quaternion.Euler(0, _mainCamera.transform.rotation.eulerAngles.y, 0);
    }
    public void ToggleCineInput(bool enabled)
    {
        _cineInputs.enabled = enabled;
    }
}
