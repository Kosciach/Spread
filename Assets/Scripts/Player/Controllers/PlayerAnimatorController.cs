using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Animator _animator;



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
}
