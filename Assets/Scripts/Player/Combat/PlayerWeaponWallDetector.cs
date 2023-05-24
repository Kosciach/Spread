using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponWallDetector : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Collider _collider;







    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Player")) return;

        _playerStateMachine.CombatController.EquipedWeaponController.Wall.ToggleWall(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Player")) return;

        _playerStateMachine.CombatController.EquipedWeaponController.Wall.ToggleWall(false);
    }


    public void ToggleCollider(bool enable)
    {
        _collider.enabled = enable;
    }
}
