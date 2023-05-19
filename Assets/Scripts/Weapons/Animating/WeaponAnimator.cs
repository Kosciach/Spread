using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [Space(5)]
    [SerializeField] WeaponSwayController _sway; public WeaponSwayController Sway { get { return _sway; } }
    [SerializeField] WeaponBobbingController _bobbing; public WeaponBobbingController Bobbing { get { return _bobbing; } }
    [SerializeField] WeaponRightHandOffseter _handOffseter; public WeaponRightHandOffseter HandOffseter { get { return _handOffseter; } }
    [Space(10)]
    [SerializeField] IkHandsTargetsStruct _ikHandsTargets; public IkHandsTargetsStruct IkHandsTargets { get { return _ikHandsTargets; } }




    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] PosRotStruct _mainVectors;
    private PosRotStruct _bobSwayVectors;
    private PosRotStruct _bobSwayVectorsTarget;




    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _bobSwayVectorsSmoothSpeed;





    [System.Serializable]
    public struct PosRotStruct
    { 
        public Vector3 Pos;
        public Vector3 Rot;
    }
    [System.Serializable]
    public struct IkHandsTargetsStruct
    {
        public Transform Right;
        public Transform Left;
    }






    private void Update()
    {
        CombineBobAndSway();
        SmoothOutBobAndSway();

        CombineVectorsToMain();


        ApplyMainVectorsToHand();
    }





    private void CombineBobAndSway()
    {
        _bobSwayVectorsTarget.Pos = _bobbing.MainBobVectors.Pos;
        _bobSwayVectorsTarget.Rot = _bobbing.MainBobVectors.Rot + _sway.SwayRot;
    }
    private void SmoothOutBobAndSway()
    {
        _bobSwayVectors.Pos = Vector3.Lerp(_bobSwayVectors.Pos, _bobSwayVectorsTarget.Pos, _bobSwayVectorsSmoothSpeed * Time.deltaTime);
        _bobSwayVectors.Rot = Vector3.Lerp(_bobSwayVectors.Rot, _bobSwayVectorsTarget.Rot, _bobSwayVectorsSmoothSpeed * Time.deltaTime);
    }

    private void CombineVectorsToMain()
    {
        _mainVectors.Pos = _bobSwayVectors.Pos + _handOffseter.HandOffsets.Pos;
        _mainVectors.Rot = _bobSwayVectors.Rot + _handOffseter.HandOffsets.Rot;
    }



    private void ApplyMainVectorsToHand()
    {
        _ikHandsTargets.Right.parent.localRotation = Quaternion.Euler(_mainVectors.Rot);
        _ikHandsTargets.Right.localPosition = _mainVectors.Pos;
    }
}
