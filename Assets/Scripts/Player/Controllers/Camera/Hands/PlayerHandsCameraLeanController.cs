using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsCameraLeanController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerHandsCameraController _handCameraController;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] Vector3 _rotation; public Vector3 Rotation { get { return _rotation; } }




    public void SetRotation(Vector3 rotation)
    {
        _rotation = rotation;
    }
}
