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

        if (playerPawn.controller) 
        {
            spawnWeapons();
        }
    }

    private void Update()
    {
        

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
            networkSpawnPrimeWeapon(ApplicationGlobals.GetWeaponIndex(weaponPrefab));
        }
        else
        {
            InvokeServerRpc(networkSpawnPrimeWeapon, ApplicationGlobals.GetWeaponIndex(weaponPrefab));
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

    [ClientRPC]
    void parentWeapon(ulong netID, ulong playerNetID)
    {
        NetworkedObject netWeapon = GetNetworkedObject(netID);
        NetworkedObject netPlayer = GetNetworkedObject(playerNetID);

        if (netWeapon && netPlayer)
        {
            netWeapon.transform.parent = netPlayer.transform;
            //netWeapon.transform.localPosition = Vector3.zero;
        }
    }

    [ServerRPC(RequireOwnership = false)]
    void networkSpawnPrimeWeapon(int weaponIndex, ulong requestID)
    {
        
        primeWeapon = playerPawn.NetSpawn(ApplicationGlobals.GetWeaponPrefab(weaponIndex), Vector3.zero, Quaternion.identity).GetComponent<Weapon>();

        NetworkedObject clientObj = GetNetworkedObject(requestID);
        if (clientObj && clientObj.TryGetComponent(out Player_Pawn clientPawn))
        {
            primeWeapon.transform.parent = clientPawn.eyes;
        }

        if (primeWeapon.TryGetComponent(out NetworkedObject netObj))
        {
            netObj.ChangeOwnership(OwnerClientId);
        }

        InvokeClientRpcOnEveryone(parentWeapon, netObj.NetworkId, playerPawn.NetworkId);
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

        GameObject weaponPawn = Instantiate(weapon, parent);
        NetworkedObject netObj = weaponPawn.GetComponent<NetworkedObject>();
        netObj.SpawnWithOwnership(OwnerClientId);

        //playerPawn.controller.PossessPawn(weaponPawn, netObj.OwnerClientId, netObj.NetworkId);

    }
}
