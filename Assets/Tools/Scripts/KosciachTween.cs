using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace KosciachTools.Tween
{
    public class KosciachTween
    {
        private class Tween
        {
            public Guid Key;
            public IEnumerator Coroutine;

            public Tween(Guid key, IEnumerator coroutine)
            {
                Key = key;
                Coroutine = coroutine;
            }

            public float Delay = 0;
            public bool IsLocal = false;
            public TweenAxis Axis = TweenAxis.ALL;
            public Func<float, float> EaseMethod = Linear;
            public Action OnFinish = null;
            public Action OnUpdate = null;
            public Action<Vector2> OnUpdateVector2D = null;
            public Action<Vector3> OnUpdateVector3D = null;
            public Action<Color> OnUpdateColor = null;
            public Action<float> OnUpdateFloat = null;
        }

        private static KosciachTweenRunner Runner;
        private static Dictionary<Guid, Tween> Tweens = new Dictionary<Guid, Tween>();
        private Tween CurrentTween;


        private KosciachTween(Tween currentTween)
        {
            if (Runner == null) Runner = new GameObject("KosciachTweenRunner").AddComponent<KosciachTweenRunner>();

            CurrentTween = currentTween;
        }
        private class KosciachTweenRunner : MonoBehaviour
        {
            private void Start()
            {
                DontDestroyOnLoad(Runner);
            }


            public void RunTween(IEnumerator tweenCoroutine)
            {
                if(tweenCoroutine == null)
                {
                    Debug.LogWarning("KosciachTween - TweenCoroutine is null");
                    return;
                }

                StartCoroutine(tweenCoroutine);
            }
            public void CancelTween(IEnumerator tweenCoroutine)
            {
                if (tweenCoroutine == null)
                {
                    Debug.LogWarning("KosciachTween - TweenCoroutine is null");
                    return;
                }

                StopCoroutine(tweenCoroutine);
            }
        }



        public Guid Run()
        {
            if (CurrentTween != null)
            {
                Runner.RunTween(CurrentTween.Coroutine);
                return CurrentTween.Key;
            }

            Debug.LogWarning("KosciachTween - No active tween to run.");
            return Guid.Empty;
        }
        public KosciachTween SetDelay(float delay)
        {
            CurrentTween.Delay = delay;
            return this;
        }
        public KosciachTween SetLocal()
        {
            CurrentTween.IsLocal = true;
            return this;
        }
        public KosciachTween SetAxis(TweenAxis axis)
        {
            CurrentTween.Axis = axis;
            return this;
        }
        public KosciachTween SetEase(TweenEase ease)
        {
            CurrentTween.EaseMethod = EaseMethods[(int)ease];
            return this;
        }
        public KosciachTween SetOnFinish(Action onFinish)
        {
            CurrentTween.OnFinish = onFinish;
            return this;
        }
        public KosciachTween SetOnUpdate(Action onUpdate)
        {
            CurrentTween.OnUpdate = onUpdate;
            return this;
        }
        public KosciachTween SetOnUpdate(Action<Vector2> onUpdate)
        {
            CurrentTween.OnUpdateVector2D = onUpdate;
            return this;
        }
        public KosciachTween SetOnUpdate(Action<Vector3> onUpdate)
        {
            CurrentTween.OnUpdateVector3D = onUpdate;
            return this;
        }
        public KosciachTween SetOnUpdate(Action<Color> onUpdate)
        {
            CurrentTween.OnUpdateColor = onUpdate;
            return this;
        }
        public KosciachTween SetOnUpdate(Action<float> onUpdate)
        {
            CurrentTween.OnUpdateFloat = onUpdate;
            return this;
        }

        public static void RemoveTween(Guid tweenKey)
        {
            if(tweenKey == Guid.Empty)
            {
                Debug.LogWarning("KosciachTween - tweenKey is empty!");
                return;
            }
            if (!Tweens.ContainsKey(tweenKey))
            {
                Debug.LogWarning("KosciachTween - tween with this key doesn't exist!");
                return;
            }

            Runner.StopCoroutine(Tweens[tweenKey].Coroutine);
            Tweens.Remove(tweenKey);
        }
        public static int GetTweensCount()
        {
            return Tweens.Count;
        }



        //PositionTween---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static KosciachTween Position(Transform transform, Vector3 to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }
            if (transform == null)
            {
                Debug.LogWarning("KosciachTween - transform is null!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = PositionCoroutine(newKey, transform, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        private static IEnumerator PositionCoroutine(Guid newKey, Transform transform, Vector3 to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if(currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);

            Vector3 startPosition = currentTween.IsLocal ? transform.localPosition : transform.position;
            Vector3 currentPosition = Vector3.zero;
            float elapsedTime = 0;

            while(elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                currentPosition = currentTween.IsLocal ? transform.localPosition : transform.position;
                if (currentTween.Axis.HasFlag(TweenAxis.X)) currentPosition.x = Mathf.LerpUnclamped(startPosition.x, to.x, timeWithEase);
                if (currentTween.Axis.HasFlag(TweenAxis.Y)) currentPosition.y = Mathf.LerpUnclamped(startPosition.y, to.y, timeWithEase);
                if (currentTween.Axis.HasFlag(TweenAxis.Z)) currentPosition.z = Mathf.LerpUnclamped(startPosition.z, to.z, timeWithEase);

                if (currentTween.IsLocal) transform.localPosition = currentPosition;
                else transform.position = currentPosition;

                currentTween.OnUpdate?.Invoke();

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentPosition = currentTween.IsLocal ? transform.localPosition : transform.position;
            if (currentTween.Axis.HasFlag(TweenAxis.X)) currentPosition.x = to.x;
            if (currentTween.Axis.HasFlag(TweenAxis.Y)) currentPosition.y = to.y;
            if (currentTween.Axis.HasFlag(TweenAxis.Z)) currentPosition.z = to.z;

            if (currentTween.IsLocal) transform.localPosition = currentPosition;
            else transform.position = currentPosition;

            currentTween.OnUpdate?.Invoke();
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }



        //RotationTween---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static KosciachTween RotationEuler(Transform transform, Vector3 to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }
            if (transform == null)
            {
                Debug.LogWarning("KosciachTween - transform is null!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = RotationEulerCoroutine(newKey, transform, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        public static KosciachTween RotationQuaternion(Transform transform, Quaternion to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }
            if (transform == null)
            {
                Debug.LogWarning("KosciachTween - transform is null!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = RotationQuaternionCoroutine(newKey, transform, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        private static IEnumerator RotationEulerCoroutine(Guid newKey, Transform transform, Vector3 to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);

            Vector3 startEuler = currentTween.IsLocal ? transform.localEulerAngles : transform.eulerAngles;
            Vector3 currentEuler = Vector3.zero;
            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                currentEuler = currentTween.IsLocal ? transform.localEulerAngles : transform.eulerAngles;
                if (currentTween.Axis.HasFlag(TweenAxis.X)) currentEuler.x = Mathf.LerpUnclamped(startEuler.x, to.x, timeWithEase);
                if (currentTween.Axis.HasFlag(TweenAxis.Y)) currentEuler.y = Mathf.LerpUnclamped(startEuler.y, to.y, timeWithEase);
                if (currentTween.Axis.HasFlag(TweenAxis.Z)) currentEuler.z = Mathf.LerpUnclamped(startEuler.z, to.z, timeWithEase);

                if (currentTween.IsLocal) transform.localEulerAngles = currentEuler;
                else transform.eulerAngles = currentEuler;

                currentTween.OnUpdate?.Invoke();

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentEuler = currentTween.IsLocal ? transform.localEulerAngles : transform.eulerAngles;
            if (currentTween.Axis.HasFlag(TweenAxis.X)) currentEuler.x = to.x;
            if (currentTween.Axis.HasFlag(TweenAxis.Y)) currentEuler.y = to.y;
            if (currentTween.Axis.HasFlag(TweenAxis.Z)) currentEuler.z = to.z;

            if (currentTween.IsLocal) transform.localEulerAngles = currentEuler;
            else transform.eulerAngles = currentEuler;

            currentTween.OnUpdate?.Invoke();
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }
        private static IEnumerator RotationQuaternionCoroutine(Guid newKey, Transform transform, Quaternion to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);

            Quaternion startQuaternion = currentTween.IsLocal ? transform.localRotation : transform.rotation;
            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                Quaternion rotation = Quaternion.LerpUnclamped(startQuaternion, to, timeWithEase);
                if (currentTween.IsLocal) transform.localRotation = rotation;
                else transform.rotation = rotation;

                currentTween.OnUpdate?.Invoke();

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            if (currentTween.IsLocal) transform.localRotation = to;
            else transform.rotation = to;

            currentTween.OnUpdate?.Invoke();
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }



        //ScaleTween---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static KosciachTween Scale(Transform transform, Vector3 to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }
            if (transform == null)
            {
                Debug.LogWarning("KosciachTween - transform is null!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = ScaleCoroutine(newKey, transform, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        private static IEnumerator ScaleCoroutine(Guid newKey, Transform transform, Vector3 to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);

            Vector3 startScale = transform.localScale;
            Vector3 currentScale = Vector3.zero;
            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                currentScale = transform.localScale;
                if (currentTween.Axis.HasFlag(TweenAxis.X)) currentScale.x = Mathf.LerpUnclamped(startScale.x, to.x, timeWithEase);
                if (currentTween.Axis.HasFlag(TweenAxis.Y)) currentScale.y = Mathf.LerpUnclamped(startScale.y, to.y, timeWithEase);
                if (currentTween.Axis.HasFlag(TweenAxis.Z)) currentScale.z = Mathf.LerpUnclamped(startScale.z, to.z, timeWithEase);
                transform.localScale = currentScale;

                currentTween.OnUpdate?.Invoke();

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentScale = transform.localScale;
            if (currentTween.Axis.HasFlag(TweenAxis.X)) currentScale.x = to.x;
            if (currentTween.Axis.HasFlag(TweenAxis.Y)) currentScale.y = to.y;
            if (currentTween.Axis.HasFlag(TweenAxis.Z)) currentScale.z = to.z;
            transform.localScale = currentScale;

            currentTween.OnUpdate?.Invoke();
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }



        //VectorsTween---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static KosciachTween Vector2D(Vector2 from, Vector2 to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = Vector2DCoroutine(newKey, from, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        public static KosciachTween Vector3D(Vector3 from, Vector3 to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = Vector3DCoroutine(newKey, from, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        private static IEnumerator Vector2DCoroutine(Guid newKey, Vector2 from, Vector2 to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);


            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                Vector2 lerpValue = Vector2.LerpUnclamped(from, to, timeWithEase);
                currentTween.OnUpdateVector2D?.Invoke(lerpValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentTween.OnUpdateVector2D?.Invoke(to);
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }
        private static IEnumerator Vector3DCoroutine(Guid newKey, Vector3 from, Vector3 to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);


            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                Vector3 lerpValue = Vector3.LerpUnclamped(from, to, timeWithEase);
                currentTween.OnUpdateVector3D?.Invoke(lerpValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentTween.OnUpdateVector3D?.Invoke(to);
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }



        //VectorsTween---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static KosciachTween Float(float from, float to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = FloatCoroutine(newKey, from, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        private static IEnumerator FloatCoroutine(Guid newKey, float from, float to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);


            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                float lerpValue = Mathf.LerpUnclamped(from, to, timeWithEase);
                currentTween.OnUpdateFloat?.Invoke(lerpValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentTween.OnUpdateFloat?.Invoke(to);
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }



        //ColorTween---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static KosciachTween Color(Color from, Color to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }


            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = ColorCoroutine(newKey, from, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        private static IEnumerator ColorCoroutine(Guid newKey, Color from, Color to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);


            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                Color lerpValue = UnityEngine.Color.LerpUnclamped(from, to, timeWithEase);
                currentTween.OnUpdateColor?.Invoke(lerpValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentTween.OnUpdateColor?.Invoke(to);
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }



        //ScaleTween---------------------------------------------------------------------------------------------------------------------------------------------------------
        public static KosciachTween CanvasGroupAlpha(CanvasGroup canvasGroup, float to, float tweenTime)
        {
            if (tweenTime < 0)
            {
                Debug.LogWarning("KosciachTween - tweenTime is negative!");
                return null;
            }
            if (canvasGroup == null)
            {
                Debug.LogWarning("KosciachTween - canvasGroup is null!");
                return null;
            }

            Guid newKey = Guid.NewGuid();
            IEnumerator tweenCoroutine = CanvasGroupAlphaCoroutine(newKey, canvasGroup, to, tweenTime);

            Tween newTween = new Tween(newKey, tweenCoroutine);
            Tweens.Add(newKey, newTween);

            KosciachTween newKosciachTween = new KosciachTween(newTween);
            return newKosciachTween;
        }
        private static IEnumerator CanvasGroupAlphaCoroutine(Guid newKey, CanvasGroup canvasGroup, float to, float tweenTime)
        {
            Tween currentTween = Tweens[newKey];

            if (currentTween.Delay > 0) yield return new WaitForSeconds(currentTween.Delay);

            float startAlpha = canvasGroup.alpha;
            float elapsedTime = 0;

            while (elapsedTime < tweenTime)
            {
                float time = elapsedTime / tweenTime;
                float timeWithEase = currentTween.EaseMethod(time);

                canvasGroup.alpha = Mathf.LerpUnclamped(startAlpha, to, timeWithEase);

                currentTween.OnUpdate?.Invoke();

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = to;

            currentTween.OnUpdate?.Invoke();
            currentTween.OnFinish?.Invoke();

            Tweens.Remove(newKey);
        }



        #region Easing
        private const float _elasticConst1 = (2 * Mathf.PI) / 3;
        private const float _elasticConst2 = (2 * Mathf.PI) / 4.5f;

        private const float _backConst1 = 1.70158f;
        private const float _backConst2 = _backConst1 * 1.525f;
        private const float _backConst3 = _backConst1 + 1;

        private const float _bounceNumeratorConst = 7.5625f;
        private const float _bounceDenominatorConst = 2.75f;

        //Linear--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float Linear(float time) => time;
        //Sine--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInSine(float time) => (float)1 - Mathf.Cos((time * Mathf.PI) / 2);
        private static float EaseOutSine(float time) => (float)Mathf.Sin((time * Mathf.PI) / 2);
        private static float EaseInOutSine(float time) => (float)-(Mathf.Cos(Mathf.PI * time) - 1) / 2;
        //Cubic--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInCubic(float time) => time * time * time;
        private static float EaseOutCubic(float time) => 1 - Mathf.Pow(1 - time, 3);
        private static float EaseInOutCubic(float time) => time < 0.5f ? 4 * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 3) / 2;
        //Quint--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInQuint(float time) => time * time * time * time * time;
        private static float EaseOutQuint(float time) => 1 - Mathf.Pow(1 - time, 5);
        private static float EaseInOutQuint(float time) => time < 0.5f ? 16 * time * time * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 5) / 2;
        //Circ--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInCirc(float time) => 1 - Mathf.Sqrt(1 - Mathf.Pow(time, 2));
        private static float EaseOutCirc(float time) => Mathf.Sqrt(1 - Mathf.Pow(time - 1, 2));
        private static float EaseInOutCirc(float time) => time < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * time, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * time + 2, 2)) + 1) / 2;
        //Elastic--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInElastic(float time) => time == 0 ? 0 : time == 1 ? 1 : -Mathf.Pow(2, 10 * time - 10) * Mathf.Sin((time * 10 - 10.75f) * _elasticConst1);
        private static float EaseOutElastic(float time) => time == 0 ? 0 : time == 1 ? 1 : Mathf.Pow(2, -10 * time) * Mathf.Sin((time * 10 - 0.75f) * _elasticConst1) + 1;
        private static float EaseInOutElastic(float time)
        {
            return time == 0 ? 0 : time == 1 ? 1 : time < 0.5f ?
                -(Mathf.Pow(2, 20 * time - 10) * Mathf.Sin((20 * time - 11.125f) * _elasticConst2)) / 2 :
                (Mathf.Pow(2, -20 * time + 10) * Mathf.Sin((20 * time - 11.125f) * _elasticConst2)) / 2 + 1;
        }
        //Quad--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInQuad(float time) => time * time;
        private static float EaseOutQuad(float time) => 1 - (1 - time) * (1 - time);
        private static float EaseInOutQuad(float time) => time < 0.5f ? 2 * time * time : 1 - Mathf.Pow(-2 * time + 2, 2) / 2;
        //Quart--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInQuart(float time) => time * time * time * time;
        private static float EaseOutQuart(float time) => 1 - Mathf.Pow(1 - time, 4);
        private static float EaseInOutQuart(float time) => time < 0.5f ? 8 * time * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 4) / 2;
        //Expo--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInExpo(float time) => time == 0 ? 0 : Mathf.Pow(2, 10 * time - 10);
        private static float EaseOutExpo(float time) => time == 1 ? 1 : 1 - Mathf.Pow(2, -10 * time);
        private static float EaseInOutExpo(float time) => time == 0 ? 0 : time == 1 ? 1 : time < 0.5f ? Mathf.Pow(2, 20 * time - 10) / 2 : (2 - Mathf.Pow(2, -20 * time + 10)) / 2;
        //Back--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInBack(float time) => _backConst3 * time * time * time - _backConst1 * time * time;
        private static float EaseOutBack(float time) => 1 + _backConst3 * Mathf.Pow(time - 1, 3) + _backConst1 * Mathf.Pow(time - 1, 2);
        private static float EaseInOutBack(float time)
        {
            return time < 0.5f ?
                (Mathf.Pow(2 * time, 2) * ((_backConst2 + 1) * 2 * time - _backConst2)) / 2 :
                (Mathf.Pow(2 * time - 2, 2) * ((_backConst2 + 1) * (time * 2 - 2) + _backConst2) + 2) / 2;
        }
        //Bounce--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private static float EaseInBounce(float time) => 1 - EaseOutBounce(1 - time);
        private static float EaseOutBounce(float time)
        {
            if (time < 1 / _bounceDenominatorConst) return _bounceNumeratorConst * time * time;
            else if (time < 2 / _bounceDenominatorConst) return _bounceNumeratorConst * (time -= 1.5f / _bounceDenominatorConst) * time + 0.75f;
            else if (time < 2.5f / _bounceDenominatorConst) return _bounceNumeratorConst * (time -= 2.25f / _bounceDenominatorConst) * time + 0.9375f;
            else return _bounceNumeratorConst * (time -= 2.625f / _bounceDenominatorConst) * time + 0.984375f;
        }
        private static float EaseInOutBounce(float time) => time < 0.5f ? (1 - EaseOutBounce(1 - 2 * time)) / 2 : (1 + EaseOutBounce(2 * time - 1)) / 2;


        private static Func<float, float>[] EaseMethods =
        {
            Linear,
            EaseInSine,     EaseOutSine,    EaseInOutSine,
            EaseInCubic,    EaseOutCubic,   EaseInOutCubic,
            EaseInQuint,    EaseOutQuint,   EaseInOutQuint,
            EaseInCirc,     EaseOutCirc,    EaseInOutCirc,
            EaseInElastic,  EaseOutElastic, EaseInOutElastic,
            EaseInQuad,     EaseOutQuad,    EaseInOutQuad,
            EaseInQuart,    EaseOutQuart,   EaseInOutQuart,
            EaseInExpo,     EaseOutExpo,    EaseInOutExpo,
            EaseInBack,     EaseOutBack,    EaseInOutBack,
            EaseInBounce,   EaseOutBounce,  EaseInOutBounce,
        };
        #endregion
    }
    #region TweeningEnums
    public enum TweenEase
    {
        Linear,
        EaseInSine, EaseOutSine, EaseInOutSine,
        EaseInCubic, EaseOutCubic, EaseInOutCubic,
        EaseInQuint, EaseOutQuint, EaseInOutQuint,
        EaseInCirc, EaseOutCirc, EaseInOutCirc,
        EaseInElastic, EaseOutElastic, EaseInOutElastic,
        EaseInQuad, EaseOutQuad, EaseInOutQuad,
        EaseInQuart, EaseOutQuart, EaseInOutQuart,
        EaseInExpo, EaseOutExpo, EaseInOutExpo,
        EaseInBack, EaseOutBack, EaseInOutBack,
        EaseInBounce, EaseOutBounce, EaseInOutBounce,
    }
    [Flags]
    public enum TweenAxis
    {
        X = 1,
        Y = 2,
        Z = 4,

        XY = X | Y,
        XZ = X | Z,

        YX = Y | X,
        YZ = Y | Z,

        ZX = Z | X,
        ZY = Z | Y,

        ALL = X | Y | Z
    }
    #endregion
}