using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncher : Weapon
{
    

    private void Start()
    {

        reloadSpeed = 2f;
        clipSize = 2;
        
        currentClip = clipSize;

        ammo.text = currentClip.ToString();
        maxAmmo.text = ammo.text;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            reload();
        }
    }

    public override void Fire() 
    {
        if (isReloading || clipEmpty() || isCooling) return;

        Debug.Log("FIRE");
        Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);

        currentClip--;
        ammo.text = currentClip.ToString();
        Debug.Log(currentClip);

        waitForFireRate();

    }
    public override void AltFire() 
    { 
    
    }
}
