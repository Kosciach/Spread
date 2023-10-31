using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KosciachTools.Delay
{
    public static class KosciachDelay
    {
        //Delay
        public static IEnumerator Delay(float delayTime, Action callback)
        {
            if(delayTime < 0)
            {
                Debug.LogWarning("KosciachDelay - delayTime is negative!");
                return null;
            }
            if (callback == null)
            {
                Debug.LogWarning("KosciachDelay - callback is null!");
                return null;
            }

            IEnumerator delay = ExecuteWithDelay(delayTime, callback);
            KosciachDelayRunner.Instance.StartCoroutine(delay);
            return delay;
        }
        private static IEnumerator ExecuteWithDelay(float delayTime, Action callback)
        {
            yield return new WaitForSeconds(delayTime);

            callback.Invoke();
        }


        //DelayUtility
        public static void CancelDelay(IEnumerator delay)
        {
            if(delay == null)
            {
                Debug.LogWarning("KosciachDelay - cancel delay is null!");
                return;
            }

            KosciachDelayRunner.Instance.StopCoroutine(delay);
        }
        private class KosciachDelayRunner : MonoBehaviour
        {
            private static KosciachDelayRunner _instance;
            public static KosciachDelayRunner Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        GameObject delayGameObject = new GameObject("KosciachDelayRunner");
                        _instance = delayGameObject.AddComponent<KosciachDelayRunner>();
                        DontDestroyOnLoad(delayGameObject);
                    }
                    return _instance;
                }
            }
        }
    }
}