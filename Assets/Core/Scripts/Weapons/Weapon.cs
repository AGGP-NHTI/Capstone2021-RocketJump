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
        setUIObj();
        setAmmo();
        StartCoroutine(waitForPawn());
    }

    private void OnEnable()
    {
        StartCoroutine(waitForPawn());
    }

    IEnumerator waitForPawn()
    {
        yield return new WaitUntil(() => (playerPawn != null));
        if (playerPawn.IsLocal())
        {
            setAmmo();
        }
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

        if (playerPawn)
        {
            //playerPawn.AudioManager.PlayAudio(playerPawn.AudioManager.YEET.name, transform.position);
        }

        currentClip--;
        setAmmo();
        return true;
    }



    [ServerRPC(RequireOwnership = false)]
    public void spawnNetworkedProjectile(Vector3 location, Quaternion dir)
    {
        Debug.Log("REQUEST PROJECTILE SPAWN");
        InvokeClientRpcOnEveryone(spawnClientProjectile, location, dir);
    }
    int projectileCounter = 0;
    [ClientRPC]
    public void spawnClientProjectile(Vector3 location, Quaternion dir)
    {
        projectileCounter++;
        Debug.Log("CLIENT PROJECTILE SPAWN-------------------" + projectileCounter);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bullet = Instantiate(projectilePrefab, location, dir);
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

    void setAmmo()
    {
        UIMan.setAmmo(currentClip,clipSize);
    }
    void setUIObj()
    {
        if (playerPawn.UI)
        {
            UI = playerPawn.UI;
            if (UI) { UIMan = UI.GetComponent<UIManager>(); }
        }
    }

}

