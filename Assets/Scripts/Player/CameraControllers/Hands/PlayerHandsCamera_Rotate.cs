using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerHandsCamera
{
    public class PlayerHandsCamera_Rotate : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerHandsCameraController _cameraController;


        [Space(20)]
        [Header("====Settings====")]
        [Tooltip("IdleWalkRun, Crouch, Combat, Throw")]
        [SerializeField] Vector3[] _presets;

        private LerpRotationPreset _lerpPreset; public LerpRotationPreset LerpPreset { get { return _lerpPreset; } }


        private void Awake()
        {
            _lerpPreset = new LerpRotationPreset();
        }


        public void ChangePreset(RotationPresetsLabels presetLabel, float duration)
        {
            if (_lerpPreset.LerpCoroutine != null) StopCoroutine(_lerpPreset.LerpCoroutine);

            _lerpPreset.LerpCoroutine = _lerpPreset.LerpPreset(_presets[(int)presetLabel], duration);

            StartCoroutine(_lerpPreset.LerpCoroutine);
        }



    }

    public class LerpRotationPreset
    {
        public IEnumerator LerpCoroutine;
        private Vector3 _rotation; public Vector3 Rotation { get { return _rotation; } }


        public IEnumerator LerpPreset(Vector3 endRotation, float duration)
        {
            float timeElapsed = 0;
            Vector3 startRotation = _rotation;

            while(timeElapsed < duration)
            {
                float time = timeElapsed / duration;
                _rotation = Vector3.Lerp(startRotation, endRotation, time);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            _rotation = endRotation;
        }
    }

    public enum RotationPresetsLabels
    {
        IdleWalkRun, Crouch, Combat, Throw
    }
}