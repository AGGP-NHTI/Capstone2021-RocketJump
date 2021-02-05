using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile
{
    public GameObject particles;
    protected override void OnCollisionEnter(Collision other)
    {

        hitSomething();
    }

    protected override void hitSomething()
    {
        StartCoroutine(waitForExplode());
    }

    protected override void explode()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Vector3 origin = transform.position;

        Collider[] hits = Physics.OverlapSphere(origin,
                                                explosiveDistance);
        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb)
            {

                Debug.Log("BAM on " + hit.gameObject.name);

                rb.AddExplosionForce(explosiveForce,
                                     origin,
                                     explosiveDistance);

                Destroy(gameObject);
            }
        }
    }

    protected override IEnumerator waitForExplode()
    {
        yield return new WaitForSeconds(contactLifetime);
        explode();
    }



}
