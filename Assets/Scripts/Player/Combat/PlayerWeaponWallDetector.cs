using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponWallDetector : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Collider _collider;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _isWallDetected;




    private void Update()
    {
        _playerStateMachine.CombatControllers.EquipedWeapon.Wall.ToggleWallBool(_isWallDetected);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Player")) return;

        _isWallDetected = true;
        _playerStateMachine.CombatControllers.EquipedWeapon.Wall.Wall(_isWallDetected);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Player")) return;

        _isWallDetected = false;
        _playerStateMachine.CombatControllers.EquipedWeapon.Wall.Wall(_isWallDetected);
    }


    public void ToggleCollider(bool enable)
    {
        _collider.enabled = enable;
    }
}
