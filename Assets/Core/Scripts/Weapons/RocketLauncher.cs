using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Fire();
        }
    }

    public override void Fire() 
    {
        Debug.Log("FIRE");
        Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
    }
    public override void AltFire() 
    { 
    
    }
}
