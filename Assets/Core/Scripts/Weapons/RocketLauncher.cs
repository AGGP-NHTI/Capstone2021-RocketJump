using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncher : Weapon
{
    

    protected override void Start()
    {
        base.Start();
        reloadSpeed = 2f;
        clipSize = 2;
        
        currentClip = clipSize;

        AmmoReference.SetMagazine(currentClip);
        AmmoReference.SetReserve(clipSize);
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

        Instantiate(projectilePrefab, projectileSpawn.position,BulletSpread(projectileSpawn.rotation));
        base.Fire();

    }
    public override void AltFire() 
    { 
    
    }
}
