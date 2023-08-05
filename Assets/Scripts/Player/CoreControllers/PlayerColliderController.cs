using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] CharacterController _characterController;
    [SerializeField] Transform _colliderBottom;
    [SerializeField] Transform _colliderTop;



    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] Vector3 _colliderCenterOffset;


    private ColliderRadiusLerper _colliderRadiusLerper;


    private void Awake()
    {
        _colliderRadiusLerper = new ColliderRadiusLerper(_characterController);
    }
    private void Update()
    {
        FitColliderToPlayer();
    }


    private void FitColliderToPlayer()
    {
        float colliderHeight = _colliderTop.position.y - _colliderBottom.position.y;
        Vector3 colliderPosition = new Vector3(0, colliderHeight / 2, 0) + _colliderCenterOffset;

        _characterController.center = colliderPosition;
        _characterController.height = colliderHeight;
    }


    public void ToggleCollider(bool enable)
    {
        _characterController.detectCollisions = enable;
    }
    public void SetColliderRadius(float radius, float lerpDuration)
    {
        if (_colliderRadiusLerper.LerpCoroutine != null) StopCoroutine(_colliderRadiusLerper.LerpCoroutine);

        _colliderRadiusLerper.LerpCoroutine = _colliderRadiusLerper.Lerp(radius, lerpDuration);
        StartCoroutine(_colliderRadiusLerper.LerpCoroutine);
    }
}


public class ColliderRadiusLerper
{
    private CharacterController _characterController;
    public IEnumerator LerpCoroutine;

    public ColliderRadiusLerper(CharacterController characterController)
    {
        _characterController = characterController;
    }


    public IEnumerator Lerp(float endRadius, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float time = timeElapsed / duration;

            _characterController.radius = Mathf.Lerp(_characterController.radius, endRadius, time);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _characterController.radius = endRadius;
    }
}