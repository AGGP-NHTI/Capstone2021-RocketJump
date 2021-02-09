using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile
{
    public GameObject particles;


    [Range(0f, 750f)]
    public float explosiveForce = 10;

    [Range(0f, 20f)]
    public float explosiveDistance = 10;

    [Range(0f,10f)]
    public float contactLifetime = 0;

    float timeAlive = 0;

    public override void Start()
    {
        base.Start();

        rb = gameObject.GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();

        if (rb)
        {
            rb.AddForce(transform.forward * projectileLaunchForce*100);
        }
    }
    public override void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            explode();
            lifeTime = 0;
        }
    }
    protected override void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosiveDistance);
    }

    protected void OnCollisionEnter(Collision other)
    {
        Debug.Log("COLLIDED WITH: "+ other.transform.name);
        hitSomething();
    }

    protected override void hitSomething()
    {
        StartCoroutine(waitForExplode());
    }

    protected virtual void explode()
    {
        GameObject part = Instantiate(particles, transform.position, Quaternion.identity);
        //Destroy(part, 3f);
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

    protected virtual IEnumerator waitForExplode()
    {
        yield return new WaitForSeconds(contactLifetime);
        explode();
    }



}
