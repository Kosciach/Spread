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
    [Space(10)]
    [SerializeField] IkHandsTargetsStruct _ikHandsTargets; public IkHandsTargetsStruct IkHandsTargets { get { return _ikHandsTargets; } }


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] MainVectorsBobSway _mainVectorsBobSway;
    private MainVectorsBobSway _mainVectorsBobSwayTarget;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _bobSwaySmoothSpeed;





    [System.Serializable]
    public struct MainVectorsBobSway
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
        ApplyBobAndSway();
    }





    private void CombineBobAndSway()
    {
        _mainVectorsBobSwayTarget.Pos = _bobbing.MainBobVectors.Pos;
        _mainVectorsBobSwayTarget.Rot = _bobbing.MainBobVectors.Rot + _sway.SwayRot;
    }
    private void SmoothOutBobAndSway()
    {
        _mainVectorsBobSway.Pos = Vector3.Lerp(_mainVectorsBobSway.Pos, _mainVectorsBobSwayTarget.Pos, _bobSwaySmoothSpeed * Time.deltaTime);
        _mainVectorsBobSway.Rot = Vector3.Lerp(_mainVectorsBobSway.Rot, _mainVectorsBobSwayTarget.Rot, _bobSwaySmoothSpeed * Time.deltaTime);
    }
    private void ApplyBobAndSway()
    {
        _ikHandsTargets.Right.parent.localRotation = Quaternion.Euler(_mainVectorsBobSway.Rot);
        _ikHandsTargets.Right.localPosition = _mainVectorsBobSway.Pos;
    }
}
