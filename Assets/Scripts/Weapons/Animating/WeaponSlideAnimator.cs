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
    [SerializeField] float _slideMoveForwardTime;
    [Range(0, 1)]
    [SerializeField] float _slideMoveBackTime;


    private Action[] _slideAnimMethod = new Action[3];



    public enum SlideAnimType
    {
        BackAndForward, Back, Forward
    }



    private void Awake()
    {
        _slideAnimMethod[0] = MoveBackAndForward;
        _slideAnimMethod[1] = MoveBack;
        _slideAnimMethod[2] = MoveForward;
    }





    public void MoveSlide(SlideAnimType slideAnim)
    {
        int animIndex = (int)slideAnim;
        _slideAnimMethod[animIndex]();
    }


    private void MoveBackAndForward()
    {
        _slide.LeanMoveLocal(_firedPosition, _slideMoveBackTime).setOnComplete(() =>
        {
            _slide.LeanMoveLocal(_defaultPosition, _slideMoveForwardTime);
        });
    }
    private void MoveBack()
    {
        _slide.LeanMoveLocal(_firedPosition, _slideMoveBackTime);
    }
    private void MoveForward()
    {
        _slide.LeanMoveLocal(_defaultPosition, _slideMoveForwardTime);
    }
}
