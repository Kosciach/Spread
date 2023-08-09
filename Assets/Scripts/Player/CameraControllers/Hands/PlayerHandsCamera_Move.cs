using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerHandsCamera
{
    public class PlayerHandsCamera_Move : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerHandsCameraController _cameraController;


        [Space(20)]
        [Header("====Settings====")]
        [Tooltip("Idle, Walk, Run, Combat, Throw")]
        [SerializeField] Vector3[] _presets;

        private LerpPositionPreset _lerpPreset;



        private void Awake()
        {
            _lerpPreset = new LerpPositionPreset(_cameraController.HandsCamera.transform);
        }



        public void ChangePreset(PositionsPresetsLabels presetLabel, float duration)
        {
            if (_lerpPreset.LerpCoroutine != null) StopCoroutine(_lerpPreset.LerpCoroutine);

            _lerpPreset.LerpCoroutine = _lerpPreset.LerpPreset(_presets[(int)presetLabel], duration);

            StartCoroutine(_lerpPreset.LerpCoroutine);
        }
    }


    public class LerpPositionPreset
    {
        private Transform _handsCamera;
        public IEnumerator LerpCoroutine;


        public LerpPositionPreset(Transform handsCamera)
        {
            _handsCamera = handsCamera;
        }


        public IEnumerator LerpPreset(Vector3 endPosition, float duration)
        {
            float timeElapsed = 0;
            Vector3 startPosition = _handsCamera.localPosition;

            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;
                _handsCamera.localPosition = Vector3.Lerp(startPosition, endPosition, time);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            _handsCamera.localPosition = endPosition;
        }
    }



    public enum PositionsPresetsLabels
    {
        Idle, Walk, Run, Combat, Throw
    }
}