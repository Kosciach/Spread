using UnityEngine;

namespace Spread.Player.Ladder
{
    public class PlayerLadderThumbController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _hint;

        internal void UpdateThumb(Transform p_target, Transform p_hint)
        {
            _target.position = p_target.position;
            _hint.position = p_hint.position;
        }
    }
}
