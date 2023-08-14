using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerHandsCamera
{
    public class PlayerHandsCamera_Enable : MonoBehaviour
    {
        [Header("====References====")]
        [SerializeField] Camera _mainCamera;
        [SerializeField] Camera _handsCamera;
        [SerializeField] PlayerStateMachine _stateMachine;


        [Space(20)]
        [Header("====Debugs====")]
        [SerializeField] bool _enableHandsCamera;


        [Space(20)]
        [Header("====Settings====")]
        [SerializeField] Transform[] _handsCameraParents;
        [SerializeField] LayerMask _everythingMask;
        [SerializeField] LayerMask _noHandsMask;


        private delegate void HandsCameraModeMethod();
        private HandsCameraModeMethod[] _handsCameraModeMethod = new HandsCameraModeMethod[2];




        private void Awake()
        {
            _handsCameraModeMethod[0] = EnableHandsCamera;//True
            _handsCameraModeMethod[1] = DisableHandsCamera;//False
            ToggleHandsCamera(false);
        }




        public void ToggleHandsCamera(bool enable)
        {
            if (!_stateMachine.CombatControllers.Combat.IsState(PlayerCombatController.CombatStateEnum.Unarmed)) return;

            int index = enable ? 0 : 1;

            _handsCamera.enabled = enable;
            _enableHandsCamera = enable;
            _handsCameraModeMethod[index]();
        }

        private void EnableHandsCamera()
        {
            _mainCamera.cullingMask = _noHandsMask;
        }
        private void DisableHandsCamera()
        {
            _mainCamera.cullingMask = _everythingMask;
            _handsCamera.transform.localPosition = Vector3.zero;

        }
    }
}