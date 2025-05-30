using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace Spread.Player.Camera
{
    using DG.Tweening;
    using StateMachine;

    public class PlayerCameraController : MonoBehaviour
    {
        private PlayerStateMachineContext _ctx;
        public UnityEngine.Camera Main { get; private set; }

        [BoxGroup("References"), SerializeField] private Animator _animator;
        [BoxGroup("References"), SerializeField] private UnityEngine.Camera _camera;
        [BoxGroup("References"), SerializeField] private CinemachineVirtualCamera _cineCamera;
        [BoxGroup("References"), SerializeField] private CinemachineInputProvider _cineInput;
        private CinemachinePOV _cinePov;

        [BoxGroup("Settings"), SerializeField] private bool _clampRot;
        [BoxGroup("Settings"), SerializeField, Range(0, 180), ShowIf(nameof(_clampRot))] private float _clampRotAngle;
        [BoxGroup("Settings"), SerializeField, Range(0, 180)] private float _overTurnStart = 100;
        [BoxGroup("Settings"), SerializeField, Range(0, 180)] private float _overTurnEnd = 5;
        [BoxGroup("Settings"), SerializeField, Range(0, 10)] private float _moveRotSpeed = 5;

        [Foldout("Debug"), SerializeField, Range(0, 360), ReadOnly] private float _yPlayerRot;
        [Foldout("Debug"), SerializeField, Range(0, 360), ReadOnly] private float _yCameraRot;
        [Foldout("Debug"), SerializeField, Range(0, 180), ReadOnly] private float _yPlayerCameraAngle;
        [Foldout("Debug"), SerializeField, Range(-1, 1), ReadOnly] private int _rotDir;
        [Foldout("Debug"), SerializeField, ReadOnly] private bool _overTurn;

        private Tween _rotTweenXAxis;
        private Tween _rotTweenYAxis;
        public bool EnableInput = true;

        internal void Setup(PlayerStateMachineContext p_ctx)
        {
            _ctx = p_ctx;
            Main = UnityEngine.Camera.main;
            _cinePov = _cineCamera.GetCinemachineComponent<CinemachinePOV>();

            ToggleCursor(false);
            Toggle(true);
        }

        private void Update()
        {
            _cineInput.enabled = EnableInput && _rotTweenXAxis == null && _rotTweenYAxis == null;
        }

        private void CalculateRotHelpers()
        {
            _yPlayerRot = transform.eulerAngles.y;
            _yCameraRot = _camera.transform.eulerAngles.y;

            Vector3 playerForward = transform.forward;
            Vector3 cameraForward = _camera.transform.forward;

            playerForward.y = 0;
            cameraForward.y = 0;

            _yPlayerCameraAngle = Vector3.Angle(playerForward, cameraForward);
        }

        private void ClampHorizontal()
        {
            if (!_clampRot) return;

            float angle = _cinePov.m_HorizontalAxis.Value;
            float min = -_clampRotAngle + _yPlayerRot;
            float max = _clampRotAngle + _yPlayerRot;

            float start = (min + max) * 0.5f - 180;
            float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
            _cinePov.m_HorizontalAxis.Value = Mathf.Clamp(angle, min - 50 + floor, max + 50 + floor);

            if (_cinePov.m_HorizontalAxis.Value < min + floor || _cinePov.m_HorizontalAxis.Value > max + floor)
            {
                float targetValue = Mathf.Clamp(_cinePov.m_HorizontalAxis.Value, min + floor, max + floor);
                _cinePov.m_HorizontalAxis.Value = Mathf.Lerp(_cinePov.m_HorizontalAxis.Value, targetValue, 5 * Time.deltaTime);
            }
        }

        internal void IdleCamera()
        {
            CalculateRotHelpers();

            _overTurn = _yPlayerCameraAngle > (_overTurn ? _overTurnEnd : _overTurnStart);

            float yRotDiff = (_yPlayerRot - _yCameraRot) % 360;
            yRotDiff = yRotDiff < 0 ? yRotDiff + 360 : yRotDiff;
            _rotDir = _overTurn ? yRotDiff > 180 ? 1 : yRotDiff < 180 ? -1 : 0 : 0;

            Quaternion additionalRot = Quaternion.Euler(Vector3.zero);
            if (Mathf.Abs(_cinePov.m_HorizontalAxis.m_InputAxisValue) > 5 && _overTurn)
                additionalRot = Quaternion.Euler(new Vector3(0, _cinePov.m_HorizontalAxis.m_InputAxisValue / 10, 0));

            _ctx.AnimatorController.SetTurn(_rotDir * Mathf.Max(1, Mathf.Abs(_cinePov.m_HorizontalAxis.m_InputAxisValue) / 5));
            transform.rotation *= _animator.deltaRotation * additionalRot;

            ClampHorizontal();
        }

        internal void MoveCamera()
        {
            ClampHorizontal();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _yCameraRot, 0), _moveRotSpeed * Time.deltaTime);

            CalculateRotHelpers();
        }

        internal void ToggleCursor(bool p_show)
        {
            Cursor.visible = p_show;
            Cursor.lockState = p_show ? CursorLockMode.None : CursorLockMode.Locked;
        }

        internal void Toggle(bool p_toggle)
        {
            _cineCamera.enabled = p_toggle;
        }

        internal void RotToXAxis(float p_rot, float p_duration)
        {
            if (_rotTweenXAxis != null)
            {
                _rotTweenXAxis.Kill();
                _rotTweenXAxis.onComplete = null;
                _rotTweenXAxis = null;
            }

            float currentRot = _cinePov.m_VerticalAxis.Value;
            float shortestRotation = Mathf.DeltaAngle(currentRot, p_rot);
            float targetRot = currentRot + shortestRotation;

            _rotTweenXAxis = DOTween.To(() => currentRot, x => currentRot = x, targetRot, p_duration);
            _rotTweenXAxis.OnUpdate(() => _cinePov.m_VerticalAxis.Value = currentRot);
            _rotTweenXAxis.OnComplete(() => { _rotTweenXAxis = null; });
            _rotTweenXAxis.SetEase(Ease.Linear);
        }

        internal void RotToYAxis(float p_rot, float p_duration)
        {
            if (_rotTweenYAxis != null)
            {
                _rotTweenYAxis.Kill();
                _rotTweenYAxis.onComplete = null;
                _rotTweenYAxis = null;
            }

            float currentRot = _cinePov.m_HorizontalAxis.Value;
            float shortestRotation = Mathf.DeltaAngle(currentRot, p_rot);
            float targetRot = currentRot + shortestRotation;

            _rotTweenYAxis = DOTween.To(() => currentRot, x => currentRot = x, targetRot, p_duration);
            _rotTweenYAxis.OnUpdate(() => _cinePov.m_HorizontalAxis.Value = currentRot);
            _rotTweenYAxis.OnComplete(() => { _rotTweenYAxis = null; });
            _rotTweenYAxis.SetEase(Ease.Linear);
        }

        internal void ToggleWrap(bool p_enable)
        {
            _cinePov.m_HorizontalAxis.m_Wrap = p_enable;
        }

        internal void SetMinMax(float p_base, float p_minMax)
        {
            _cinePov.m_HorizontalAxis.m_MinValue = p_base - p_minMax;
            _cinePov.m_HorizontalAxis.m_MaxValue = p_base + p_minMax;
        }

        internal void ResetMinMax()
        {
            _cinePov.m_HorizontalAxis.m_MinValue = 0;
            _cinePov.m_HorizontalAxis.m_MaxValue = 360;
        }
    }
}