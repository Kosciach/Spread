using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsHudController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] BarAndIcon _health;
    [SerializeField] BarAndIcon _armor;
    [SerializeField] BarAndIcon _stamina;
    [SerializeField] BarAndIcon _weaponStamina;



    [System.Serializable]
    private struct BarAndIcon
    {
        public Image Icon;
        public Image Bar;
    }





    public void UpdateHealth(float health)
    {
        LeanTween.scaleX(_health.Bar.gameObject, health, 0.1f);
    }
    public void UpdateArmor(float armor)
    {
        LeanTween.scaleX(_armor.Bar.gameObject, armor, 0.1f);
    }
    public void UpdateStamina(float stamina)
    {
        _stamina.Bar.rectTransform.localScale = new Vector3(stamina, 1, 1);
    }
    public void UpdateWeaponStamina(float weaponStamina)
    {
        _weaponStamina.Bar.rectTransform.localScale = new Vector3(1, weaponStamina, 1);
    }
}
