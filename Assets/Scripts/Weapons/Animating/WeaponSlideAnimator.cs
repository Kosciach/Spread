using System;
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


    private Action[] _slideAnimMethod = new Action[2];


    private void Awake()
    {
        _slideAnimMethod[0] = MoveBack;
        _slideAnimMethod[1] = MoveBackAndForward;
    }





    public void MoveSlide(bool moveForward)
    {
        int animIndex = moveForward ? 1 : 0;
        _slideAnimMethod[animIndex]();
    }


    private void MoveBackAndForward()
    {
        _slide.LeanMoveLocal(_firedPosition, _slideMoveBackTime).setOnComplete(() =>
        {
            _slide.LeanMoveLocal(_defaultPosition, _slideResetTime);
        });
    }
    private void MoveBack()
    {
        _slide.LeanMoveLocal(_firedPosition, _slideMoveBackTime);
    }
}
