using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : Weapon
{
    private bool isReloading = false;
    

    [Range(-100f, 100f)]
    public float knockBackForce;
    [Range(0.01f, 50f)]
    public float fireRate;

    
    KeyCode reloadBinding = KeyCode.R;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(reloadBinding) || isReloading)
        {
            reload();
        }
    }
    public override bool Fire()
    {
        if (skipFire()) return false;
        
        bool fireSuccess = base.Fire();

        Debug.Log("FIREING FROM GUNS");

        //pauses for firerate cooldown
        waitForFireRate();

        return fireSuccess;
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


    float reloadingTime = 0;
    float percentGoingDown = 0.1f;
    float percentGoingUp = 0.85f;
    Vector3 reloadAngle = new Vector3(25, -50, 0);
    public virtual void reload()
    {
        //dropDown logic
        float progress = reloadingTime / reloadSpeed;
        float goingDownProgress = reloadingTime / reloadSpeed * (1/percentGoingDown);
        float goingUpProgress = (progress-percentGoingUp)/(1-percentGoingUp);
        if (progress <= percentGoingDown)//portion where the gun should slerp down
        {
            //Debug.Log($"Progress: {progress},  Progress going up: {goingDownProgress}");
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero,reloadAngle,goingDownProgress));
        }
        else if (progress >= percentGoingUp)//portion where the gun should slerp back up
        {
            Debug.Log($"Progress: {progress},  Progress going up: {goingUpProgress}");
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(reloadAngle, Vector3.zero, goingUpProgress));
        }

        //reloading logic
        if (!isReloading)
        {
            isReloading = true;
        }
        else
        {
            reloadingTime += Time.deltaTime;

            if (reloadingTime > reloadSpeed)
            {
                reloadingTime = 0;
                isReloading = false;
                currentClip = clipSize;
                resetRotation();
                setAmmo();
            }
        }
        
    }



    IEnumerator reloadTimer(float input)
    {
        isReloading = true;
        yield return new WaitForSeconds(input);
        isReloading = false;
        currentClip = clipSize;
        
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
            reload();
            return true;
        }
        //if (isCooling)
        //{
        //    UIMan.sendMessage("Gun is cooling...", new Vector3(0, -100, 0));
        //    return true;
        //}

        if (isReloading || clipEmpty() || isCooling) { return true; }

        return false;
    }
}
