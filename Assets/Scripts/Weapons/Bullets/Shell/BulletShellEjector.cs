using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletShellEjector : MonoBehaviour
{
    [Header("====Refernces====")]
    [SerializeField] Transform _shellPrefab;


    private WeaponStateMachine _weaponStateMachine;



    private void Awake()
    {
        _weaponStateMachine = transform.parent.GetComponent<WeaponStateMachine>();
    }

    public void EjectShell()
    {
       BulletShellController shellController = Instantiate(_shellPrefab, transform.position, transform.rotation).GetComponent<BulletShellController>();
       shellController.PassData(_weaponStateMachine.PlayerStateMachine.CoreControllers.Input.MovementInputVector.x);
    }
}
