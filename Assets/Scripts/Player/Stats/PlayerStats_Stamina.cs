using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats_Stamina : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStatsController _statsController;


    [Space(20)]
    [Header("====Debugs====")]
    [Range(0, 100)]
    [SerializeField] float _stamina;
}
