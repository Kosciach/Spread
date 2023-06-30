using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCineCamera_Bobbing : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerCineCameraController _cineCameraController;
    [SerializeField] CineCameraRotationOffset _rotationOffset;




    private void Update()
    {
        _rotationOffset.m_BobbingOffset = _cineCameraController.PlayerStateMachine.AnimatingControllers.Weapon.Bobbing.Base.CameraVectors.Rot;
    }
}
