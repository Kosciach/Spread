using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController_FragGrenade : BaseThrowableController
{
    [Header("====References====")]
    [SerializeField] GameObject _explosionParticle;



    public override void OnActivate()
    {
        StartCoroutine(Explode());
    }

    public override void OnSafe()
    {

    }





    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2);

        RaycastHit hit;
        Quaternion particleRotation = Quaternion.Euler(-90, 0, 0);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
            particleRotation = Quaternion.Euler(hit.normal);

        Instantiate(_explosionParticle, transform.position, particleRotation);


        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, 5);

        foreach (Collider detectedObject in detectedObjects)
        {
            Rigidbody detectedRigidbody = detectedObject.GetComponent<Rigidbody>();
            if (detectedRigidbody != null) detectedRigidbody.AddExplosionForce(1000, transform.position, 5);
        }

        Destroy(gameObject);
    }
}
