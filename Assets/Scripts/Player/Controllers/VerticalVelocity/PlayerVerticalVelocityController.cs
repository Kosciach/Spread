using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVerticalVelocityController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] PlayerGravityController _gravityController; public PlayerGravityController GravityController { get { return _gravityController; } }
    [SerializeField] PlayerJumpController _jumpController; public PlayerJumpController JumpController { get { return _jumpController; } }
    [SerializeField] PlayerSlopeController _slopeController; public PlayerSlopeController SlopeController { get { return _slopeController; } }

    [Space(5)]
    [SerializeField] Transform _playerTransform; public Transform PlayerTransform { get { return _playerTransform; } }
    [SerializeField] CharacterController _characterController; public CharacterController CharacterController { get { return _characterController; } }
}
