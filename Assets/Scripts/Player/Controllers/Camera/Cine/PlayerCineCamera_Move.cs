using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCineCamera_Move : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cameraController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] CameraPositionsEnum _cameraPositionType;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3[] _cameraPositions;


    private CineCameraMove _cineCameraMove;


    public enum CameraPositionsEnum
    {
        OnGround, Swim, Ladder, AttachmentTable
    }





    private void Awake()
    {
        _cineCameraMove = new CineCameraMove(_cameraController.MainCameraHolder);
    }

    public void SetCameraPosition(CameraPositionsEnum cameraPosition, float duration)
    {
        _cameraPositionType  = cameraPosition;

        if (_cineCameraMove.LerpCoroutine != null) StopCoroutine(_cineCameraMove.LerpCoroutine);

        _cineCameraMove.LerpCoroutine = _cineCameraMove.MoveSmooth(_cameraPositions[(int)cameraPosition], duration);
        StartCoroutine(_cineCameraMove.LerpCoroutine);
    }
}



public class CineCameraMove
{
    private Transform _mainCameraHolder;
    private IEnumerator _lerpCoroutine; public IEnumerator LerpCoroutine { get { return _lerpCoroutine; } set { _lerpCoroutine = value; } }


    public CineCameraMove(Transform mainCameraHolder)
    {
        _mainCameraHolder = mainCameraHolder;
    }


    public IEnumerator MoveSmooth(Vector3 endPos, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _mainCameraHolder.localPosition = Vector3.Lerp(_mainCameraHolder.localPosition, endPos, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _mainCameraHolder.localPosition = endPos;
    }
}