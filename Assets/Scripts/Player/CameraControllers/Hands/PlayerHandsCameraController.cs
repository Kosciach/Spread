using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerHandsCamera
{
    public class PlayerHandsCameraController : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerHandsCamera_Move _move; public PlayerHandsCamera_Move Move { get { return _move; } }
        [SerializeField] PlayerHandsCamera_Rotate _rotate; public PlayerHandsCamera_Rotate Rotate { get { return _rotate; } }
        [SerializeField] PlayerHandsCamera_Lean _lean; public PlayerHandsCamera_Lean Lean { get { return _lean; } }
        [SerializeField] PlayerHandsCamera_Enable _enable; public PlayerHandsCamera_Enable Enable { get { return _enable; } }
        [Space(5)]
        [SerializeField] Camera _handsCamera; public Camera HandsCamera { get { return _handsCamera; } }
        [SerializeField] PlayerStateMachine _playerStateMachine; public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }





        private void Update()
        {
            CombineAndApplyRotationVectors();
        }

        private void CombineAndApplyRotationVectors()
        {
            Vector3 rotation = _rotate.LerpPreset.Rotation + _lean.Rotation;
            _handsCamera.transform.localRotation = Quaternion.Euler(rotation);
        }
    }
}
