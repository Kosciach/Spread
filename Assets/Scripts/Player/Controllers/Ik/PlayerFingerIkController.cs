using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFingerIkController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerIkController _ikController;
    [SerializeField] Hand _rightHand;
    [SerializeField] Hand _leftHand;


    [System.Serializable]
    public struct Hand
    {
        public Transform Thumb;
        public Transform Index;
        public Transform Middle;
        public Transform Ring;
        public Transform Pinky;
    }








    public void SetUpAllFingers(FingerPreset fingerPreset)
    {
        SetUpHandFingers(_rightHand, fingerPreset.RightHand);
        SetUpHandFingers(_leftHand, fingerPreset.LeftHand);
    }
    private void SetUpHandFingers(Hand hand, FingerPreset.Hand fingerPresetHand)
    {
        SetUpOneFinger(hand.Thumb, fingerPresetHand.Thumb);
        SetUpOneFinger(hand.Index, fingerPresetHand.Index);
        SetUpOneFinger(hand.Middle, fingerPresetHand.Middle);
        SetUpOneFinger(hand.Ring, fingerPresetHand.Ring);
        SetUpOneFinger(hand.Pinky, fingerPresetHand.Pinky);
    }
    private void SetUpOneFinger(Transform finger, FingerPreset.Finger fingerPresets)
    {
        finger.localPosition = fingerPresets.Target_Position;
        finger.localRotation = Quaternion.Euler(fingerPresets.Target_Rotation);

        int siblingIndex = finger.GetSiblingIndex();
        finger.parent.GetChild(siblingIndex + 1).localPosition = fingerPresets.Hint_Position;
    }
}
