using Tools;
using UnityEngine;
using System.Collections.Generic;
using KosciachTools.Tween;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;

        [Header("--References--")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _cameraHolder; public Transform CameraHolder => _cameraHolder;

        [Space(20)]
        [Header("--Settings--")]
        [SerializeField, Range(0, 100)] private float _sensitivity;
        [SerializeField, Range(-90, 5)] private float _minVerticalRotation;
        [SerializeField, Range(5, 90)] private float _maxVerticalRotation;
        [SerializeField] private List<Vector2> _bobbingSettings;

        [Space(20)]
        [Header("--Debugs--")]
        [SerializeField, ReadOnly] private float _verticalRotation;
        [SerializeField, ReadOnly] private float _horizontalRotation;
        [SerializeField, ReadOnly] private float _bobbingTimer;
        [SerializeField, ReadOnly] private Vector3 _bobbing;

        private float _bobbingAmplitude;

        private void Awake()
        {
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
            ToggleCursor(false);
        }
        private void Update()
        {
            Bobbing();
            CameraToCameraHolder();
        }


        public void Look()
        {
            float inputY = _playerContext.InputController.MouseInputVector.y;
            float inputX = _playerContext.InputController.MouseInputVector.x;

            //Vertical
            _verticalRotation -= inputY * _sensitivity * 2 * Time.deltaTime;
            _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalRotation, _maxVerticalRotation);
            _mainCamera.transform.eulerAngles = new Vector3(_verticalRotation, transform.eulerAngles.y, 0);

            //Horizontal
            _horizontalRotation += inputX * _sensitivity * 2 * Time.deltaTime;
            _horizontalRotation = Mathf.Repeat(_horizontalRotation, 360);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _horizontalRotation, transform.eulerAngles.z);

            _playerContext.AnimatorController.SetLookX(inputX);
        }


        private void Bobbing()
        {
            int movementType =
            !_playerContext.InputController.IsMoving ? 0 :
            !_playerContext.InputController.IsRun ? _playerContext.MovementController.IsCrouch ? 3 : 1 :
            _playerContext.InputController.MoveInputVector.y > 0 ? 2 : 1;

            _bobbingTimer += Time.deltaTime * _bobbingSettings[movementType].x;
            _bobbingAmplitude = Mathf.Lerp(_bobbingAmplitude, _bobbingSettings[movementType].y, 1 * Time.deltaTime);

            _bobbing.x = Mathf.Cos(_bobbingTimer/2) * _bobbingAmplitude * 2;
            _bobbing.y = Mathf.Sin(_bobbingTimer) * _bobbingAmplitude;

            _bobbing *= _playerContext.SlopeController.ShouldSlide ? 0 : 1;
        }
        private void CameraToCameraHolder()
        {
            _mainCamera.transform.localPosition = _cameraHolder.localPosition + _bobbing;
        }


        public void Crouch(bool p_isCrouch)
        {
            if (p_isCrouch)
                KosciachTween.Position(_cameraHolder.transform, Vector3.one * 1.357f, 0.1f).SetLocal().SetAxis(TweenAxis.Y).Run();
            else
                KosciachTween.Position(_cameraHolder.transform, Vector3.one * 1.63f, 0.1f).SetLocal().SetAxis(TweenAxis.Y).Run();
        }
        public void ToggleCursor(bool p_toggle)
        {
            Cursor.lockState = p_toggle ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = p_toggle;
        }
    }
}