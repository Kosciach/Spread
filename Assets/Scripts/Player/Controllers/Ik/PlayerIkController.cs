using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIkController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] PlayerIkLayerController _layers; public PlayerIkLayerController Layers { get { return _layers; } }
    [SerializeField] PlayerFingerIkController _fingers; public PlayerFingerIkController Fingers { get { return _fingers; } }
    [SerializeField] WeaponIkAnimator _weaponAnimator; public WeaponIkAnimator WeaponAnimator { get { return _weaponAnimator; } }
}
