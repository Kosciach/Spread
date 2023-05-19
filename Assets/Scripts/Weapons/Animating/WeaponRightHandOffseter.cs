using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WeaponRightHandOffseter : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator; public WeaponAnimator WeaponAnimator { get { return _weaponAnimator; } }



    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] OffsetVectors _handOffsets; public OffsetVectors HandOffsets { get { return _handOffsets; } }
    [Space(5)]
    [SerializeField] OffsetVectors _handOffsetsTargets;
    [Space(10)]
    [Range(0, 1)]
    [SerializeField] int _offsetToggle;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _posOffsetSmoothSpeed;
    [Range(0, 5)]
    [SerializeField] float _rotOffsetSmoothSpeed;





    [System.Serializable]
    public struct OffsetVectors
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }




    private void Update()
    {
        UpdateOffsets();
    }


    private void UpdateOffsets()
    {
        _handOffsets.Pos = Vector3.Lerp(_handOffsets.Pos, _handOffsetsTargets.Pos, _posOffsetSmoothSpeed * Time.deltaTime) * _offsetToggle;
        _handOffsets.Rot = Vector3.Lerp(_handOffsets.Rot, _handOffsetsTargets.Rot, _rotOffsetSmoothSpeed * Time.deltaTime) * _offsetToggle;
    }



    public void SetPosOffset(Vector3 pos, float speed)
    {
        _handOffsetsTargets.Pos = pos;
        _posOffsetSmoothSpeed = speed;
    }
    public void SetRotOffset(Vector3 rot, float speed)
    {
        _handOffsetsTargets.Rot = rot;
        _rotOffsetSmoothSpeed = speed;
    }


    public void ResetPosOffset()
    {
        _handOffsetsTargets.Pos = Vector3.zero;
    }
    public void ResetRotOffset()
    {
        _handOffsetsTargets.Rot = Vector3.zero;
    }



    public void ToggleOffset(bool enemy)
    {
        _offsetToggle = enemy ? 1 : 0;
    }
}
