using UnityEngine;

namespace Spread.Player.Input
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerInputs _inputs;
        internal PlayerInputs Inputs => _inputs;

        internal void Setup()
        {
            _inputs = new PlayerInputs();
            ToggleInput(true);
        }

        internal void ToggleInput(bool p_enable)
        {
            if (_inputs == null) return;

            if (p_enable) _inputs.Enable();
            else _inputs.Disable();
        }

        internal void ToggleCameraInput(bool p_enable)
        {
            if(p_enable) _inputs.Mouse.Look.Enable();
            else _inputs.Mouse.Look.Disable();
        }


        private void OnEnable() => ToggleInput(true);
        private void OnDisable() => ToggleInput(false);
    }
}