using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponAnimator_BakeTransformer : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] WeaponAnimator _weaponAnimator;
    [Space(5)]
    [SerializeField] Transform[] _baseIkTransforms;
    [SerializeField] Transform[] _bakedIkTransforms;
    [Space(5)]
    [SerializeField] Rig _bakedWeaponAnimatingLayer;

    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _updateBakedTransforms;
   





    public void UpdateBakedTransforms()
    {
        for(int i=0; i<4; i++)
        {
            _bakedIkTransforms[i].position = _baseIkTransforms[i].position;
            _bakedIkTransforms[i].rotation = _baseIkTransforms[i].rotation;
        }
    }
}
