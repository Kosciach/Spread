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
    [Header("====Debugs====")]
    [SerializeField] Vector3 _velocity; public Vector3 Velocity { get { return _velocity; } }
    [SerializeField] float _desiredColliderRadius;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _colliderRadiusChangeSpeed;





    private void Update()
    {
        FitColliderToPlayer();
        LerpColliderRadius();


        _velocity = _characterController.velocity;
    }



    private void LerpColliderRadius()
    {
        _characterController.radius = Mathf.Lerp(_characterController.radius, _desiredColliderRadius, _colliderRadiusChangeSpeed * Time.deltaTime);
    }

    private void FitColliderToPlayer()
    {
        float colliderHeight = _colliderTop.position.y - _colliderBottom.position.y;
        Vector3 colliderPosition = new Vector3(0, colliderHeight / 2, 0);

        _characterController.center = colliderPosition;
        _characterController.height = colliderHeight;
    }


    public void ToggleCollider(bool enable)
    {
        _characterController.detectCollisions = enable;
    }
    public void SetColliderRadius(float radius)
    {
        _desiredColliderRadius = radius;
    }
}
