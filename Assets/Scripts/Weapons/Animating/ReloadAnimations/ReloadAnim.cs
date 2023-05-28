using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReloadAnim : MonoBehaviour
{
    [Header("====Debugs====")]
    [SerializeField] bool _isPlaying; public bool IsPlaying {  get { return _isPlaying; } }


    [Space(20)]
    [Header("====Settings====")]
    [Range(0.1f, 2)]
    [SerializeField] float _speed;







    public void Play(Transform rightHandIk, Transform leftHandIk)
    {
        _isPlaying = true;
        PlayAnim(rightHandIk, leftHandIk);
    }
    protected abstract void PlayAnim(Transform rightHandIk, Transform leftHandIk);
}
