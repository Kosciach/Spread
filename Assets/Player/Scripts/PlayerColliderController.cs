using UnityEngine;

namespace Player
{
    public class PlayerColliderController : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;
        private CharacterController _characterController;

        [Header("--References--")]
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _bottom;

        private void Awake()
        {
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float height = _top.position.y - _bottom.position.y;
            float center = height / 2;

            _characterController.height = height;
            _characterController.center = new Vector3(_characterController.center.x, center, _characterController.center.z);
        }
    }
}