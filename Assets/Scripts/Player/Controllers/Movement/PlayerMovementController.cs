using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _playerTransform; public Transform PlayerTransform { get { return _playerTransform; } }
    [SerializeField] CharacterController _characterController; public CharacterController CharacterController { get { return _characterController; } }
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }

    [Space(5)]
    [SerializeField] PlayerOnGroundMovementController _onGround; public PlayerOnGroundMovementController OnGround { get { return _onGround; } }
    [SerializeField] PlayerLadderMovementController _ladder; public PlayerLadderMovementController Ladder { get { return _ladder; } }
    [SerializeField] PlayerSwimMovementController _swim; public PlayerSwimMovementController Swim { get { return _swim; } }
    [SerializeField] PlayerInAirMovementController _inAir; public PlayerInAirMovementController InAir { get { return _inAir; } }
}




