using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile
{
    public GameObject particles;


    [Range(0f, 5000f)]
    public float explosiveForce = 10;

    [Range(0f, 20f)]
    public float explosiveDistance = 10;

    
    protected override void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosiveDistance);
    }
    protected override void trigger()
    {
        GameObject part = Instantiate(particles, transform.position, Quaternion.identity);
        //Destroy(part, 3f);
        Vector3 origin = transform.position;

        Collider[] hits = Physics.OverlapSphere(origin,
                                                explosiveDistance);
        foreach (Collider hit in hits)
        {
            Rigidbody rbObj = hit.GetComponent<Rigidbody>();
            CharacterController cc = hit.GetComponent<CharacterController>();

            //For other projectiles
            if (rbObj)
            {

                Debug.Log("BAM on " + hit.gameObject.name);

                rbObj.AddExplosionForce(explosiveForce,
                                     origin,
                                     explosiveDistance
                                     );
            }
            //for players
            else if (cc)
            {

                Debug.Log("BAM on " + hit.gameObject.name);

                Vector3 dir = origin - hit.transform.position;

                
            }
        }

        Destroy(gameObject);
    }





}
