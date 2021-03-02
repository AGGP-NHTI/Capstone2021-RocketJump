using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : Weapon
{
    private bool isReloading = false;
    

    [Range(0f, 100f)]
    public float knockBackForce;
    [Range(1f, 50f)]
    public float fireRate;

    
    KeyCode reloadBinding = KeyCode.R;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(reloadBinding))
        {
            reload();
        }
    }
    public override void Fire()
    {
        if (skipFire()) return;
        
        base.Fire();

        //pushes the player back 
        KnockBack(-transform.parent.forward, knockBackForce);

        //pauses for firerate cooldown
        waitForFireRate();
    }
    public virtual bool clipEmpty()
    {

        //Debug.Log("CURRENT CLIP: " + currentClip);

        if (currentClip <= 0)
        {
            return true;
        }
        return false;
    }
    public virtual void waitForFireRate()
    {
        StartCoroutine(waitFireRateTimer(fireRate));
    }
    public virtual void reload()
    {
        StartCoroutine(reloadTimer(reloadSpeed));
    }

    IEnumerator reloadTimer(float input)
    {
        isReloading = true;
        yield return new WaitForSeconds(input);
        isReloading = false;
        currentClip = clipSize;
        AmmoReference.SetMagazine(currentClip);
    }

    bool skipFire()
    {
        if (isReloading) 
        {
            UIMan.sendMessage("Reloading...", new Vector3(0, -100, 0));
            return true;
        }
        if (clipEmpty()) 
        {
            UIMan.sendMessage("Press \'" + reloadBinding + "\' to reload your weapon.", new Vector3(0, -100, 0));
            return true;
        }
        if (isCooling)
        {
            UIMan.sendMessage("Gun is cooling...",new Vector3(0,-100,0));
            return true;
        }

        if (isReloading || clipEmpty() || isCooling) { return true; }

        return false;
    }
}
