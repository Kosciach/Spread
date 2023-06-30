using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [Space(5)]
    [SerializeField] StatsStruct _stats;                            public StatsStruct Stats { get { return _stats; } }



    [System.Serializable]
    public struct StatsStruct
    {
        public PlayerStats_Health Health;
        public PlayerStats_Stamina Stamina;
        public PlayerStats_RangeWeaponStamina RangeWeaponStamina;
    }
}
