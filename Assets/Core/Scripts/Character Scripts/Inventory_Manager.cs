using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Inventory_Manager : NetworkedBehaviour
{
    public Player_Pawn playerPawn;

    protected GameObject UI;
    protected UIManager UIMan;
    protected Ammo_UI_Script AmmoReference;




    bool isHoldingPrime = true;    

    [Header("Weapons")]
    public GameObject startingWeaponPrefab;
    public GameObject altWeaponPrefab;
    public GameObject testPowerUpWeaponPrefab;

    public Weapon currentWeapon = null;

    Weapon primeWeapon = null;
    Weapon altWeapon = null;

    //[HideInInspector]
    public Weapon powerUpWeapon;

    private void Awake()
    {
        
    }

    private void Start()
    {
 
        
    }

    private void Update()
    {
        if (playerPawn.controller)
        {
           spawnWeapons();
        }

        checkPowerUpWeapon();
        setSelectedWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            spawnPowerWeapon(testPowerUpWeaponPrefab);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isHoldingPrime = !isHoldingPrime;
        }

        if (playerPawn.IsLocal() && currentWeapon)
        {
            FireInput();
        }
    }

    void FireInput()
    {
        if (currentWeapon.isRapidFire)
        {
            if (Input.GetMouseButton(0))
            {
                Fire();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                Fire();
            }
        }
    }
    void Fire()
    {
        if (currentWeapon.Fire())
        {
            if (currentWeapon is Guns gun)
            {
                //playerPawn.movementControl.ApplyKnockback(gun.knockBackForce);
            }
        }
    }
    void spawnWeapons()
    {
        if(!primeWeapon)
            spawnPrimeWeapon(startingWeaponPrefab);

    }

    void spawnPrimeWeapon(GameObject weaponPrefab)
    {
        

        if (IsServer)
        {
            networkSpawnPrimeWeapon(weaponPrefab);
        }
        else
        {
            InvokeServerRpc(networkSpawnPrimeWeapon, weaponPrefab);
        }

        currentWeapon = primeWeapon;

    }
    void spawnAltWeapon(GameObject weaponPrefab)
    {

        if (IsServer)
        {
            networkSpawnAltWeapon(weaponPrefab);
        }
        else
        {
            InvokeServerRpc(networkSpawnAltWeapon, weaponPrefab);
        }
    }
    void spawnPowerWeapon(GameObject weaponPrefab)
    {

        if (IsServer)
        {
            networkSpawnPowerWeapon(weaponPrefab);
        }
        else
        {
            InvokeServerRpc(networkSpawnPowerWeapon, weaponPrefab);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    void networkSpawnPrimeWeapon(GameObject weaponPrefab)
    {
        
        primeWeapon = playerPawn.NetSpawn(weaponPrefab, Vector3.zero, Quaternion.identity).GetComponent<Weapon>();

        primeWeapon.transform.parent = playerPawn.eyes;
        primeWeapon.transform.localPosition = Vector3.zero;
        primeWeapon.transform.rotation = Quaternion.identity;

        playerPawn.controller.PossessPawn(primeWeapon.gameObject);
        
    }
    [ServerRPC(RequireOwnership = false)]
    void networkSpawnAltWeapon(GameObject weaponPrefab)
    {
        altWeapon = playerPawn.NetSpawn(weaponPrefab, Vector3.zero, Quaternion.identity).GetComponent<Weapon>();

        altWeapon.transform.parent = playerPawn.eyes;
        altWeapon.transform.localPosition = Vector3.zero;
        altWeapon.transform.rotation = Quaternion.identity;

        playerPawn.controller.PossessPawn(altWeapon.gameObject);
    }
    [ServerRPC(RequireOwnership = false)]
    void networkSpawnPowerWeapon(GameObject weaponPrefab)
    {
        powerUpWeapon = playerPawn.NetSpawn(weaponPrefab, Vector3.zero, Quaternion.identity).GetComponent<Weapon>();

        powerUpWeapon.transform.parent = playerPawn.eyes;
        powerUpWeapon.transform.localPosition = Vector3.zero;
        powerUpWeapon.transform.rotation = Quaternion.identity;

        playerPawn.controller.PossessPawn(powerUpWeapon.gameObject);

    }


    void checkPowerUpWeapon()
    {
        if (powerUpWeapon && powerUpWeapon.transform.parent != playerPawn.eyes)
        {
            powerUpWeapon = null; 
        }
    }
    void setSelectedWeapon()
    {

        if (powerUpWeapon && primeWeapon && altWeapon)
        {
            currentWeapon = powerUpWeapon;

            powerUpWeapon.gameObject.SetActive(true);
            primeWeapon.gameObject.SetActive(false);
            altWeapon.gameObject.SetActive(false);
        }
        else
        {
            if (isHoldingPrime && primeWeapon && altWeapon)
            {
                currentWeapon = primeWeapon;

                primeWeapon.gameObject.SetActive(true);
                altWeapon.gameObject.SetActive(false);
            }
            else if (primeWeapon && altWeapon)
            {
                currentWeapon = altWeapon;

                altWeapon.gameObject.SetActive(true);
                primeWeapon.gameObject.SetActive(false);
            }
        }

    }

    public void GiveExtraWeapon(GameObject item)
    {
        spawnPowerWeapon(item);
    }

    void setAmmoReference()
    {
        Ammo_UI_Script ammo = UI.transform.GetComponentInChildren<Ammo_UI_Script>();
        if (ammo)
        {
            AmmoReference = ammo;
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnPlayerPrimeWeapon(GameObject weapon, Transform parent)
    {
        Vector3 location = Vector3.right * NetworkId;
        location += Vector3.up * 10;
        GameObject weaponPawn = Instantiate(weapon, parent);
        NetworkedObject netObj = weaponPawn.GetComponent<NetworkedObject>();
        //netObj.Spawn();


        netObj.SpawnWithOwnership(OwnerClientId);


        playerPawn.controller.PossessPawn(weaponPawn, netObj.OwnerClientId, netObj.NetworkId);

    }
}
