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
    [SerializeField] float _stamina;
    [SerializeField] float _lowStaminaBobStrength; public float LowStaminaBobStrength { get { return _lowStaminaBobStrength; } }
    [SerializeField] float _weaponWeight;
    [SerializeField] bool _useStamina;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 2)]
    [SerializeField] float _staminaRecoverSpeed;
    [Range(0, 2)]
    [SerializeField] float _staminaUseSpeed;

    private float _staminaControll = 10;



    private void Update()
    {
        UpdateStamina();
    }



    public void ToggleUseStamina(bool useStamina)
    {
        _useStamina = useStamina;
        _staminaControll = _useStamina ? (-_staminaUseSpeed * _weaponWeight) : (_staminaRecoverSpeed / _weaponWeight);
    }

    private void UpdateStamina()
    {
        _stamina += _staminaControll * 10 * Time.deltaTime;
        _stamina = Mathf.Clamp(_stamina, 0, 100);

        CheckStamina();

        CanvasController.Instance.HudControllers.Stats.UpdateWeaponStamina(_stamina / 100);
    }


    private void CheckStamina()
    {
        _lowStaminaBobStrength = _stamina <= 35 ? (36 - _stamina)/10 : 0;
    }
}
