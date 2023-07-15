using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;

namespace WeaponAnimatorNamespace
{
    public class WeaponAnimator_Bobbing : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] WeaponAnimator _weaponAnimator; public WeaponAnimator WeaponAnimator { get { return _weaponAnimator; } }
        [SerializeField] WeaponAnimator_Bobbing_Base _base; public WeaponAnimator_Bobbing_Base Base { get { return _base; } }
        [SerializeField] WeaponAnimator_Bobbing_Side _side; public WeaponAnimator_Bobbing_Side Side { get { return _side; } }



        [Space(20)]
        [Header("====Debugs====")]
        [Range(0, 1)]
        [SerializeField] int _bobbingToggle; public int BobbingToggle { get { return _bobbingToggle; } }



        [Space(20)]
        [Header("====Settings====")]
        [Range(0, 5)]
        [SerializeField] float _playerVelocitySmoothSpeed;


        [System.Serializable]
        public struct MainBobVectorsStruct
        {
            public Vector3 Pos;
            public Vector3 Rot;
        }



        public void Toggle(bool enable)
        {
            _bobbingToggle = enable ? 1 : 0;
        }
    }
}