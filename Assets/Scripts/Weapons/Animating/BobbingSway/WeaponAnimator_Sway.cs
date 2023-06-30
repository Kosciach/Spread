using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WeaponAnimator_Sway : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] WeaponAnimator.PosRotStruct _currentSwayVectors; public WeaponAnimator.PosRotStruct CurrentSwayVectors { get { return _currentSwayVectors; } }
    private WeaponAnimator.PosRotStruct _desiredSwayVectors;
    [Range(0, 1)]
    [SerializeField] int _swayToggle;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _smoothSpeedPos;
    [Range(0, 10)]
    [SerializeField] float _smoothSpeedRot;


    [Space(10)]
    [Header("====SwayStructs====")]
    [SerializeField] SwayValues _horizontal;
    [SerializeField] SwayValues _vertical;




    [System.Serializable]
    public struct SwayValues
    {
        [Range(0, 2)]
        public float Weight;

        [Space(5)]
        [Range(0, 10)]
        public float PosStrength;
        [Range(0, 5)]
        public float PosLimit;

        [Space(5)]
        [Range(0, 10)]
        public float RotStrength;
        [Range(0, 20)]
        public float RotLimit;
    }




    private void Update()
    {
        HorizonstalSway();
        VerticalSway();

        SmoothOutSway();
    }






    private void HorizonstalSway()
    {
        Vector3 mouseInputVector = _weaponAnimator.PlayerStateMachine.CoreControllers.Input.MouseInputVector;

        //Pos
        float swayPos = (mouseInputVector.x / 1000) * _horizontal.PosStrength;
        swayPos = Mathf.Clamp(swayPos, -_horizontal.PosLimit/20, _horizontal.PosLimit/20);
        _desiredSwayVectors.Pos.x = -swayPos * _horizontal.Weight;

        //Rot
        float swayRot = (mouseInputVector.x / 10) * _horizontal.RotStrength;
        swayRot = Mathf.Clamp(swayRot, -_horizontal.RotLimit, _horizontal.RotLimit);
        _desiredSwayVectors.Rot.y = swayRot * _horizontal.Weight;
        _desiredSwayVectors.Rot.z = -swayRot * _horizontal.Weight;
    }

    private void VerticalSway()
    {
        Vector3 mouseInputVector = _weaponAnimator.PlayerStateMachine.CoreControllers.Input.MouseInputVector;

        //Pos
        float swayPos = (mouseInputVector.y / 1000) * _vertical.PosStrength;
        swayPos = Mathf.Clamp(swayPos, -_vertical.PosLimit / 20, _vertical.PosLimit / 20);
        _desiredSwayVectors.Pos.y = -swayPos * _vertical.Weight;


        //Rot
        float swayRot = (mouseInputVector.y / 10) * _vertical.RotStrength;
        swayRot = Mathf.Clamp(swayRot, -_vertical.RotLimit, _vertical.RotLimit);
        _desiredSwayVectors.Rot.x = -swayRot * _vertical.Weight;
    }

    private void SmoothOutSway()
    {
        _currentSwayVectors.Pos = Vector3.Lerp(_currentSwayVectors.Pos, _desiredSwayVectors.Pos, _smoothSpeedPos * Time.deltaTime);
        _currentSwayVectors.Rot = Vector3.Lerp(_currentSwayVectors.Rot, _desiredSwayVectors.Rot, _smoothSpeedRot * Time.deltaTime);
    }



    public void SetWeight(float weight)
    {
        _horizontal.Weight = weight;
        _vertical.Weight = weight;
    }


    public void Toggle(bool enable)
    {
        _swayToggle = enable ? 1 : 0;
    }
}
