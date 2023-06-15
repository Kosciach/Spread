using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFingerAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] PlayerTriggerDisciplineController _triggerDiscipline; public PlayerTriggerDisciplineController TriggerDiscipline { get { return _triggerDiscipline; } }
    [SerializeField] Hand _rightHand; public Hand RightHand { get { return _rightHand; } }
    [SerializeField] Hand _leftHand; public Hand LeftHand { get { return _leftHand; } }


    [System.Serializable]
    public struct Hand
    {
        public Transform Thumb;
        public Transform Index;
        public Transform Middle;
        public Transform Ring;
        public Transform Pinky;
    }








    public void SetUpAllFingers(FingerPreset.PresetStruct fingerPreset, float lerpTime)
    {
        SetUpHandFingers(_rightHand, fingerPreset.RightHand, lerpTime);
        SetUpHandFingers(_leftHand, fingerPreset.LeftHand, lerpTime);
    }
    private void SetUpHandFingers(Hand hand, FingerPreset.Hand fingerPresetHand, float lerpTime)
    {
        SetUpOneFinger(hand.Thumb, fingerPresetHand.Thumb, lerpTime);
        SetUpOneFinger(hand.Index, fingerPresetHand.Index, lerpTime);
        SetUpOneFinger(hand.Middle, fingerPresetHand.Middle, lerpTime);
        SetUpOneFinger(hand.Ring, fingerPresetHand.Ring, lerpTime);
        SetUpOneFinger(hand.Pinky, fingerPresetHand.Pinky, lerpTime);
    }
    private void SetUpOneFinger(Transform finger, FingerPreset.Finger fingerPresets, float lerpTime)
    {
        LeanTween.moveLocal(finger.gameObject, fingerPresets.Target_Position, lerpTime);
        LeanTween.rotateLocal(finger.gameObject, fingerPresets.Target_Rotation, lerpTime);

        int siblingIndex = finger.GetSiblingIndex();

        Transform currentHint = finger.parent.GetChild(siblingIndex + 1);
        LeanTween.moveLocal(currentHint.gameObject, fingerPresets.Hint_Position, lerpTime);
    }
}
