using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerEquipedWeaponController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCombatController _combatController; public PlayerCombatController CombatController { get { return _combatController; } }
    [SerializeField] PlayerEquipedWeaponHoldController _hold; public PlayerEquipedWeaponHoldController Hold { get { return _hold; } }
    [SerializeField] PlayerAimController _aim; public PlayerAimController Aim { get { return _aim; } }
    [SerializeField] PlayerBlockController _block; public PlayerBlockController Block { get { return _block; } }
    [SerializeField] PlayerWeaponRunController _run; public PlayerWeaponRunController Run { get { return _run; } }
}
