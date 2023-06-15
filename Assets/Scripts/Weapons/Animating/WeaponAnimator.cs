using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [Space(5)]
    [SerializeField] WeaponBakeTargetsController _bakeTargets; public WeaponBakeTargetsController BakeTargets { get { return _bakeTargets; } }
    [SerializeField] WeaponMainPositioner _mainPositioner; public WeaponMainPositioner MainPositioner { get { return _mainPositioner; } }
    [SerializeField] WeaponSwayController _sway; public WeaponSwayController Sway { get { return _sway; } }
    [SerializeField] WeaponBobbingController _bobbing; public WeaponBobbingController Bobbing { get { return _bobbing; } }
    [SerializeField] WeaponRecoilAnimator _recoil; public WeaponRecoilAnimator Recoil { get { return _recoil; } }
    [SerializeField] WeaponCrouchAnimator _crouch; public WeaponCrouchAnimator Crouch { get { return _crouch; } }
    [SerializeField] WeaponInAirAnimator _inAir; public WeaponInAirAnimator InAir { get { return _inAir; } }
    [SerializeField] WeaponFireModeAnimator _fireMode; public WeaponFireModeAnimator FireMode { get { return _fireMode; } }
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
        _baseVectors.Pos = _mainPositioner.Pos;
        _baseVectors.Rot = _mainPositioner.Rot.eulerAngles;
    }
    private void CombineVectorsForExtraTarget()
    {
        _extraVectors.Pos = _bobbing.Base.CurrentVectors.Pos                 +                 _sway.CurrentSwayVectors.Pos + _recoil.RecoilVectors.Pos + _crouch.CurrentVectors.Pos + _inAir.CurrentVectors.Pos + _fireMode.Vectors.Pos;
        _extraVectors.Rot = _bobbing.Base.CurrentVectors.Rot + _bobbing.Side.SideMovementRot + _sway.CurrentSwayVectors.Rot + _recoil.RecoilVectors.Rot + _crouch.CurrentVectors.Rot + _inAir.CurrentVectors.Rot + _fireMode.Vectors.Rot;
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
