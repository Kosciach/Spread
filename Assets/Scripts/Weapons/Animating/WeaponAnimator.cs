using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator_BakeTransformer _bakeTransformer;       public WeaponAnimator_BakeTransformer BakeTransformer { get { return _bakeTransformer; } }
    [SerializeField] WeaponAnimator_MainTransformer _mainTransformer;       public WeaponAnimator_MainTransformer MainTransformer { get { return _mainTransformer; } }
    [SerializeField] WeaponAnimator_Sway _sway;                             public WeaponAnimator_Sway Sway { get { return _sway; } }
    [SerializeField] WeaponAnimator_Bobbing _bobbing;                       public WeaponAnimator_Bobbing Bobbing { get { return _bobbing; } }
    [SerializeField] WeaponAnimator_Recoil _recoil;                         public WeaponAnimator_Recoil Recoil { get { return _recoil; } }
    [SerializeField] WeaponAnimator_Crouch _crouch;                         public WeaponAnimator_Crouch Crouch { get { return _crouch; } }
    [SerializeField] WeaponAnimator_InAir _inAir;                           public WeaponAnimator_InAir InAir { get { return _inAir; } }
    [SerializeField] WeaponAnimator_FireMode _fireMode;                     public WeaponAnimator_FireMode FireMode { get { return _fireMode; } }
    [Space(5)]
    [SerializeField] PlayerStateMachine _playerStateMachine;                public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    [SerializeField] Transform _rightHandIk;                                public Transform RightHandIk { get { return _rightHandIk; } }




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
        _baseVectors.Pos = _mainTransformer.Pos;
        _baseVectors.Rot = _mainTransformer.Rot.eulerAngles;
    }
    private void CombineVectorsForExtraTarget()
    {
        _extraVectors.Pos = _bobbing.Base.SmoothVectors.Pos                 +                   _sway.SmoothVectors.Pos + _recoil.RecoilVectors.Pos + _crouch.CurrentVectors.Pos + _inAir.CurrentVectors.Pos + _fireMode.Vectors.Pos;
        _extraVectors.Rot = _bobbing.Base.SmoothVectors.Rot + _bobbing.Side.SmoothVectors.Rot + _sway.SmoothVectors.Rot + _recoil.RecoilVectors.Rot + _crouch.CurrentVectors.Rot + _inAir.CurrentVectors.Rot + _fireMode.Vectors.Rot;
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
