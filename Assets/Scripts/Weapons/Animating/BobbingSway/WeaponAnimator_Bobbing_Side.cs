using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponAnimatorNamespace
{
    public class WeaponAnimator_Bobbing_Side : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] WeaponAnimator_Bobbing _bobbingController;



        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] WeaponAnimator.PosRotStruct _rawVectors;
        [SerializeField] WeaponAnimator.PosRotStruct _smoothVectors; public WeaponAnimator.PosRotStruct SmoothVectors { get { return _smoothVectors; } }



        [Space(20)]
        [Header("====Settings====")]
        [Range(0, 10)]
        [SerializeField] float _strength;
        [Range(0, 10)]
        [SerializeField] float _smoothSpeed;



        private void Update()
        {
            SetVectors();
            SmoothOutVectors();
        }




        private void SetVectors()
        {
            Vector3 movementInputVector = _bobbingController.WeaponAnimator.PlayerStateMachine.CoreControllers.Input.MovementInputVector;
            _rawVectors.Rot = new Vector3(movementInputVector.z * _strength / 3, 0, movementInputVector.x * -_strength);
        }

        private void SmoothOutVectors()
        {
            float aimWeight = _bobbingController.WeaponAnimator.PlayerStateMachine.CombatControllers.EquipedWeapon.Aim.IsAim ? 0.3f : 1;
            _smoothVectors.Rot = Vector3.Lerp(_smoothVectors.Rot, _rawVectors.Rot * aimWeight, _smoothSpeed * (_strength / 2) * Time.deltaTime);
        }
    }
}