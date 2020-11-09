using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseProjectile : Actor
{
    
    Rigidbody rb;
    public float lifeTime = 3;
    public float contactLifetime = 0;

    public float explosiveForce = 10;
    public float explosiveDistance = 10;
    public float projectileForce = 10;
    float timeAlive = 0;

    private void Start()
    {
        
        rb = gameObject.GetComponent<Rigidbody>();

        if (rb)
        {
            Debug.Log("Has RB");
            rb.AddForce(transform.forward * projectileForce);
        }
    }
    private void Update()
    {

        
        //timeAlive += Time.deltaTime;
        //if (timeAlive >= lifeTime)
        //{
        //    explode();
        //    lifeTime = 0;
        //}
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explosiveDistance);
    }
    private void OnCollisionEnter(Collision other)
    {
        
        hitSomething();
    }

    void hitSomething()
    {
        StartCoroutine(waitForExplode());
    }

    void explode()
    {
        Vector3 origin = transform.position;

        Collider[] hits = Physics.OverlapSphere(origin,
                                                explosiveDistance);
        foreach (Collider hit in hits)
        {
            Vector3 dir = origin - hit.transform.position;
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

    IEnumerator waitForExplode()
    {
        yield return new WaitForSeconds(contactLifetime);
        explode();
    }
}
