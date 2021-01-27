using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Weapon : Actor
{

    [Header("Dependencies")]
    public TextMeshProUGUI ammo;
    public TextMeshProUGUI maxAmmo;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    [Header("Stats")]
    public float reloadSpeed;
    public float fireRate;
    public float bulletSpread;
    public int currentClip;
    public int clipSize;
    
    [Header("Info")]
    public bool isReloading = false;
    public bool isCooling = false;

    GameObject weaponModel;

    public abstract void Fire();
    public abstract void AltFire();

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
        ammo.text = currentClip.ToString();
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
    
    Vector3 calculateBulletSpread(float amnt)
    { 
    
    }
}

