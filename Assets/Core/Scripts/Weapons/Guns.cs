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
    protected override void Start()
    {
        base.Start();

        if (Input.GetKeyDown(reloadBinding))
        {
            reload();
        }
    }
    public override void Fire()
    {
        if(isReloading || clipEmpty() || isCooling) { return; }
           base.Fire();

        //pushes the player back 
        KnockBack(-transform.parent.forward, knockBackForce);

        //pauses for firerate cooldown
        waitForFireRate();
    }
    public virtual bool clipEmpty()
    {
        if (currentClip <= 0)
        {
            UIManager UiMan = UI.GetComponent<UIManager>();
            if (UiMan)
            {
                UiMan.sendMessage("Press \'" + reloadBinding + "\' to reload your weapon.");
            }
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
}
