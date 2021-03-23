using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Inventory_Manager : Pawn
{
    Player_Movement_Controller player = null;






    bool isHoldingPrime = true;    

    [Header("Weapons")]
    public GameObject startingWeaponPrefab;
    public GameObject altWeaponPrefab;
    public GameObject testPowerUpWeaponPrefab;

    GameObject primeWeapon = null;
    GameObject altWeapon = null;

    //[HideInInspector]
    public GameObject powerUpWeapon;

    private void Awake()
    {
        player = gameObject.GetComponent<Player_Movement_Controller>() ?? gameObject.AddComponent<Player_Movement_Controller>();
    }

    private void Start()
    {
        
        
    }

    private void Update()
    {
        if (player.controller)
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
    }

    void spawnWeapons()
    {
        if(!primeWeapon)
            spawnPrimeWeapon(startingWeaponPrefab);
        //spawnAltWeapon(altWeaponPrefab);
    }

    void spawnPrimeWeapon(GameObject weaponPrefab)
    {
        primeWeapon = Instantiate(weaponPrefab, player.eyes);

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
        
        primeWeapon = NetSpawn(weaponPrefab, Vector3.zero, Quaternion.identity);

        primeWeapon.transform.parent = player.eyes;
        primeWeapon.transform.localPosition = Vector3.zero;
        primeWeapon.transform.rotation = Quaternion.identity;

        
    }
    [ServerRPC(RequireOwnership = false)]
    void networkSpawnAltWeapon(GameObject weaponPrefab)
    {
        altWeapon = NetSpawn(weaponPrefab, Vector3.zero, Quaternion.identity);

        altWeapon.transform.parent = player.eyes;
        altWeapon.transform.localPosition = Vector3.zero;
        altWeapon.transform.rotation = Quaternion.identity;
    }
    [ServerRPC(RequireOwnership = false)]
    void networkSpawnPowerWeapon(GameObject weaponPrefab)
    {
        powerUpWeapon = NetSpawn(weaponPrefab, Vector3.zero, Quaternion.identity);

        powerUpWeapon.transform.parent = player.eyes;
        powerUpWeapon.transform.localPosition = Vector3.zero;
        powerUpWeapon.transform.rotation = Quaternion.identity;
    }


    void checkPowerUpWeapon()
    {
        if (powerUpWeapon && powerUpWeapon.transform.parent != player.eyes)
        {
            powerUpWeapon = null; 
        }
    }
    void setSelectedWeapon()
    {

        if (powerUpWeapon && primeWeapon && altWeapon)
        {
            powerUpWeapon.SetActive(true);
            primeWeapon.SetActive(false);
            altWeapon.SetActive(false);
        }
        else
        {
            if (isHoldingPrime && primeWeapon && altWeapon)
            {
                primeWeapon.SetActive(true);
                altWeapon.SetActive(false);
            }
            else if (primeWeapon && altWeapon)
            {
                altWeapon.SetActive(true);
                primeWeapon.SetActive(false);
            }
        }

    }

    public void GiveExtraWeapon(GameObject item)
    {
        spawnPowerWeapon(item);
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


        player.controller.PossessPawn(weaponPawn, netObj.OwnerClientId, netObj.NetworkId);

    }
}
