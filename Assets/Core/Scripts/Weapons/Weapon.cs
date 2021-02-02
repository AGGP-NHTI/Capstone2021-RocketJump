using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : Actor
{
    private GameObject UI;
    private PlayerController playerReference;

    [Header("Dependencies")] 
    //public GameObject weaponModel;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public Ammo_UI_Script AmmoReference;

    [Header("Stats")]
    public float    reloadSpeed;
    public float    fireRate;
    public float    bulletSpread;
    public float    knockBackForce;
    public int      currentClip;
    public int      clipSize;
    
    
    [Header("Info")]
    public bool isReloading = false; 
    public bool isCooling = false;




    public virtual void Fire() 
    {
        currentClip--;
        AmmoReference.SetMagazine(currentClip);
        KnockBack(transform.rotation.eulerAngles, knockBackForce);
        waitForFireRate();
    }
    public virtual void AltFire() 
    { 
        
    }

    protected virtual void Start()
    {
        Debug.Log("Start--");
        setPlayerReference();
        setUIObj();
        setAmmoReference();
    }

    public virtual bool clipEmpty() 
    {
        if (currentClip <= 0)
        {
            return true;
        }
        return false;
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

    public virtual void waitForFireRate()
    {
        StartCoroutine(waitFireRateTimer(fireRate));
    }

    IEnumerator waitFireRateTimer(float input)
    {
        float delay = 1/input ;
        isCooling = true;
        yield return new WaitForSeconds(delay);
        isCooling = false;
    }
    
    

    void setPlayerReference()
    {
        PlayerController player = transform.root.GetComponent<PlayerController>();
        Debug.Log("NAME: " + player.name);
        if (player)
        {
            playerReference = player;
            Debug.Log("PLAYER SET"); 
        }
        
    }
    void setUIObj()
    {
        UI = playerReference.UI;
        
    }

    void setAmmoReference()
    {
        Ammo_UI_Script ammo = UI.transform.GetComponentInChildren<Ammo_UI_Script>();
        if(ammo)
        {
            AmmoReference = ammo;
            Debug.Log("AMMO REFERENCE SET");    
        }
    }

    protected Quaternion BulletSpread(Quaternion input)
    {
        input.x += Random.Range(-bulletSpread, bulletSpread);
        input.y += Random.Range(-bulletSpread, bulletSpread);

        return input;
    }
    void KnockBack(Vector3 direction, float magnitude)
    {
        playerReference.rb.AddForce(-direction.normalized * magnitude, ForceMode.Impulse);
        
    }



}

