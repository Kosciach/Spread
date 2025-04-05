using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

namespace Spread.Tools
{
    public static class Helpers
    {
        public static bool IsInRange(float p_value, float p_min, float p_max, bool p_checkEqual = false)
        {
            if(p_checkEqual)
                return p_value >= p_min && p_value <= p_max;
            return p_value > p_min && p_value < p_max;
        }

        public static Tween SimpleTimer(float p_duration, Action p_onComplete = null, Action p_onUpdate = null, bool p_unscaledTime = false)
        {
            float timer = p_duration;
            Tween tween = DOTween.To(() => timer, x => timer = x, 0, timer);
            tween.onUpdate += () => { p_onUpdate?.Invoke(); };
            tween.onComplete += () => { p_onComplete?.Invoke(); };
            tween.SetUpdate(p_unscaledTime);
            return tween;
        }
    }

    public static class HelpersExtentions
    {
        public static void Reset(this Transform p_transform)
        {
            p_transform.localPosition = Vector3.zero;
            p_transform.localEulerAngles = Vector3.zero;
            p_transform.localScale = Vector3.one;
        }

        public static Transform GetSibling(this Transform p_transform, int p_index)
        {
            return p_transform.parent.GetChild(p_index);
        }

        private static readonly System.Random _rand = new System.Random();
        public static void Shuffle<T>(this IList<T> p_list)
        {
            var count = p_list.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = p_list[i];
                p_list[i] = p_list[r];
                p_list[r] = tmp;
            }
        }
    }
}