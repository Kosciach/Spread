using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMoveController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _mainCameraHolder;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _desiredPosition;
    [SerializeField] Vector3 _currentPosition;
    [SerializeField] float _lerpSpeed;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3 _basePosition;
    [Range(0, 1)]
    [SerializeField] int _lockCameraMove;


    private void Update()
    {
        Move();
    }


    private void Move()
    {
        _currentPosition = Vector3.Lerp(_currentPosition, _desiredPosition, _lerpSpeed * _lockCameraMove * Time.deltaTime);
        _mainCameraHolder.localPosition = _currentPosition;
    }


    public void SetPosition(Vector3 offset, float lerpSpeed)
    {
        _desiredPosition = _basePosition + offset;
        _lerpSpeed = lerpSpeed;
    }
}
