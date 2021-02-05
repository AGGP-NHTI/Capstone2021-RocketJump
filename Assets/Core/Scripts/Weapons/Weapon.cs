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
    private Ammo_UI_Script AmmoReference;

    private bool isReloading = false;
    private bool isCooling = false;

    [Header("Dependencies")] 
    //public GameObject weaponModel;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    [Header("Stats")]
    public float    reloadSpeed;
    public float    fireRate;
    [Range(0f,100f)]
    public float    bulletSpread;
    public float    knockBackForce;
    public int      currentClip;
    public int      clipSize;
    
    
    [Header("Info")]
    public bool isRapidFire = false;

    protected KeyCode reloadBinding = KeyCode.R;

    protected virtual void Start()
    {
        Debug.Log("Start--");
        setPlayerReference();
        setUIObj();
        setAmmoReference();


        currentClip = clipSize;
        AmmoReference.SetMagazine(currentClip);
        AmmoReference.SetReserve(clipSize);
    }
    private void Update()
    {
        if (isRapidFire)
        {
            if (Input.GetMouseButton(0)) { Fire(); }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) { Fire(); }
        }
        if (Input.GetKeyDown(reloadBinding))
        {
            reload();
        }
    }

    public virtual void Fire() 
    {
        if (isReloading || clipEmpty() || isCooling) { return; }

        GameObject go = Instantiate(projectilePrefab,
                                    projectileSpawn.position,
                                    Quaternion.LookRotation(BulletSpread(projectileSpawn.forward))
                                    );

        currentClip--;
        AmmoReference.SetMagazine(currentClip);
        KnockBack(-transform.parent.forward, knockBackForce);
        waitForFireRate();
       
    }
    public virtual void AltFire() 
    { 
        
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

    protected Vector3 BulletSpread(Vector3 input)
    {
        input += Random.insideUnitSphere * bulletSpread/100;

        return input.normalized;
    }
    void KnockBack(Vector3 direction, float magnitude)
    {
        playerReference.rb.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        
    }



}

