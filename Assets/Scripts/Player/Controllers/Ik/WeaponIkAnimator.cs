using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIkAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerIkController _ikController;
    [SerializeField] IkHandsTargetsStruct _ikHandsTargets; public IkHandsTargetsStruct IkHandsTargets { get { return _ikHandsTargets; } }


    [System.Serializable]
    public struct IkHandsTargetsStruct
    {
        public Transform Right;
        public Transform Left;
    }
}
