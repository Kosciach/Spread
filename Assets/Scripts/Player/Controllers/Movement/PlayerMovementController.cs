using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerMovement_OnGround _onGround;             public PlayerMovement_OnGround OnGround { get { return _onGround; } }
    [SerializeField] PlayerMovement_Ladder _ladder;                 public PlayerMovement_Ladder Ladder { get { return _ladder; } }
    [SerializeField] PlayerMovement_Swim _swim;                     public PlayerMovement_Swim Swim { get { return _swim; } }
    [SerializeField] PlayerMovement_InAir _inAir;                   public PlayerMovement_InAir InAir { get { return _inAir; } }

    [Space(5)]
    [SerializeField] Transform _playerTransform;                    public Transform PlayerTransform { get { return _playerTransform; } }
    [SerializeField] CharacterController _characterController;      public CharacterController CharacterController { get { return _characterController; } }
    [SerializeField] PlayerStateMachine _playerStateMachine;        public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
}




