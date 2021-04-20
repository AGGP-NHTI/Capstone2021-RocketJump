using MLAPI.Messaging;
using System.Collections;
using UnityEngine;

public class Weapon : Actor
{
    public Player_Pawn playerPawn;
    public int globalIndex = -1;
    protected GameObject UI;
    protected UIManager UIMan;
    protected Ammo_UI_Script AmmoReference;

    protected bool isCooling = false;

    [Header("Dependencies")] 
    public GameObject projectilePrefab;
    
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
    public Transform projectileSpawn;



    protected virtual void Start()
    {
        //Debug.Log("Start--");
        setPlayerReference();
        
        setUIObj();
        setAmmoReference();

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
        if (playerPawn)
        {
            Color col = Color.red;
            if(playerPawn.IsLocal())
            {
                col = Color.green;
            }
            Debug.DrawRay(projectileSpawn.position, projectileSpawn.forward * 100, Color.red);
        }
    }

    public virtual bool Fire() 
    {
        Debug.Log("FIREING FROM WEAPONS");
        if (isCooling) { return false; }

        Quaternion fireDirection = Quaternion.LookRotation(BulletSpread(projectileSpawn.forward));
        Vector3 position = projectileSpawn.position;
        InvokeServerRpc(spawnNetworkedProjectile,position, fireDirection);

        


        
        currentClip--;
        return true;
    }



    [ServerRPC(RequireOwnership = false)]
    public void spawnNetworkedProjectile(Vector3 location, Quaternion dir)
    {
        InvokeClientRpcOnEveryone(spawnClientProjectile, location, dir);
    }

    [ClientRPC]
    public void spawnClientProjectile(Vector3 location, Quaternion dir)
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bullet = NetSpawn(projectilePrefab,
                                location,
                                dir
                                );
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
        input += Random.insideUnitSphere * bulletSpread / 100;
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
        //Ammo_UI_Script ammo = UI.transform.GetComponentInChildren<Ammo_UI_Script>();
        //if (ammo)
        //{
        //    AmmoReference = ammo;
        //}
    }
    void setUIObj()
    {
        //UI = playerReference.UI;
        //if (UI) { UIMan = UI.GetComponent<UIManager>(); }
    }

}

