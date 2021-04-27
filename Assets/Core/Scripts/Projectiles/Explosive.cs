using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile
{
    public GameObject particles;


    [Range(0f, 50f)]
    public float explosiveForce = 10;

    [Range(0f, 20f)]
    public float explosiveDistance = 10;


    public bool forwardExplosion = false;

    
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
            Player_Movement_Controller playerController = hit.GetComponent<Player_Movement_Controller>();
            
            

            //For other projectiles
            if (rbObj)
            {
                rbObj.AddExplosionForce(explosiveForce,
                                     origin,
                                     explosiveDistance
                                     );
            }
            //forplayers
            else if (playerController)
            {
                Vector3 dir = hit.transform.position - origin;

                float kindaAngle = Vector3.Dot(transform.forward, dir.normalized);

                if (kindaAngle > 0 || !forwardExplosion) 
                {
                    playerController.AddForce(dir.normalized * explosiveForce);
                }
            }
        }

        Destroy(gameObject);
    }





}
