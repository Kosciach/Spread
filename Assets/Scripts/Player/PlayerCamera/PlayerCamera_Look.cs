using UnityEngine;

namespace PlayerCamera
{
    [System.Serializable]
    public class PlayerCamera_Look
    {
        private PlayerCameraController _playerCameraController;

        [Header("---Settings---")]
        [Range(0, 10)][SerializeField] float _sensitivity;
        [SerializeField] Vector2 _verticalMinMax;


        [Space(20)]
        [Header("---Debugs---")]
        [SerializeField] float _rotationHorizontal;
        [SerializeField] float _rotationVertical;



        public void OnAwake(PlayerCameraController playerCameraController)
        {
            _playerCameraController = playerCameraController;
        }



        public void Look()
        {
            Horizontal();
            Vertical();
        }

        private void Horizontal()
        {
            Vector2 inputVector = _playerCameraController.PlayerStateMachine.Input.MouseInputVector;

            _rotationHorizontal += inputVector.x * _sensitivity * Time.deltaTime * 3;
            _rotationHorizontal = Mathf.Repeat(_rotationHorizontal, 360);

            _playerCameraController.PlayerStateMachine.transform.rotation = Quaternion.Euler(new Vector3(0, _rotationHorizontal, 0));
        }
        private void Vertical()
        {
            Vector2 inputVector = _playerCameraController.PlayerStateMachine.Input.MouseInputVector;

            _rotationVertical -= inputVector.y * _sensitivity * Time.deltaTime * 3;
            _rotationVertical = Mathf.Clamp(_rotationVertical, _verticalMinMax.x, _verticalMinMax.y);

            _playerCameraController.PlayerMainCamera.transform.rotation = Quaternion.Euler(new Vector3(_rotationVertical, _playerCameraController.PlayerStateMachine.transform.rotation.eulerAngles.y, 0));
        }
    }
}
