using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCameraRotationOffsetController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;
    [SerializeField] CineCameraRotationOffset _rotationOffset;

    private CineCameraRotationOffsetLerp _cineCameraRotationOffsetLerp;


    private void Awake()
    {
        _cineCameraRotationOffsetLerp = new CineCameraRotationOffsetLerp(_rotationOffset);
    }


    public void SetOffsetSmooth(Vector3 offset, float duration)
    {
        if (_cineCameraRotationOffsetLerp.LerpCoroutine != null) StopCoroutine(_cineCameraRotationOffsetLerp.LerpCoroutine);

        _cineCameraRotationOffsetLerp.LerpCoroutine = _cineCameraRotationOffsetLerp.Lerp(offset, duration);
        StartCoroutine(_cineCameraRotationOffsetLerp.LerpCoroutine);
    }
    public void SetOffsetRaw(Vector3 offset)
    {
        _rotationOffset.m_Offset = offset;
    }

}



public class CineCameraRotationOffsetLerp
{
    private CineCameraRotationOffset _rotationOffset;
    private IEnumerator _lerpCoroutine; public IEnumerator LerpCoroutine { get { return _lerpCoroutine; } set { _lerpCoroutine = value; } }


    public CineCameraRotationOffsetLerp(CineCameraRotationOffset rotationOffset)
    {
        _rotationOffset = rotationOffset;
    }


    public IEnumerator Lerp(Vector3 endOffset, float duration)
    {
        float timeElapsed = 0;

        while(timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _rotationOffset.m_Offset = Vector3.Lerp(_rotationOffset.m_Offset, endOffset, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _rotationOffset.m_Offset = endOffset;
    }
}