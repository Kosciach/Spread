using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace LeftHandAnimatorNamespace
{
    public class LeftHand_RightHandFollow : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] LeftHandAnimator _leftHandAnimator;
        [SerializeField] TwoBoneIKConstraint _RHF;
        [SerializeField] TwoBoneIKConstraint _NRHF;

        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] Transform _leftHandIkForUpdate;
        [SerializeField] bool _followRightHand;



        private void Awake()
        {
            Toggle(true);
        }
        private void Update()
        {
            _leftHandIkForUpdate.position = _leftHandAnimator.LeftHandIk.position;
            _leftHandIkForUpdate.rotation = _leftHandAnimator.LeftHandIk.rotation;
        }


        public void Toggle(bool enable)
        {
            _followRightHand = enable;

            Action toggleMethod = enable ? Enable : Disable;
            toggleMethod();
        }

        private void Enable()
        {
            _RHF.weight = 1;
            _NRHF.weight = 0;

            _leftHandAnimator.LeftHandIk = _leftHandAnimator.LeftHandIks.RHF;
            _leftHandIkForUpdate = _leftHandAnimator.LeftHandIks.NRHF;
        }
        private void Disable()
        {
            _RHF.weight = 0;
            _NRHF.weight = 1;

            _leftHandAnimator.LeftHandIk = _leftHandAnimator.LeftHandIks.NRHF;
            _leftHandIkForUpdate = _leftHandAnimator.LeftHandIks.RHF;
        }
    }
}