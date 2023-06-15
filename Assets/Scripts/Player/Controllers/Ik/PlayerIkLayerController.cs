using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;



namespace IkLayers
{
    public class PlayerIkLayerController : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerStateMachine _playerStateMachine;


        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] int _layerCount;
        [SerializeField] LayerData[] _layerData;

        public enum LayerEnum
        {
            SpineLock, Body, Head, Swim, UnderWater, RangeCombat, FingersRightHand, FingersLeftHand, BakedWeaponAnimating
        }






        private void Awake()
        {
            _layerCount = _layerData.Length;
        }
        private void Start()
        {
            //Enable SpineLock, Body and Head
            StartCoroutine(_layerData[0].Lerp(0, 1, 1));
            StartCoroutine(_layerData[1].Lerp(0, 1, 1));
            StartCoroutine(_layerData[2].Lerp(0, 1, 1));
        }



        public PlayerIkLayerController ToggleLayer(LayerEnum layer, bool enable, float duration)
        {
            float weight = enable ? 1 : 0;
            LayerData currentLayerData = _layerData[(int)layer];


            if (currentLayerData.LerpCoroutine != null) StopCoroutine(currentLayerData.LerpCoroutine);

            currentLayerData.LerpCoroutine = currentLayerData.Lerp(currentLayerData.LayerWeight, weight, duration);
            StartCoroutine(currentLayerData.LerpCoroutine);

            return this;
        }

        public void OnLerpFinish(LayerEnum layer, Action toDo)
        {
            _layerData[(int)layer].OnFinish = toDo;
        }
    }


    [System.Serializable]
    public class LayerData
    {
        public string name;
        public Rig Layer;

        [Range(0, 1)]
        public float LayerWeight;
        public IEnumerator LerpCoroutine;
        public Action OnFinish;

        public IEnumerator Lerp(float startValue, float endValue, float duration)
        {
            OnFinish = null;
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                float time = timeElapsed / duration;

                LayerWeight = Mathf.Lerp(startValue, endValue, time);
                Layer.weight = LayerWeight;

                timeElapsed += Time.deltaTime;


                yield return null;
            }

            LayerWeight = endValue;
            Layer.weight = LayerWeight;

            if (OnFinish != null) OnFinish.Invoke();
        }
    }
}