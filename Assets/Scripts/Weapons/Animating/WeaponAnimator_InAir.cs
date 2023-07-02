using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponAnimatorNamespace
{
    public class WeaponAnimator_InAir : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] WeaponAnimator _weaponAnimator;



        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] WeaponAnimator.PosRotStruct _currentVectors; public WeaponAnimator.PosRotStruct CurrentVectors { get { return _currentVectors; } }
        [Space(5)]
        [SerializeField] WeaponAnimator.PosRotStruct _desiredVectors;
        [Space(10)]
        [Range(0, 1)]
        [SerializeField] int _toggle;



        [Space(20)]
        [Header("====Settings====")]
        [Range(0, 20)]
        [SerializeField] float _posSmoothSpeed;
        [Range(0, 20)]
        [SerializeField] float _rotSmoothSpeed;
        [Space(5)]
        [Range(0, 0.1f)]
        [SerializeField] float _maxPos;
        [Range(0, 45)]
        [SerializeField] float _maxRot;


        private PlayerVerticalVelocityController _verticalVelocity;
        private float _gravityStrength => _verticalVelocity.Gravity.CurrentGravityForce - _verticalVelocity.Slope.SlopeAngle;




        private void Awake()
        {
            _verticalVelocity = _weaponAnimator.PlayerStateMachine.MovementControllers.VerticalVelocity;
        }
        private void Update()
        {
            SetVectors();
            UpdateVectors();
        }




        private void SetVectors()
        {
            _desiredVectors.Pos.y = _gravityStrength / 90;
            _desiredVectors.Rot.x = _gravityStrength * 4;
        }
        private void UpdateVectors()
        {
            _currentVectors.Pos = Vector3.Lerp(_currentVectors.Pos, _desiredVectors.Pos, _posSmoothSpeed * Time.deltaTime) * _toggle;
            _currentVectors.Pos = Vector3.ClampMagnitude(_currentVectors.Pos, _maxPos);

            _currentVectors.Rot = Vector3.Lerp(_currentVectors.Rot, _desiredVectors.Rot, _rotSmoothSpeed * Time.deltaTime) * _toggle;
            _currentVectors.Rot = Vector3.ClampMagnitude(_currentVectors.Rot, _maxRot);
        }



        public void SetPosSpeed(float speed)
        {
            _posSmoothSpeed = speed;
        }
        public void SetRotSpeed(float speed)
        {
            _rotSmoothSpeed = speed;
        }



        public void Toggle(bool enable)
        {
            _toggle = enable ? 1 : 0;
        }
    }
}