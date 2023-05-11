using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIkLayerController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerIkController _ikController;
    [SerializeField] Rig[] _ikLayers;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] int _layerCount;
    [SerializeField] float[] _ikLayerCurrentWeights;
    [SerializeField] float[] _ikLayerLerpSpeeds;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] float[] _ikLayerDesiredWeights;


    public enum LayerEnum
    {
        SpineLock, Body, Head, Swim, UnderWater, RangeCombat, FingersRightHand, FingersLeftHand
    }






    private void Awake()
    {
        _layerCount = _ikLayers.Length;
        _ikLayerCurrentWeights = new float[_layerCount];
    }
    private void Update()
    {
        LerpRigWeights();
    }






    private void LerpRigWeights()
    {
        for(int i=0; i<_layerCount; i++)
        {
            _ikLayerCurrentWeights[i] = Mathf.Lerp(_ikLayerCurrentWeights[i], _ikLayerDesiredWeights[i], _ikLayerLerpSpeeds[i] * Time.deltaTime);
            _ikLayerCurrentWeights[i] = Mathf.Clamp(_ikLayerCurrentWeights[i], 0, 1);

            _ikLayers[i].weight = _ikLayerCurrentWeights[i];
        }
    }


    public void SetLayerWeight(LayerEnum layer, bool enable, float lerpSpeed)
    {
        int index = (int)layer;
        float weight = enable ? 1.1f : -0.1f;

        _ikLayerDesiredWeights[index] = weight;
        _ikLayerLerpSpeeds[index] = lerpSpeed;
    }
    public float GetLayerWeight(LayerEnum layer)
    {
        int index = (int)layer;
        return _ikLayerCurrentWeights[index];
    }
}