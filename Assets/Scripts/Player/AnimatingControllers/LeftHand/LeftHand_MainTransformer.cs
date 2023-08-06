using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftHandAnimatorNamespace
{
    public class LeftHand_MainTransformer : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] LeftHandAnimator _leftHandAnimator;

        private Vector3Lerp _move;
        private QuaternionLerp _rotate;




        private void Awake()
        {
            _move = new Vector3Lerp(_leftHandAnimator);
            _rotate = new QuaternionLerp(_leftHandAnimator);
        }



        public void Move(Vector3 endPos, float duration)
        {
            if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

            _move.LerpCoroutine = _move.Lerp(_leftHandAnimator.LeftHandIk.localPosition, endPos, duration);
            StartCoroutine(_move.LerpCoroutine);
        }
        public void Move(Vector3 endPos, float duration, AnimationCurve curve)
        {
            if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

            _move.LerpCoroutine = _move.Lerp(_leftHandAnimator.LeftHandIk.localPosition, endPos, duration, curve);
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

            _rotate.LerpCoroutine = _rotate.Lerp(_leftHandAnimator.LeftHandIk.localRotation.eulerAngles, endRot, duration);
            StartCoroutine(_rotate.LerpCoroutine);
        }
        public void Rotate(Vector3 endRot, float duration, AnimationCurve curve)
        {
            if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

            _rotate.LerpCoroutine = _rotate.Lerp(_leftHandAnimator.LeftHandIk.localRotation.eulerAngles, endRot, duration, curve);
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




    public class Vector3Lerp
    {
        public IEnumerator LerpCoroutine;
        public LeftHandAnimator _leftHandAnimator;

        public Vector3Lerp(LeftHandAnimator leftHandAnimator)
        {
            _leftHandAnimator = leftHandAnimator;
        }

        public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration)
        {
            float timeElapsed = 0;

            //Start
            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;

                _leftHandAnimator.LeftHandIk.localPosition = Vector3.Lerp(startVector, endVector, time);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _leftHandAnimator.LeftHandIk.localPosition = endVector;
        }
        public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration, AnimationCurve curve)
        {
            float timeElapsed = 0;

            //Start
            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;
                float curveTime = curve.Evaluate(time);

                _leftHandAnimator.LeftHandIk.localPosition = Vector3.LerpUnclamped(startVector, endVector, curveTime);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _leftHandAnimator.LeftHandIk.localPosition = endVector;
        }
        public void SetRaw(Vector3 pos)
        {
            _leftHandAnimator.LeftHandIk.localPosition = pos;
            LerpCoroutine = null;
        }
    }

    [System.Serializable]
    public class QuaternionLerp
    {
        public IEnumerator LerpCoroutine;
        public LeftHandAnimator _leftHandAnimator;

        public QuaternionLerp(LeftHandAnimator leftHandAnimator)
        {
            _leftHandAnimator = leftHandAnimator;
        }


        public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration)
        {
            float timeElapsed = 0;
            Quaternion startQuaternion = Quaternion.Euler(startVector);
            Quaternion endQuaternion = Quaternion.Euler(endVector);

            //Start
            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;

                _leftHandAnimator.LeftHandIk.localRotation = Quaternion.Lerp(startQuaternion, endQuaternion, time);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _leftHandAnimator.LeftHandIk.localRotation = endQuaternion;
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

                _leftHandAnimator.LeftHandIk.localRotation = Quaternion.LerpUnclamped(startQuaternion, endQuaternion, curveTime);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            //Finish
            _leftHandAnimator.LeftHandIk.localRotation = endQuaternion;
        }
        public void SetRaw(Quaternion rot)
        {
            _leftHandAnimator.LeftHandIk.localRotation = rot;
            LerpCoroutine = null;
        }
    }
}