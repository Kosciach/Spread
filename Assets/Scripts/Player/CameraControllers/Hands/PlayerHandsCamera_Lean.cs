using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerHandsCamera
{
    public class PlayerHandsCamera_Lean : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] PlayerHandsCameraController _handCameraController;

        private Vector3 _rotation; public Vector3 Rotation { get { return _rotation; } }


        public void SetRotation(Vector3 rotation)
        {
            _rotation = rotation;
        }
    }
}