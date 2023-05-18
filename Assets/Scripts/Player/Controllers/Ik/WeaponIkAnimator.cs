using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIkAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerIkController _ikController; public PlayerIkController IkController { get { return _ikController; } }
    [SerializeField] WeaponSwayController _sway; public WeaponSwayController Sway { get { return _sway; } }
    [SerializeField] IkHandsTargetsStruct _ikHandsTargets; public IkHandsTargetsStruct IkHandsTargets { get { return _ikHandsTargets; } }

    [SerializeField] Vector3 _desiredRightHandPosition; public Vector3 DesiredRightHandPosition { get { return _desiredRightHandPosition; } set { _desiredRightHandPosition = value; } }



    [System.Serializable]
    public struct IkHandsTargetsStruct
    {
        public Transform Right;
        public Transform Left;
    }
}
