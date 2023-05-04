using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Animator _animator;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] float[] _currentLayerWeights;
    [SerializeField] float[] _desiredLayerWeights;
    [SerializeField] float[] _lerpWeightLayerSpeed;




    public enum LayersEnum
    {
        Combat, TopBodyStabilizer
    }


    private void Update()
    {
        UpdateLayers();
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
        if(!reset) _animator.SetTrigger(name);
        else _animator.ResetTrigger(name);
    }

    public void ToggleRootMotion(bool enable)
    {
        _animator.applyRootMotion = enable;
    }









    private void UpdateLayers()
    {
        for(int i=0; i<2; i++)
        {
            _currentLayerWeights[i] = Mathf.Lerp(_currentLayerWeights[i], _desiredLayerWeights[i], _lerpWeightLayerSpeed[i] * Time.deltaTime);
            _currentLayerWeights[i] = Mathf.Clamp(_currentLayerWeights[i], 0, 1);
            _animator.SetLayerWeight(i + 1, _currentLayerWeights[i]);
        }    
    }
    public void ToggleLayer(LayersEnum layer, bool enable, float lerpSpeed)
    {
        float weight = enable ? 1.1f : -0.1f;
        int index = (int)layer;

        _desiredLayerWeights[index] = weight;
        _lerpWeightLayerSpeed[index] = lerpSpeed;
    }
}
