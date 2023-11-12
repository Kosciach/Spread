using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    [Header("---References---")]
    [SerializeField] CharacterController _characterController;
    [SerializeField] Transform _playerTop;
    [SerializeField] Transform _playerBottom;

    [Space(20)]
    [Header("---Settings---")]
    [SerializeField, Range(-0.5f, 0.5f)] float _heightOffset;
    [SerializeField, Range(-0.5f, 0.5f)] float _centerOffset;

    [Space(20)]
    [Header("---Debugs---")]
    [SerializeField] float _colliderHeight;
    [SerializeField] float _colliderCenter;


    private void Update()
    {
        _colliderHeight = (_playerTop.position.y + _heightOffset) - _playerBottom.position.y;
        _colliderCenter = _colliderHeight / 2 + _centerOffset;

        _characterController.height = _colliderHeight;
        _characterController.center = new Vector3(0, _colliderCenter, 0);
    }
}
