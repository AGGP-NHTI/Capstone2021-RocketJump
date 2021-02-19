using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Projectile
{
    bool hasTriggered = false;
    Vector3 lerpDestination;




    [Range(0.1f, 100f)]
    public float moveSpeed;
    [Range(0.1f,5f)]
    public float distanceBuffer;

    public override void Update()
    {
        base.Update();

        
        if (hasTriggered)
        {
            //player.transform.position = Vector3.Lerp(player.transform.position, lerpDestination, moveSpeed * Time.deltaTime);
            //player.moveTo(lerpDestination);

            if (Vector3.Distance(player.transform.position, lerpDestination) < distanceBuffer)
            {
                Destroy(gameObject);
            }
        }
        
    }
    
    protected override void trigger()
    {
        if (player)
        {
            lerpDestination = transform.position;
            hasTriggered = true;
        }
    }

 
}
