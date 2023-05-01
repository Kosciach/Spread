using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepAudioController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _walkFootStepsClips;
    [SerializeField] AudioClip[] _runFootStepsClips;
    [SerializeField] AudioClip[] _landFootStepsClips;


    private int _index;








    public void FootStep(string baseMovementType)
    {
        _index = Random.Range(0, _walkFootStepsClips.Length);
        switch (baseMovementType)
        {
            case "walk":
                _audioSource.clip = _walkFootStepsClips[_index];
            break;

            case "run":
                _audioSource.clip = _runFootStepsClips[_index];
            break;
        }
        _audioSource.Play();
    }
    public void LandFootStep(float velocity)
    {
        int index = 0;

        if (velocity <= -0.3f) index = 0;
        else if (velocity > -0.3f && velocity <= -0.2f) index = 1;
        else if (velocity > -0.2f) index = 2;

        _audioSource.clip = _walkFootStepsClips[index];
        _audioSource.Play();
    }
}
