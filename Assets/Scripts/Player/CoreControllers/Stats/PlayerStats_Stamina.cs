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
    [SerializeField] bool _useStamina;
    [SerializeField] bool _canUseStamina; public bool CanUseStamina { get { return _canUseStamina; } }


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _staminaRecoverSpeed;
    [Range(0, 10)]
    [SerializeField] float _staminaUseSpeed;

    private float _staminaControll = 10;




    private void Update()
    {
        UpdateStamina();
    }



    public void ToggleUseStamina(bool useStamina)
    {
        _useStamina = useStamina;
        _staminaControll = _useStamina ? -_staminaUseSpeed : _staminaRecoverSpeed;
    }

    private void UpdateStamina()
    {
        _stamina += _staminaControll * 10 * Time.deltaTime;
        _stamina = Mathf.Clamp(_stamina, 0, 100);

        CheckCanUseStamina();
        CanvasController.Instance.HudControllers.Stats.UpdateStamina(_stamina / 100);
    }

    private void CheckCanUseStamina()
    {
        if (!_canUseStamina)
        {
            _canUseStamina = _stamina == 100;
            return;
        }
        if (_stamina == 0)
        {
            _canUseStamina = false;
        }
    }
}
