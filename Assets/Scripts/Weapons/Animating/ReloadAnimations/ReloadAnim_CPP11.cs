using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnim_CPP11 : ReloadAnim
{
    [Space(20)]
    [Header("====AnimationRightHand====")]
    [SerializeField] PosRotStruct _start;



    [System.Serializable]
    private struct PosRotStruct
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }



    protected override void PlayAnim(Transform rightHandIk, Transform leftHandIk)
    {
        Debug.Log("Cpp11R");
    }
}
