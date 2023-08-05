using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftHandAnimatorNamespace
{
    public class LeftHandAnimator : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerStateMachine _playerStateMachine;
        [SerializeField] Transform _leftHandIk;
        [SerializeField] Transform _positioner;


        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] VectorUpdateType _vectorUpdateType;
        [Space(5)]
        [SerializeField] Vector3Lerp _move;
        [SerializeField] QuaternionLerp _rotate;


        private Action[] _vectorUpdateMethods = new Action[2];


        private enum VectorUpdateType
        {
            Gameplay, SettingUp
        }




        private void Awake()
        {
            _vectorUpdateMethods[0] = UpdateVectorsGameplay;
            _vectorUpdateMethods[1] = UpdateVectorsSettingUp;
        }
        private void Update()
        {
            _vectorUpdateMethods[(int)_vectorUpdateType]();
        }


        private void UpdateVectorsGameplay()
        {
            _leftHandIk.localPosition = _move.Vector;
            _leftHandIk.localRotation = _rotate.Quaternion;

            _positioner.localPosition = _move.Vector;
            _positioner.localRotation = _rotate.Quaternion;
        }
        private void UpdateVectorsSettingUp()
        {
            _leftHandIk.localPosition = _positioner.localPosition;
            _leftHandIk.localRotation = _positioner.localRotation;
        }



        public void Move(Vector3 endPos, float duration)
        {
            if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

            _move.LerpCoroutine = _move.Lerp(_leftHandIk.localPosition, endPos, duration);
            StartCoroutine(_move.LerpCoroutine);
        }
        public void Move(Vector3 endPos, float duration, AnimationCurve curve)
        {
            if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

            _move.LerpCoroutine = _move.Lerp(_leftHandIk.localPosition, endPos, duration, curve);
            StartCoroutine(_move.LerpCoroutine);
        }
        public void MoveRaw(Vector3 pos)
        {
            if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);
            _move.SetRaw(pos);
        }


        public void Rotate(Vector3 endRot, float duration)
        {
            if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

            _rotate.LerpCoroutine = _rotate.Lerp(_leftHandIk.localRotation.eulerAngles, endRot, duration);
            StartCoroutine(_rotate.LerpCoroutine);
        }
        public void Rotate(Vector3 endRot, float duration, AnimationCurve curve)
        {
            if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

            _rotate.LerpCoroutine = _rotate.Lerp(_leftHandIk.localRotation.eulerAngles, endRot, duration, curve);
            StartCoroutine(_rotate.LerpCoroutine);
        }
        public void RotateRaw(Vector3 rot)
        {
            if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

            _rotate.SetRaw(Quaternion.Euler(rot));
        }
        public void RotateRaw(Quaternion rot)
        {
            if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

            _rotate.SetRaw(rot);
        }
    }




    [System.Serializable]
    public class Vector3Lerp
    {
        [SerializeField] Vector3 _vector; public Vector3 Vector { get { return _vector; } }
        public IEnumerator LerpCoroutine;


        public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration)
        {
            float timeElapsed = 0;

            //Start
            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;

                _vector = Vector3.Lerp(startVector, endVector, time);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _vector = endVector;
        }
        public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration, AnimationCurve curve)
        {
            float timeElapsed = 0;

            //Start
            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;
                float curveTime = curve.Evaluate(time);

                _vector = Vector3.LerpUnclamped(startVector, endVector, curveTime);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _vector = endVector;
        }
        public void SetRaw(Vector3 pos)
        {
            _vector = pos;
            LerpCoroutine = null;
        }
    }

    [System.Serializable]
    public class QuaternionLerp
    {
        [SerializeField] Quaternion _quaternion; public Quaternion Quaternion { get { return _quaternion; } }
        public IEnumerator LerpCoroutine;


        public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration)
        {
            float timeElapsed = 0;
            Quaternion startQuaternion = Quaternion.Euler(startVector);
            Quaternion endQuaternion = Quaternion.Euler(endVector);

            //Start
            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;

                _quaternion = Quaternion.Lerp(startQuaternion, endQuaternion, time);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _quaternion = endQuaternion;
        }
        public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration, AnimationCurve curve)
        {
            float timeElapsed = 0;
            Quaternion startQuaternion = Quaternion.Euler(startVector);
            Quaternion endQuaternion = Quaternion.Euler(endVector);

            //Start
            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;
                float curveTime = curve.Evaluate(time);

                _quaternion = Quaternion.LerpUnclamped(startQuaternion, endQuaternion, curveTime);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _quaternion = endQuaternion;
        }
        public void SetRaw(Quaternion rot)
        {
            _quaternion = rot;
            LerpCoroutine = null;
        }
    }
}