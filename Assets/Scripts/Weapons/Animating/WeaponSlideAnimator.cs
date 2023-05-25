using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlideAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _slide;
    [Space(5)]
    [SerializeField] Vector3 _firedPosition;
    [SerializeField] Vector3 _defaultPosition;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 1)]
    [SerializeField] float _slideResetTime;
    [Range(0, 1)]
    [SerializeField] float _slideMoveBackTime;


    public void MoveSlide()
    {
        _slide.LeanMoveLocal(_firedPosition, _slideMoveBackTime).setEaseInQuart().setOnComplete(() =>
        {
            _slide.LeanMoveLocal(_defaultPosition, _slideResetTime).setEaseInExpo();
        });
    }
}
