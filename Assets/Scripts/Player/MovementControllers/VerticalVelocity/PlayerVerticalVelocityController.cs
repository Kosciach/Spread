using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVerticalVelocityController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;                public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] Transform _playerTransform;                            public Transform PlayerTransform { get { return _playerTransform; } }
    [SerializeField] CharacterController _characterController;              public CharacterController CharacterController { get { return _characterController; } }

    [Space(5)]
    [SerializeField] PlayerVerticalVelocity_Gravity _gravity;               public PlayerVerticalVelocity_Gravity Gravity { get { return _gravity; } }
    [SerializeField] PlayerVerticalVelocity_Jump _jump;                     public PlayerVerticalVelocity_Jump Jump { get { return _jump; } }
    [SerializeField] PlayerVerticalVelocity_Slope _slope;                   public PlayerVerticalVelocity_Slope Slope { get { return _slope; } }
}
