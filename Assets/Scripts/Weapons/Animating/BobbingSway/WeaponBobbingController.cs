using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;

public class WeaponBobbingController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator; public WeaponAnimator WeaponAnimator { get { return _weaponAnimator; } }
    [SerializeField] WeaponBaseBobbing _base;
    [SerializeField] WeaponSideBobbing _side;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] MainBobVectorsStruct _mainBobVectors; public MainBobVectorsStruct MainBobVectors { get { return _mainBobVectors; } }
    [Range(0, 1)]
    [SerializeField] int _bobbingToggle; public int BobbingToggle { get { return _bobbingToggle; } }



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _playerVelocitySmoothSpeed;



    private float _playerVelocity => _weaponAnimator.PlayerStateMachine.MovementControllers.Movement.OnGround.CurrentMovementVector.magnitude;
    public float PlayerVelocity { get { return _playerVelocity; } }


    [System.Serializable]
    public struct MainBobVectorsStruct
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }





    private void Update()
    {
        CombineBobbingVectors();
    }


    private void CombineBobbingVectors()
    {
        _mainBobVectors.Rot = (_side.SideMovementRot + _base.CurrentVectors.Rot) * _bobbingToggle;
        _mainBobVectors.Pos = (_base.CurrentVectors.Pos) * _bobbingToggle;
    }


    public void Toggle(bool enable)
    {
        _bobbingToggle = enable ? 1 : 0;
    }
}
