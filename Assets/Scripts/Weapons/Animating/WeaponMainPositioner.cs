using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMainPositioner : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;
    [SerializeField] Transform _positioner;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] VectorUpdateType _vectorUpdateType;
    [Space(5)]
    [SerializeField] Vector3 _pos; public Vector3 Pos { get { return _pos; } }
    [SerializeField] Quaternion _rot; public Quaternion Rot { get { return _rot; } }


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3Lerp _move;
    [SerializeField] QuaternionLerp _rotate;


    private Action[] _vectorUpdateMethods = new Action[2];

    private enum VectorUpdateType
    {
        Gameplay, SettingUp
    }





    private void Awake()
    {
        _vectorUpdateMethods[0] = UpdateVectorsGameplay;
        _vectorUpdateMethods[1] = UpdateVectorsSettingUp;
    }
    public void Update()
    {
        _vectorUpdateMethods[(int)_vectorUpdateType]();
    }


    private void UpdateVectorsGameplay()
    {
        _pos = _move.Vector;
        _rot = _rotate.Quaternion;


        _positioner.localPosition = _pos;
        _positioner.localRotation = _rot;
    }
    private void UpdateVectorsSettingUp()
    {
        _pos = _positioner.localPosition;
        _rot = _positioner.localRotation;
    }



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
    public WeaponMainPositioner Move(Vector3 startVector, Vector3 endVector, float duration, AnimationCurve curve)
    {
        if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

        _move.LerpCoroutine = _move.Lerp(startVector, endVector, duration, curve);
        StartCoroutine(_move.LerpCoroutine);

        return this;
    }
    public WeaponMainPositioner Move(Vector3 endVector, float duration, AnimationCurve curve)
    {
        if (_move.LerpCoroutine != null) StopCoroutine(_move.LerpCoroutine);

        _move.LerpCoroutine = _move.Lerp(_weaponAnimator.RightHandIk.parent.localPosition, endVector, duration, curve);
        StartCoroutine(_move.LerpCoroutine);

        return this;
    }
    public void MoveRaw(Vector3 pos)
    {
        _move.SetRaw(pos);
    }
    public void SetOnMoveFinish(Action toDo)
    {
        _move.OnFinish = toDo;
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
    public WeaponMainPositioner Rotate(Vector3 startVector, Vector3 endVector, float duration, AnimationCurve curve)
    {
        if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

        _rotate.LerpCoroutine = _rotate.Lerp(startVector, endVector, duration, curve);
        StartCoroutine(_rotate.LerpCoroutine);

        return this;
    }
    public WeaponMainPositioner Rotate(Vector3 endVector, float duration, AnimationCurve curve)
    {
        if (_rotate.LerpCoroutine != null) StopCoroutine(_rotate.LerpCoroutine);

        _rotate.LerpCoroutine = _rotate.Lerp(_weaponAnimator.RightHandIk.localRotation.eulerAngles, endVector, duration, curve);
        StartCoroutine(_rotate.LerpCoroutine);

        return this;
    }
    public void RotateRaw(Vector3 rot)
    {
        _rotate.SetRaw(rot);
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
        OnFinish = null;
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
    public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration, AnimationCurve curve)
    {
        OnFinish = null;
        float timeElapsed = 0;

        //Start

        _isLerping = true;
        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;
            time = curve.Evaluate(time);

            _vector = Vector3.Lerp(startVector, endVector, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //Finish

        _vector = endVector;
        _isLerping = false;
        if (OnFinish != null) OnFinish.Invoke();
    }
    public void SetRaw(Vector3 pos)
    {
        _vector = pos;

        _isLerping = false;
        OnFinish = null;
        LerpCoroutine = null;
    }
}


[System.Serializable]
public class QuaternionLerp
{
    [SerializeField] Quaternion _quaternion; public Quaternion Quaternion { get { return _quaternion; } }
    [SerializeField] bool _isLerping;
    public Action OnFinish;
    public IEnumerator LerpCoroutine;


    public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration)
    {
        OnFinish = null;
        float timeElapsed = 0;
        Quaternion startQuaternion = Quaternion.Euler(startVector);
        Quaternion endQuaternion = Quaternion.Euler(endVector);

        //Start

        _isLerping = true;
        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _quaternion = Quaternion.Lerp(startQuaternion, endQuaternion, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //Finish

        _quaternion = endQuaternion;
        _isLerping = false;
        if (OnFinish != null) OnFinish.Invoke();
    }
    public IEnumerator Lerp(Vector3 startVector, Vector3 endVector, float duration, AnimationCurve curve)
    {
        OnFinish = null;
        float timeElapsed = 0;
        Quaternion startQuaternion = Quaternion.Euler(startVector);
        Quaternion endQuaternion = Quaternion.Euler(endVector);

        //Start

        _isLerping = true;
        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;
            time = curve.Evaluate(time);

            _quaternion = Quaternion.Lerp(startQuaternion, endQuaternion, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //Finish

        _quaternion = endQuaternion;
        _isLerping = false;
        if (OnFinish != null) OnFinish.Invoke();
    }

    public void SetRaw(Vector3 rot)
    {
        _quaternion = Quaternion.Euler(rot);


        _isLerping = false;
        OnFinish = null;
        LerpCoroutine = null;
    }
}