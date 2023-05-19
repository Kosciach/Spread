using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSideBobbing : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponBobbingController _bobbingController;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _sideMovementRotTarget;
    [SerializeField] Vector3 _sideMovementRot; public Vector3 SideMovementRot { get { return _sideMovementRot; } }



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _sideBobbingStrength;
    [Range(0, 10)]
    [SerializeField] float _smoothSpeed;



    private void Update()
    {
        SetSideBobbing();
        SmoothSideBobbing();
    }




    private void SetSideBobbing()
    {
        int sideMovementX = (int)_bobbingController.WeaponAnimator.PlayerStateMachine.InputController.MovementInputVector.x;
        int sideMovementZ = (int)_bobbingController.WeaponAnimator.PlayerStateMachine.InputController.MovementInputVector.z;
        _sideMovementRotTarget = new Vector3(sideMovementZ * _sideBobbingStrength/3, 0, sideMovementX * -_sideBobbingStrength);
    }

    private void SmoothSideBobbing()
    {
        _sideMovementRot = Vector3.Lerp(_sideMovementRot, _sideMovementRotTarget, _smoothSpeed * (_sideBobbingStrength/2) * Time.deltaTime);
    }
}
