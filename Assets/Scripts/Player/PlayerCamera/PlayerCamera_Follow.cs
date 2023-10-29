using UnityEngine;

namespace PlayerCamera
{
    [System.Serializable]
    public class PlayerCamera_Follow
    {
        private PlayerCameraController _playerCameraController;

        [Header("---References---")]
        [SerializeField] Transform _followTarget;


        public void OnAwake(PlayerCameraController playerCameraController)
        {
            _playerCameraController = playerCameraController;
        }



        public void OnLateUpdate()
        {
            _playerCameraController.PlayerMainCamera.transform.position = _followTarget.position;
        }
    }
}