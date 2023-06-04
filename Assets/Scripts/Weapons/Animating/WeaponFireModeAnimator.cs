using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFireModeAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] WeaponAnimator.PosRotStruct _vectors; public WeaponAnimator.PosRotStruct Vectors { get { return _vectors; } }



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] float _resetSpeed;



    private void Update()
    {
        ResetVectors();

    }




    private void ResetVectors()
    {
        _vectors.Pos = Vector3.Lerp(_vectors.Pos, Vector3.zero, _resetSpeed * Time.deltaTime);
        _vectors.Rot = Vector3.Lerp(_vectors.Rot, Vector3.zero, _resetSpeed * Time.deltaTime);
    }


    public void ChangeFireModeAnim()
    {
        //Rotation
        LeanTween.value(_vectors.Rot.z, 4, 0.1f).setEaseOutBack().setOnUpdate((float val) =>
        {
            _vectors.Rot.z = val;
        });


        //Position
        LeanTween.value(_vectors.Pos.x, 0.01f, 0.1f).setOnUpdate((float val) =>
        {
            _vectors.Pos.x = val;
        });
        LeanTween.value(_vectors.Pos.z, 0.01f, 0.1f).setOnUpdate((float val) =>
        {
            _vectors.Pos.z = val;
        });
    }

}
