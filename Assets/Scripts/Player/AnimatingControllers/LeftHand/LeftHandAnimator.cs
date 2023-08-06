using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftHandAnimatorNamespace
{
    public class LeftHandAnimator : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerStateMachine _playerStateMachine;        public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
        [SerializeField] LeftHandIksStruct _leftHandIks;                public LeftHandIksStruct LeftHandIks { get { return _leftHandIks; } }
        [Space(5)]
        [SerializeField] LeftHand_MainTransformer _mainTransformer;     public LeftHand_MainTransformer MainTransformer { get { return _mainTransformer; } }
        [SerializeField] LeftHand_RightHandFollow _rightHandFollow;     public LeftHand_RightHandFollow RightHandFollow { get { return _rightHandFollow; } }


        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] Transform _leftHandIk; public Transform LeftHandIk { get { return _leftHandIk; } set { _leftHandIk = value; } }



        [System.Serializable]
        public struct LeftHandIksStruct
        {
            public Transform RHF;
            public Transform NRHF;
        }
    }
}