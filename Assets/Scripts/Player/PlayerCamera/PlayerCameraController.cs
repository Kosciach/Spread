using PlayerStateMachineSystem;
using UnityEngine;

namespace PlayerCamera
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("---StateMachine---")]
        [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }


        [Space(20)]
        [Header("---CameraReferences---")]
        [SerializeField] Camera _playerMainCamera; public Camera PlayerMainCamera { get { return _playerMainCamera; } }


        [Space(20)]
        [Header("---SubControllers---")]
        [SerializeField] PlayerCamera_Follow _follow; public PlayerCamera_Follow Follow { get { return _follow; } }
        [SerializeField] PlayerCamera_Look _look; public PlayerCamera_Look Look { get { return _look; } }




        private void Awake()
        {
            _follow.OnAwake(this);
            _look.OnAwake(this);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void LateUpdate()
        {
            _follow.OnLateUpdate();
        }
    }
}