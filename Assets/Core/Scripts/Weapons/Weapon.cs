using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MLAPI;
using MLAPI.Messaging;

public class Weapon : Pawn
{
    protected GameObject UI;
    protected UIManager UIMan;
    //protected Player_Movement_Controller playerReference;
    protected Ammo_UI_Script AmmoReference;

    protected bool isCooling = false;

    [Header("Dependencies")] 
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    [Range(0.5f, 10f)]
    public float    reloadSpeed;
    [Range(1,50)]
    public int      bulletsPerShot;
    [Range(0f,10f)]
    public float    bulletSpread;
    [Range(1f,1000f)]
    public int      currentClip;
    [Range(1f, 1000f)]
    public int      clipSize;

    [Header("Info")]
    public bool isRapidFire = false;




    protected virtual void Start()
    {
        //Debug.Log("Start--");
        //setPlayerReference();
        
        //setUIObj();
        //setAmmoReference();

        //if (controller.IsLocalPlayer)
        //{
        //    currentClip = clipSize;
        //}
    }

    private void OnEnable()
    {
        //if (controller.IsLocalPlayer)
        //{
        //    Debug.Log("SETTING UI-----");

        //    AmmoReference.SetMagazine(currentClip);
        //    AmmoReference.SetReserve(clipSize);
        //}
    }
    protected virtual void Update()
    {
        if (isRapidFire)
        {
            if (Input.GetMouseButton(0)) { Fire(); }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) { Fire(); }
        }
    }

    public virtual void Fire() 
    {
        if (isCooling) { return; }

        if (IsServer)
        {
            spawnNetworkedProjectile();
        }
        else 
        {
            InvokeServerRpc(spawnNetworkedProjectile);
        }

        if (controller.IsLocalPlayer)
        {
            currentClip--;
            AmmoReference.SetMagazine(currentClip);
        }
    }

    public virtual void AltFire() 
    { 
        
    }

    [ServerRPC(RequireOwnership = false)]
    public void spawnNetworkedProjectile()
    {
        //CANNOT GET HERE
       // Debug.Log($"I AM SHOOTING A {gameObject.name} AM Server {IsServer}");

        for (int i = 0; i < bulletsPerShot; i++)
        {
            //Debug.Log("---------------Position: " + projectileSpawn.position);
            GameObject bullet = NetSpawn(projectilePrefab,
                                projectileSpawn.position,
                                Quaternion.LookRotation(BulletSpread(projectileSpawn.forward))
                                );

            
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile)
            {
                //projectile.setPlayer(playerReference);
            }
        }
    }

    
    
    

    

    protected IEnumerator waitFireRateTimer(float input)
    {
        float delay = 1 / input;
        isCooling = true;
        yield return new WaitForSeconds(delay);
        isCooling = false;
    }

    protected Vector3 BulletSpread(Vector3 input)
    {
        input += Random.insideUnitSphere * bulletSpread/100;
        return input.normalized;
    }



    protected void KnockBack(Vector3 direction, float magnitude)
    {
        //playerReference.AddForce(direction.normalized * magnitude);    
    }


    void setPlayerReference()
    {
        //Player_Movement_Controller player = transform.root.GetComponent<Player_Movement_Controller>();
        //if (player)
        //{
        //    //if (!controller.IsLocalPlayer) { Destroy(this); }
        //    playerReference = player;
        //}
        
    }
    void setAmmoReference()
    {
        Ammo_UI_Script ammo = UI.transform.GetComponentInChildren<Ammo_UI_Script>();
        if (ammo)
        {
            AmmoReference = ammo;
        }
    }
    void setUIObj()
    {
        //UI = playerReference.UI;
        //if (UI) { UIMan = UI.GetComponent<UIManager>(); }
    }

}

