using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShellEjector : MonoBehaviour
{
    [Header("====Refernces====")]
    [SerializeField] Transform _shellPrefab;






    public void EjectShell()
    {
        Instantiate(_shellPrefab, transform.position, transform.rotation);
    }
}
