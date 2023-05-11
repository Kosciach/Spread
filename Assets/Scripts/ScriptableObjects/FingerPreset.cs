using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/FingerPreset",fileName = "FingerPreset")]
public class FingerPreset : ScriptableObject
{
    public Hand RightHand;
    public Hand LeftHand;




    [System.Serializable]
    public struct Hand
    {
        public Finger Thumb;
        public Finger Index;
        public Finger Middle;
        public Finger Ring;
        public Finger Pinky;
    }

    [System.Serializable]
    public struct Finger
    {
        public Vector3 Target_Position;
        public Vector3 Target_Rotation;
        public Vector3 Hint_Position;
    }
}
