using UnityEngine;

namespace Spread.Ladder
{
    public class LadderGeneratorHandle : MonoBehaviour
    {
        [SerializeField] private Transform _step;
        public Transform Step => _step;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_step.position, 0.06f);
            Gizmos.DrawLine(transform.position, _step.position);
        }
#endif
    }
}
