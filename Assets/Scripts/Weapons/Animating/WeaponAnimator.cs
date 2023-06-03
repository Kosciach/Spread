using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [Space(5)]
    [SerializeField] WeaponMainPositioner _mainPositioner; public WeaponMainPositioner MainPositioner { get { return _mainPositioner; } }
    [SerializeField] WeaponSwayController _sway; public WeaponSwayController Sway { get { return _sway; } }
    [SerializeField] WeaponBobbingController _bobbing; public WeaponBobbingController Bobbing { get { return _bobbing; } }
    [SerializeField] WeaponRightHandOffseter _handOffseter; public WeaponRightHandOffseter HandOffseter { get { return _handOffseter; } }
    [SerializeField] WeaponRecoilController _recoil; public WeaponRecoilController Recoil { get { return _recoil; } }
    [Space(10)]
    [SerializeField] Transform _rightHandIk; public Transform RightHandIk { get { return _rightHandIk; } }




    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] PosRotStruct _baseVectors;
    [SerializeField] PosRotStruct _extraVectors; public PosRotStruct ExtraVectors { get { return _extraVectors; } }
    private PosRotStruct _bobSwayVectors;
    private PosRotStruct _bobSwayVectorsTarget;




    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _bobSwayVectorsSmoothSpeed;





    [System.Serializable]
    public struct PosRotStruct
    { 
        public Vector3 Pos;
        public Vector3 Rot;
    }






    private void Update()
    {
        CombineVectorsForBaseTarget();
        CombineVectorsForExtraTarget();


        ApplyVectorsToBaseTarget();
        ApplyVectorsToExtraTarget();
    }






    private void CombineVectorsForBaseTarget()
    {
        _baseVectors.Pos = _mainPositioner.CurrentMainVectors.Pos + _bobbing.Base.CurrentVectors.Pos;
        _baseVectors.Rot = _mainPositioner.CurrentMainVectors.Rot + _bobbing.Base.CurrentVectors.Rot;
    }
    private void CombineVectorsForExtraTarget()
    {
        _extraVectors.Pos = _recoil.RecoilVectors.Pos + _sway.CurrentSwayVectors.Pos + _handOffseter.HandOffsets.Pos;
        _extraVectors.Rot = _recoil.RecoilVectors.Rot + _sway.CurrentSwayVectors.Rot + _handOffseter.HandOffsets.Rot + _bobbing.Side.SideMovementRot;
    }



    private void ApplyVectorsToBaseTarget()
    {
        _rightHandIk.parent.localPosition = _baseVectors.Pos;
        _rightHandIk.localRotation = Quaternion.Euler(_baseVectors.Rot);
    }
    private void ApplyVectorsToExtraTarget()
    {
        _rightHandIk.parent.localRotation = Quaternion.Euler(_extraVectors.Rot);
        _rightHandIk.localPosition = _extraVectors.Pos;
    }
}
