using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Inventory_Manager : Actor
{
    NewPC player;

    public int selectedWeapon = 1;
    enum currentWeapon {
        main = 1,
        alt = 2,
    }

    

    [Header("Weapons")]
    public GameObject startingWeaponPrefab;
    public GameObject altWeaponPrefab;



    //[HideInInspector]
    public GameObject powerUpWeapon;

    private void Awake()
    {
        player = gameObject.GetComponent<NewPC>() ?? gameObject.AddComponent<NewPC>();
    }

    private void Start()
    {
        spawnWeapons();
        
    }

    private void Update()
    {
        setSelectedWeapon();
    }

    void spawnWeapons()
    {
        spawnWeapon(startingWeaponPrefab);
        spawnWeapon(altWeaponPrefab);
    }

    void spawnWeapon(GameObject weapon)
    {

        if (IsServer)
        {
            networkSpawnWeapon(weapon);
        }
        else
        {
            InvokeServerRpc(networkSpawnWeapon, weapon);
        }
    }

    void setSelectedWeapon()
    {
        startingWeaponPrefab.SetActive(false);
        altWeaponPrefab.SetActive(false);

        if (powerUpWeapon)
        {
            powerUpWeapon.SetActive(true);
        }
        else 
        {
            if (selectedWeapon == 1)
            {
                if (startingWeaponPrefab) { startingWeaponPrefab.SetActive(true); }
            }
            else if (selectedWeapon == 2)
            {
                if (altWeaponPrefab) { altWeaponPrefab.SetActive(true); }
            }
        }
    }

    [ServerRPC(RequireOwnership = false)]
    void networkSpawnWeapon(GameObject weapon)
    {
        weapon = NetSpawn(weapon, Vector3.zero, Quaternion.identity);

        weapon.transform.parent = player.eyes;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.rotation = Quaternion.identity;
    }

    public void GiveExtraWeapon(GameObject item, float lifeTime = 5)
    {
        powerUpWeapon = item;
        spawnWeapon(powerUpWeapon);

    }


}
