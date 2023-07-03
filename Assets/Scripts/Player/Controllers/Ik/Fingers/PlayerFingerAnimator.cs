using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFingerAnimator : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerFingerAnimator_Discipline _discipline; public PlayerFingerAnimator_Discipline Discipline { get { return _discipline; } }
    
    [Space(5)]

    [SerializeField] PlayerStateMachine _playerStateMachine;

    [Space(5)]
    [SerializeField] Hand _rightHand; public Hand RightHand { get { return _rightHand; } }
    [SerializeField] Hand _leftHand; public Hand LeftHand { get { return _leftHand; } }
    [SerializeField] Transform _disciplineTarget; public Transform DisciplineTarget { get { return _disciplineTarget; } }


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
