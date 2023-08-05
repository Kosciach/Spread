using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCameraController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCamera_Fov _fov;                     public PlayerCineCamera_Fov Fov { get { return _fov; } }
    [SerializeField] PlayerCineCamera_Move _move;                   public PlayerCineCamera_Move Move { get { return _move; } }
    [SerializeField] PlayerCineCamera_Horizontal _horizontal;       public PlayerCineCamera_Horizontal Horizontal { get { return _horizontal; } }
    [SerializeField] PlayerCineCamera_Vertical _vertical;           public PlayerCineCamera_Vertical Vertical { get { return _vertical; } }
    [SerializeField] PlayerCineCamera_Recoil _recoil;               public PlayerCineCamera_Recoil Recoil { get { return _recoil; } }
    [Space(5)]
    [SerializeField] PlayerStateMachine _playerStateMachine;        public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] CinemachineVirtualCamera _cineCamera;          public CinemachineVirtualCamera CineCamera { get { return _cineCamera; } }
    [SerializeField] CinemachineInputProvider _cineInputs;          public CinemachineInputProvider InputProvider { get { return _cineInputs; } }
    [SerializeField] Camera _mainCamera;                            public Camera MainCamera { get { return _mainCamera; } }
    [SerializeField] Transform _mainCameraHolder;                   public Transform MainCameraHolder { get { return _mainCameraHolder; } }
    [SerializeField] Transform _playerTransform;                    public Transform PlayerTransform { get { return _playerTransform; } }
    private CinemachinePOV _cinePOV;                                public CinemachinePOV CinePOV { get { return _cinePOV; } }




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
    public void ToggleCineInput(bool enabled)
    {
        _cineInputs.enabled = enabled;
    }
}
