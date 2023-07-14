using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat_UnEquip : MonoBehaviour
{
    private PlayerCombatController _combatController;




    private void Awake()
    {
        _combatController = GetComponent<PlayerCombatController>();
    }
}
