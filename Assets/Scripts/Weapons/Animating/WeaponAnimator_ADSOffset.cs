using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace WeaponAnimatorNamespace
{
    public class WeaponAnimator_ADSOffset : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] WeaponAnimator _weaponAnimator;


        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] WeaponAnimator.PosRotStruct _outputVectors; public WeaponAnimator.PosRotStruct OutputVectors { get { return _outputVectors; } }
        [SerializeField] WeaponAnimator.PosRotStruct _rawVectors;

        [Range(0, 1)]
        [SerializeField] float _toggle;




        private void Update()
        {
            ApplyToggle();
        }


        private void ApplyToggle()
        {
            _outputVectors.Pos = _rawVectors.Pos * _toggle;
            _outputVectors.Rot = _rawVectors.Rot * _toggle;
        }


        public void Toggle(bool enable)
        {
            int toggleWeight = enable ? 1 : 0;
            LeanTween.value(_toggle, toggleWeight, 0.2f).setOnUpdate((float val) =>
            {
                _toggle = val;
            });
        }
    }
}