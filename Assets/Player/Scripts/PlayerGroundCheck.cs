using System.Collections.Generic;
using Tools;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Player
{
    public class PlayerGroundCheck : MonoBehaviour
    {
        private PlayerStateMachineContext _playerContext;

        [Header("--Settings--")]
        [SerializeField] Vector3 _originOffset;
        [Space(5)]
        [Range(0.1f, 1)][SerializeField] private float _range;
        [Range(0.1f, 2)][SerializeField] private float _radius;
        [Range(3, 50)][SerializeField] private int _precision;
        [Space(5)]
        [SerializeField] private LayerMask _mask;

        [Space(20)]
        [Header("--Debugs--")]
        [SerializeField, ReadOnly] private List<bool> _groundChecks = new();
        [SerializeField, ReadOnly] private bool _isGrounded = true; public bool IsGrounded => _isGrounded;

        private Vector3 _origin => transform.position + _originOffset;


        private void Awake()
        {
            _playerContext = GetComponent<PlayerStateMachine>().Ctx;
        }
        private void Update()
        {
            _groundChecks.Clear();

            Debug.DrawRay(_origin, Vector3.down * _range, Color.blue);
            _groundChecks.Add(Physics.Raycast(_origin, Vector3.down, out RaycastHit hit, _range, _mask));

            for (int i = 0; i < _precision; i++)
            {
                float angle = i * 360 / _precision;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                Vector3 origin = _origin + direction * _radius;

                ShootGroundCheckRay(origin);
            }

            _isGrounded = _groundChecks.Contains(true);
        }

        private void ShootGroundCheckRay(Vector3 pos)
        {
            Debug.DrawRay(pos, Vector3.down * _range, Color.red);
            _groundChecks.Add(Physics.Raycast(pos, Vector3.down, _range, _mask));
        }
    }
}