using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMainPositioner : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3Lerp _move;
    [SerializeField] Vector3Lerp _rotate;

    public Vector3 PosVector { get { return _move.Vector; } }
    public Vector3 RotVector { get { return _rotate.Vector; } }



    public WeaponMainPositioner Move(Vector3 startVector, Vector3 endVector, float duration)
    {
        if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

        _move.LerpCoroutine = _move.Lerp(startVector, endVector, duration);
        StartCoroutine(_move.LerpCoroutine);

        return this;
    }
    public WeaponMainPositioner Move(Vector3 endVector, float duration)
    {
        if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

        _move.LerpCoroutine = _move.Lerp(_weaponAnimator.RightHandIk.parent.localPosition, endVector, duration);
        StartCoroutine(_move.LerpCoroutine);

        return this;
    }


    public WeaponMainPositioner Rotate(Vector3 startVector, Vector3 endVector, float duration)
    {
        if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

        _rotate.LerpCoroutine = _rotate.Lerp(startVector, endVector, duration);
        StartCoroutine(_rotate.LerpCoroutine);

        return this;
    }
    public WeaponMainPositioner Rotate(Vector3 endVector, float duration)
    {
        if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

        _rotate.LerpCoroutine = _rotate.Lerp(_weaponAnimator.RightHandIk.localRotation.eulerAngles, endVector, duration);
        StartCoroutine(_rotate.LerpCoroutine);

        return this;
    }




    public void SetOnMoveFinish(Action toDo)
    {
        _move.OnFinish = toDo;
    }
    public void SetOnRotationFinish(Action toDo)
    {
        _rotate.OnFinish = toDo;
    }
}

[System.Serializable]
public class Vector3Lerp
{
    [SerializeField] Vector3 _vector; public Vector3 Vector { get { return _vector; } }
    [SerializeField] bool _isLerping;
    public Action OnFinish;
    public IEnumerator LerpCoroutine;


    public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration)
    {
        float timeElapsed = 0;

        //Start

        _isLerping = true;
        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _vector = Vector3.Lerp(startVector, endVector, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //Finish

        _vector = endVector;
        _isLerping = false;
        if (OnFinish != null) OnFinish.Invoke();
    }
}
