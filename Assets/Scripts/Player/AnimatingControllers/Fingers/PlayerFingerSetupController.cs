using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] PlayerFingerAnimator _fingerAnimator;
    [SerializeField] FingerPreset _fingerPreset;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _setFingers;
    [SerializeField] SetupType _setupType;



    private enum SetupType
    {
        None, Base, Block, Discipline
    }



    private void Update()
    {
        if (!_setFingers) return;

        SetUpFingers();
        _setFingers = false;
        _setupType = SetupType.None;
    }


    private void SetUpFingers()
    {
        if (_fingerPreset == null) return;
        Debug.Log(1);


        switch(_setupType)
        {
            case SetupType.None: break;


            case SetupType.Base:

                SetUpHand(ref _fingerPreset.Base.RightHand, _fingerAnimator.RightHand);
                SetUpHand(ref _fingerPreset.Base.LeftHand, _fingerAnimator.LeftHand);

            break;
            case SetupType.Block:

                SetUpHand(ref _fingerPreset.Block.RightHand, _fingerAnimator.RightHand);
                SetUpHand(ref _fingerPreset.Block.LeftHand, _fingerAnimator.LeftHand);

            break;
            case SetupType.Discipline:

                SetUpTriggerDiscipline(ref _fingerPreset.TriggerDisciplineIndexFinger);

            break;
        }

        EditorUtility.SetDirty(_fingerPreset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }




    private void SetUpFinger(ref FingerPreset.Finger scriptableFinger, Transform finger)
    {
        scriptableFinger.Target_Position = finger.localPosition;
        scriptableFinger.Target_Rotation = finger.localRotation.eulerAngles;

        int fingerIndex = finger.GetSiblingIndex();
        scriptableFinger.Hint_Position = finger.parent.GetChild(fingerIndex + 1).localPosition;
    }
    private void SetUpHand(ref FingerPreset.Hand scriptableHand, PlayerFingerAnimator.Hand hand)
    {
        SetUpFinger(ref scriptableHand.Thumb, hand.Thumb);
        SetUpFinger(ref scriptableHand.Index, hand.Index);
        SetUpFinger(ref scriptableHand.Middle, hand.Middle);
        SetUpFinger(ref scriptableHand.Ring, hand.Ring);
        SetUpFinger(ref scriptableHand.Pinky, hand.Pinky);
    }

    private void SetUpTriggerDiscipline(ref FingerPreset.Finger scriptableFinger)
    {
        scriptableFinger.Target_Position = _fingerAnimator.DisciplineTarget.localPosition;
        scriptableFinger.Target_Rotation = _fingerAnimator.DisciplineTarget.localRotation.eulerAngles;

        int fingerIndex = _fingerAnimator.DisciplineTarget.GetSiblingIndex();
        scriptableFinger.Hint_Position = _fingerAnimator.DisciplineTarget.parent.GetChild(fingerIndex + 1).localPosition;
    }



    private void OnDisable()
    {
        _setFingers = false;
    }
}
