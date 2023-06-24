using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats_RangeWeaponStamina : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStatsController _statsController;


    [Space(20)]
    [Header("====Debugs====")]
    [Range(0, 100)]
    [SerializeField] float _holdingStamina;
    [SerializeField] float _weaponWeight = 2;

    private int _staminaControll = 1;



    private void Update()
    {
        UpdateStamina();
    }





    public void SetWeaponWeight(float weaponWeight)
    {
        _weaponWeight = weaponWeight;
    }

    public void SetRestoreStamina()
    {
        _staminaControll = 1;
    }
    public void SetUseStamina()
    {
        _staminaControll = -1;
    }


    private void UpdateStamina()
    {
        _holdingStamina += _staminaControll * _weaponWeight * 5 * Time.deltaTime;
        _holdingStamina = Mathf.Clamp(_holdingStamina, 0, 100);
    }
}
