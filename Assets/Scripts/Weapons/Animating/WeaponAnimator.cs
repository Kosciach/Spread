using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [Space(5)]
    [SerializeField] WeaponMainPositionerAnimator _mainPositioner; public WeaponMainPositionerAnimator MainPositioner { get { return _mainPositioner; } }
    [SerializeField] WeaponSwayController _sway; public WeaponSwayController Sway { get { return _sway; } }
    [SerializeField] WeaponBobbingController _bobbing; public WeaponBobbingController Bobbing { get { return _bobbing; } }
    [SerializeField] WeaponRightHandOffseter _handOffseter; public WeaponRightHandOffseter HandOffseter { get { return _handOffseter; } }
    [SerializeField] WeaponRecoilController _recoil; public WeaponRecoilController Recoil { get { return _recoil; } }
    [Space(10)]
    [SerializeField] Transform _rightHandIk; public Transform RightHandIk { get { return _rightHandIk; } }




    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] PosRotStruct _mainVectors;
    [SerializeField] PosRotStruct _additionalVectors; public PosRotStruct AdditionalVectors { get { return _additionalVectors; } }
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






    private void Update()
    {
        CombineBobAndSway();
        SmoothOutBobAndSway();

        CombineAdditionalVectors();


        ApplyAdditionalVectorsToHand();
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

    private void CombineAdditionalVectors()
    {
        _additionalVectors.Pos = _bobSwayVectors.Pos + _handOffseter.HandOffsets.Pos + _recoil.RecoilVectors.Pos;
        _additionalVectors.Rot = _bobSwayVectors.Rot + _handOffseter.HandOffsets.Rot + _recoil.RecoilVectors.Rot;
    }



    private void ApplyMainVectorsToHand()
    {
        _mainVectors.Pos = _mainPositioner.CurrentMainVectors.Pos;
        _mainVectors.Rot = _mainPositioner.CurrentMainVectors.Rot;

        _rightHandIk.parent.localPosition = _mainVectors.Pos;
        _rightHandIk.localRotation = Quaternion.Euler(_mainVectors.Rot);
    }
    private void ApplyAdditionalVectorsToHand()
    {
        _rightHandIk.parent.localRotation = Quaternion.Euler(_additionalVectors.Rot);
        _rightHandIk.localPosition = _additionalVectors.Pos;
    }
}
