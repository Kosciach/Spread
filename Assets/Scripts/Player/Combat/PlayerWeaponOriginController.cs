using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponOriginController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;


    [Header("====Settings====")]
    [SerializeField] Vector3 _basePos;
    [Space(5)]
    [Range(0.1f, 50)]
    [SerializeField] float _posOffsetWeakness;


    private void Update()
    {
        KeepWorldRotationZero();
        UpdateOriginPosition();
    }



    private void KeepWorldRotationZero()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    private void UpdateOriginPosition()
    {
        Vector3 inputVector = _playerStateMachine.InputController.MovementInputVectorNormalized;
        Vector3 posOffset = (transform.forward * inputVector.z + transform.right * inputVector.x) * _playerStateMachine.MovementController.OnGround.Speed;


        transform.localPosition = _basePos + posOffset / _posOffsetWeakness;
    }



    public void SetRotation(Vector3 rotation)
    {
        transform.GetChild(0).localRotation = Quaternion.Euler(rotation);
    }
}
