using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsHudController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Image _healthBar;
    [SerializeField] Image _staminaBar;
    [SerializeField] Image _weaponStaminaBar;
    [Space(5)]
    [SerializeField] Image _healthIcon;
    [SerializeField] Image _staminasIcon;








    public void UpdateHealth(float health)
    {
        LeanTween.scaleX(_healthBar.gameObject, health, 0.1f);
    }
    public void UpdateStamina(float stamina)
    {
        LeanTween.scaleX(_staminaBar.gameObject, stamina, 0.1f);
    }
    public void UpdateWeaponStamina(float weaponStamina)
    {
        LeanTween.scaleX(_weaponStaminaBar.gameObject, weaponStamina, 0.1f);
    }
}
