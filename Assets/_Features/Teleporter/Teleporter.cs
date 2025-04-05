using UnityEngine;

namespace Spread.Teleporter
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private Transform _destination;

        private void OnTriggerEnter(Collider other)
        {
            CharacterController characterController = other.gameObject.GetComponent<CharacterController>();

            if (characterController != null)
                characterController.enabled = false;

            other.transform.position = _destination.position;

            if (characterController != null)
                characterController.enabled = true;
        }
    }
}
