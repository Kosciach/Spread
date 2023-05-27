using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerDisciplineController : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] PlayerFingerAnimator _fingerAnimator;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] bool _enable;


    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 5)]
    [SerializeField] float _tweenTime;



    private delegate void TriggerDisciplineMethods(FingerPreset fingerPreset);
    private TriggerDisciplineMethods[] _triggerDisciplineMethods = new TriggerDisciplineMethods[2];



    private void Awake()
    {
        _triggerDisciplineMethods[0] = DisableTriggerDiscipline;
        _triggerDisciplineMethods[1] = EnableTriggerDiscipline;
    }

    public void SwitchTriggerDiscipline(WeaponData weaponData, bool enable)
    {
        if (weaponData.WeaponType == WeaponData.WeaponTypeEnum.Melee) return;

        int index = enable ? 1 : 0;
        _enable = enable;

        _triggerDisciplineMethods[index](weaponData.FingersPreset);
    }


    private void EnableTriggerDiscipline(FingerPreset fingerPreset)
    {
        LeanTween.moveLocal(_fingerAnimator.RightHand.Index.gameObject, fingerPreset.TriggerDisciplineIndexFinger.Target_Position, _tweenTime);
        LeanTween.rotateLocal(_fingerAnimator.RightHand.Index.gameObject, fingerPreset.TriggerDisciplineIndexFinger.Target_Rotation, _tweenTime);

        LeanTween.moveLocal(_fingerAnimator.RightHand.Index.parent.GetChild(3).gameObject, fingerPreset.TriggerDisciplineIndexFinger.Hint_Position, _tweenTime);
    }
    private void DisableTriggerDiscipline(FingerPreset fingerPreset)
    {
        LeanTween.moveLocal(_fingerAnimator.RightHand.Index.gameObject, fingerPreset.Base.RightHand.Index.Target_Position, _tweenTime);
        LeanTween.rotateLocal(_fingerAnimator.RightHand.Index.gameObject, fingerPreset.Base.RightHand.Index.Target_Rotation, _tweenTime);

        LeanTween.moveLocal(_fingerAnimator.RightHand.Index.parent.GetChild(3).gameObject, fingerPreset.Base.RightHand.Index.Hint_Position, _tweenTime);
    }
}
