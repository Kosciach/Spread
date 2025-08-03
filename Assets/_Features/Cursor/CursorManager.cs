using UnityEngine;

namespace Spread.Cursor
{
    public class CursorManager : MonoBehaviour
    {
        private void Awake()
        {
            ToggleCursor(false);
        }

        public void ToggleCursor(bool p_show)
        {
            UnityEngine.Cursor.visible = p_show;
            UnityEngine.Cursor.lockState = p_show ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
