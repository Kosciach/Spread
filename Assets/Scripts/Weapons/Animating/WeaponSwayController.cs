using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WeaponSwayController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator; public WeaponAnimator WeaponAnimator { get { return _weaponAnimator; } }



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _swayRot; public Vector3 SwayRot { get { return _swayRot; } }
    [Range(0, 1)]
    [SerializeField] int _swayToggle;


    [Space(10)]
    [Header("====SwayStructs====")]
    [SerializeField] SwayValues _horizontal;
    [SerializeField] SwayValues _vertical;


    [System.Serializable]
    public struct SwayValues
    {
        [Header("====Debugs====")]
        public float DesiredSway;
        public float CurrentSway;

        [Space(20)]
        [Header("====Settings====")]
        [Range(0, 10)]
        public float Strength;
        [Range(0, 10)]
        public float Speed;
        [Range(0, 10)]
        public float MaxSway;
    }



    private void Update()
    {
        GetHorizontalSway();
        GetVerticalSway();

        SetSway();
    }




    private void GetHorizontalSway()
    {
        _horizontal.DesiredSway = _weaponAnimator.PlayerStateMachine.InputController.MouseInputVector.x / _horizontal.Strength;
        _horizontal.CurrentSway = Mathf.Lerp(_horizontal.CurrentSway, _horizontal.DesiredSway, _horizontal.Speed * Time.deltaTime);
        _horizontal.CurrentSway = Mathf.Clamp(_horizontal.CurrentSway, -_horizontal.MaxSway, _horizontal.MaxSway);
    }
    private void GetVerticalSway()
    {
        _vertical.DesiredSway = _weaponAnimator.PlayerStateMachine.InputController.MouseInputVector.y / _vertical.Strength;
        _vertical.CurrentSway = Mathf.Lerp(_vertical.CurrentSway, _vertical.DesiredSway, _vertical.Speed * Time.deltaTime);
        _vertical.CurrentSway = Mathf.Clamp(_vertical.CurrentSway, -_vertical.MaxSway, _vertical.MaxSway);
    }
    private void SetSway()
    {
        _swayRot = new Vector3(-_vertical.CurrentSway, _horizontal.CurrentSway, 0) * _swayToggle; 
    }


    public void Toggle(bool enable)
    {
        _swayToggle = enable ? 1 : 0;
    }
}
