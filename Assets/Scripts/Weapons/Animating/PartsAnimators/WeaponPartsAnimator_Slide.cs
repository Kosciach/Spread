using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartsAnimator_Slide : BaseWeaponPartsAnimator
{
    [Header("====References====")]
    [SerializeField] Transform _slide;
    [Space(5)]
    [SerializeField] Vector3 _firedPosition;
    [SerializeField] Vector3 _defaultPosition;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] bool _isSlideForward;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 1)]
    [SerializeField] float _slideMoveForwardTime;
    [Range(0, 1)]
    [SerializeField] float _slideMoveBackTime;




    public override void OnShoot(bool isAmmoReadyToBeShoot)
    {
        if (isAmmoReadyToBeShoot) MoveBackAndForward();
        else MoveBack();
    }
    public override void OnReload()
    {
        MoveForward();
    }




    private void MoveBackAndForward()
    {
        _isSlideForward = false;
        _slide.LeanMoveLocal(_firedPosition, _slideMoveBackTime).setOnComplete(() =>
        {
            _slide.LeanMoveLocal(_defaultPosition, _slideMoveForwardTime);
            _isSlideForward = true;
        });
    }
    private void MoveBack()
    {
        _isSlideForward = false;
        _slide.LeanMoveLocal(_firedPosition, _slideMoveBackTime);
    }
    private void MoveForward()
    {
        _isSlideForward = true;
        _slide.LeanMoveLocal(_defaultPosition, _slideMoveForwardTime);
    }
}