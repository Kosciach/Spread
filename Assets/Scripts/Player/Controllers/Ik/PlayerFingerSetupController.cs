using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFingerSetupController : MonoBehaviour
{
    //This script sets values in FingerPrest scriptableObject automatically, use with causion!
    //How to use:
    //1. Setup fingers with ik in playmode for weapon.
    //2. In inspector set correct finger preset for current weapon.
    //3. Make sure script is enabled and all values in inspector are correct.
    //4. Press    SetFingers    bool, it will automatically setup fingers for given FingerPrest and disable this bool.



    [Header("====References====")]
    [SerializeField] PlayerIkController _ikController;
    [SerializeField] FingerPreset _fingerPreset;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _setFingers;




    private void Update()
    {
        if (!_setFingers) return;

        SetUpFingers();
        _setFingers = false;
    }


    private void SetUpFingers()
    {
        if (_fingerPreset == null) return;
        Debug.Log(1);


        RThumb();
        RIndex();
        RMiddle();
        RRing();
        RPinky();


        LThumb();
        LIndex();
        LMiddle();
        LRing();
        LPinky();
    }



    private void RThumb()
    {
        _fingerPreset.RightHand.Thumb.Target_Position = _ikController.Fingers.RightHand.Thumb.localPosition;
        _fingerPreset.RightHand.Thumb.Target_Rotation = _ikController.Fingers.RightHand.Thumb.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.RightHand.Thumb.GetSiblingIndex();
        _fingerPreset.RightHand.Thumb.Hint_Position = _ikController.Fingers.RightHand.Thumb.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void RIndex()
    {
        _fingerPreset.RightHand.Index.Target_Position = _ikController.Fingers.RightHand.Index.localPosition;
        _fingerPreset.RightHand.Index.Target_Rotation = _ikController.Fingers.RightHand.Index.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.RightHand.Index.GetSiblingIndex();
        _fingerPreset.RightHand.Index.Hint_Position = _ikController.Fingers.RightHand.Index.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void RMiddle()
    {
        _fingerPreset.RightHand.Middle.Target_Position = _ikController.Fingers.RightHand.Middle.localPosition;
        _fingerPreset.RightHand.Middle.Target_Rotation = _ikController.Fingers.RightHand.Middle.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.RightHand.Middle.GetSiblingIndex();
        _fingerPreset.RightHand.Middle.Hint_Position = _ikController.Fingers.RightHand.Middle.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void RRing()
    {
        _fingerPreset.RightHand.Ring.Target_Position = _ikController.Fingers.RightHand.Ring.localPosition;
        _fingerPreset.RightHand.Ring.Target_Rotation = _ikController.Fingers.RightHand.Ring.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.RightHand.Ring.GetSiblingIndex();
        _fingerPreset.RightHand.Ring.Hint_Position = _ikController.Fingers.RightHand.Ring.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void RPinky()
    {
        _fingerPreset.RightHand.Pinky.Target_Position = _ikController.Fingers.RightHand.Pinky.localPosition;
        _fingerPreset.RightHand.Pinky.Target_Rotation = _ikController.Fingers.RightHand.Pinky.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.RightHand.Pinky.GetSiblingIndex();
        _fingerPreset.RightHand.Pinky.Hint_Position = _ikController.Fingers.RightHand.Pinky.parent.GetChild(fingerIndex + 1).localPosition;
    }



    private void LThumb()
    {
        _fingerPreset.LeftHand.Thumb.Target_Position = _ikController.Fingers.LeftHand.Thumb.localPosition;
        _fingerPreset.LeftHand.Thumb.Target_Rotation = _ikController.Fingers.LeftHand.Thumb.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.LeftHand.Thumb.GetSiblingIndex();
        _fingerPreset.LeftHand.Thumb.Hint_Position = _ikController.Fingers.LeftHand.Thumb.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void LIndex()
    {
        _fingerPreset.LeftHand.Index.Target_Position = _ikController.Fingers.LeftHand.Index.localPosition;
        _fingerPreset.LeftHand.Index.Target_Rotation = _ikController.Fingers.LeftHand.Index.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.LeftHand.Index.GetSiblingIndex();
        _fingerPreset.LeftHand.Index.Hint_Position = _ikController.Fingers.LeftHand.Index.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void LMiddle()
    {
        _fingerPreset.LeftHand.Middle.Target_Position = _ikController.Fingers.LeftHand.Middle.localPosition;
        _fingerPreset.LeftHand.Middle.Target_Rotation = _ikController.Fingers.LeftHand.Middle.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.LeftHand.Middle.GetSiblingIndex();
        _fingerPreset.RightHand.Middle.Hint_Position = _ikController.Fingers.RightHand.Middle.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void LRing()
    {
        _fingerPreset.LeftHand.Ring.Target_Position = _ikController.Fingers.LeftHand.Ring.localPosition;
        _fingerPreset.LeftHand.Ring.Target_Rotation = _ikController.Fingers.LeftHand.Ring.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.LeftHand.Ring.GetSiblingIndex();
        _fingerPreset.LeftHand.Ring.Hint_Position = _ikController.Fingers.LeftHand.Ring.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void LPinky()
    {
        _fingerPreset.LeftHand.Pinky.Target_Position = _ikController.Fingers.LeftHand.Pinky.localPosition;
        _fingerPreset.LeftHand.Pinky.Target_Rotation = _ikController.Fingers.LeftHand.Pinky.localRotation.eulerAngles;

        int fingerIndex = _ikController.Fingers.LeftHand.Pinky.GetSiblingIndex();
        _fingerPreset.LeftHand.Pinky.Hint_Position = _ikController.Fingers.LeftHand.Pinky.parent.GetChild(fingerIndex + 1).localPosition;
    }










    private void OnDisable()
    {
        _setFingers = false;
    }
}
