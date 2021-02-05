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
        if (Input.GetKeyDown(reloadBinding))
        {
            reload();
        }
    }

    public override void Fire() 
    {
        if (isReloading || clipEmpty() || isCooling) return;

        GameObject go = Instantiate(projectilePrefab, projectileSpawn.position,BulletSpread(projectileSpawn.rotation));
        
        go.GetComponent<MLAPI.NetworkedObject>().Spawn();
        base.Fire();

    }
    public override void AltFire() 
    { 
    
    }
}
