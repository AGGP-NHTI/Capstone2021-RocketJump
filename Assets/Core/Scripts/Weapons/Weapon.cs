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
        if (IsOwner)
        {
            StartCoroutine(setup());
        }
    }

    IEnumerator setup()
    {
        yield return new WaitUntil(() => playerPawn != null);
        setUIObj();
        setAmmo();
        StartCoroutine(waitForPawn());
    }

    private void OnEnable()
    {
        if (IsOwner)
        {
            StartCoroutine(waitForPawn());
            resetRotation();
        }
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
        if (isCooling) { return false; }

        for(int i = 0; i < bulletsPerShot; i++)
        { 
        Quaternion fireDirection = Quaternion.LookRotation(BulletSpread(projectileSpawn.forward));
        Vector3 position = projectileSpawn.position;

        InvokeServerRpc(spawnNetworkedProjectile,position, fireDirection);
        }

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
        InvokeClientRpcOnEveryone(spawnClientProjectile, location, dir);
    }
    int projectileCounter = 0;
    [ClientRPC]
    public void spawnClientProjectile(Vector3 location, Quaternion dir)
    {
        GameObject bullet = Instantiate(projectilePrefab, location, dir);
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
        Debug.Log("initial Input: " + input);
        input += Random.insideUnitSphere * bulletSpread/50;
        Debug.Log("after Input: " + input);
        return input.normalized;
    }

    public void setAmmo()
    {
        if (UIMan) { UIMan.setAmmo(currentClip, clipSize); }
    }
    public void resetRotation()
    {
        transform.localRotation = Quaternion.identity;
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

