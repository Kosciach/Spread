using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerAnimator
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] Animator _animator;



        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] LayerData[] _layerData;




        public enum LayersEnum
        {
            Combat, TopBodyStabilizer, Crouch
        }


        private void Start()
        {
            ToggleLayer(LayersEnum.TopBodyStabilizer, true, 0.1f);
        }


        public void SetInt(string name, int value)
        {
            _animator.SetInteger(name, value);
        }
        public void SetFloat(string name, float value)
        {
            _animator.SetFloat(name, value);
        }
        public void SetFloat(string name, float value, float dumpTime)
        {
            _animator.SetFloat(name, value, dumpTime, Time.deltaTime);
        }
        public void SetBool(string name, bool value)
        {
            _animator.SetBool(name, value);
        }
        public void SetTrigger(string name, bool reset)
        {
            if (!reset) _animator.SetTrigger(name);
            else _animator.ResetTrigger(name);
        }

        public void ToggleRootMotion(bool enable)
        {
            _animator.applyRootMotion = enable;
        }




        public void OverrideAnimationClip(AnimatorOverrideController overide)
        {
            _animator.runtimeAnimatorController = overide;
        }



        public void ToggleLayer(LayersEnum layer, bool enable, float duration)
        {
            float weight = enable ? 1 : 0;
            LayerData currentLayerData = _layerData[(int)layer];


            if (currentLayerData.LerpCoroutine != null) StopCoroutine(currentLayerData.LerpCoroutine);

            currentLayerData.LerpCoroutine = currentLayerData.Lerp(_animator, currentLayerData.LayerWeight, weight, duration);
            StartCoroutine(currentLayerData.LerpCoroutine);
        }
    }


    [System.Serializable]
    public class LayerData
    {
        public string name;
        public PlayerAnimatorController.LayersEnum Layer;

        [Range(0, 1)]
        public float LayerWeight;
        public IEnumerator LerpCoroutine;


        public IEnumerator Lerp(Animator animator, float startValue, float endValue, float duration)
        {
            int layerIndex = (int)Layer + 1;
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;

                LayerWeight = Mathf.Lerp(startValue, endValue, time);
                animator.SetLayerWeight(layerIndex, LayerWeight);

                timeElapsed += Time.deltaTime;


                yield return null;
            }

            LayerWeight = endValue;
            animator.SetLayerWeight(layerIndex, LayerWeight);
        }
    }
}
