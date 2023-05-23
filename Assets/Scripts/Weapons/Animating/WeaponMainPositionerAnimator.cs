using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMainPositionerAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] MainVectors _currentMainVectors; public MainVectors CurrentMainVectors { get { return _currentMainVectors; } }
    [Space(5)]
    [SerializeField] MainVectors _desiredMainVectors;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _posVectorSmoothSpeed;
    [Range(0, 5)]
    [SerializeField] float _rotVectorSmoothSpeed;





    [System.Serializable]
    public struct MainVectors
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }




    private void Update()
    {
        UpdateTransformVectors();
    }


    private void UpdateTransformVectors()
    {
        _currentMainVectors.Pos = Vector3.Lerp(_currentMainVectors.Pos, _desiredMainVectors.Pos, _posVectorSmoothSpeed * Time.deltaTime);
        _currentMainVectors.Rot = Vector3.Lerp(_currentMainVectors.Rot, _desiredMainVectors.Rot, _rotVectorSmoothSpeed * Time.deltaTime);
    }



    public void SetPos(Vector3 pos, float speed)
    {
        _desiredMainVectors.Pos = pos;
        _posVectorSmoothSpeed = speed;
    }
    public void SetRot(Vector3 rot, float speed)
    {
        _desiredMainVectors.Rot = rot;
        _rotVectorSmoothSpeed = speed;
    }


    public void ResetPosOffset()
    {
        _desiredMainVectors.Pos = Vector3.zero;
    }
    public void ResetRotOffset()
    {
        _desiredMainVectors.Rot = Vector3.zero;
    }
}
