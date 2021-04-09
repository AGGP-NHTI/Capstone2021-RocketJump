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


    [Header("Stuff")]
    public List<GameObject> weaponPrefabs = new List<GameObject>();
    public List<Weapon> weapons = new List<Weapon>();
    public int currentWeaponIndex = -1;
    Weapon currentWeapon => weapons[currentWeaponIndex];
    

    private void Awake()
    {
        
    }

    private void Start()
    {
        //HAVE THE SERVER COMUNICATETHE WEAPON INDEX OF THE CLIENT
        if (playerPawn.controller) 
        {
           //spawnWeapons();
        }
    }

    private void Update()
    {
        if (!playerPawn.IsLocal())
        {
            
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            spawnWeapons();
        }
        Vector2 mouseScroll = Input.mouseScrollDelta;
        if (mouseScroll.y > 0)
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= weapons.Count)
            {
                currentWeaponIndex = 0;
            }
            InvokeServerRpc(setCurrentWeaponIndexOnServer, NetworkId, currentWeaponIndex);
        }
        else if( mouseScroll.y < 0)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weapons.Count - 1;
            }
            InvokeServerRpc(setCurrentWeaponIndexOnServer, NetworkId, currentWeaponIndex);
        }
       

        if (playerPawn.IsLocal() && currentWeaponIndex != -1)
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
        for (int i = 0; i < weaponPrefabs.Count; i++)
        {
            Debug.Log($"Spawn weapon with index {i}.");

            InvokeServerRpc(networkSpawnWeapon, ApplicationGlobals.GetWeaponIndex(weaponPrefabs[i]), OwnerClientId);
        }
    }

    

    [ClientRPC]
    void setWeaponParent(ulong weaponNetID, ulong clientID)
    {
        NetworkedObject netWeapon = GetNetworkedObject(weaponNetID);
        NetworkedObject netPlayer = GetNetworkedObject(clientID);

        if (netWeapon && netPlayer && netPlayer.gameObject.TryGetComponent(out Player_Pawn clientPawn))
        {
            netWeapon.transform.parent = clientPawn.eyes;
            netWeapon.transform.localPosition = Vector3.zero;
            netWeapon.transform.localRotation = Quaternion.identity;

            if (netWeapon.gameObject.TryGetComponent(out Weapon clientWeapon))
            {
                weapons.Add(clientWeapon);
                if (currentWeaponIndex == -1)
                {
                    currentWeaponIndex = 0;
                    

                }
                setSelectedWeapon();
            }
            else
            {
                Debug.LogWarning($"{netWeapon.name} does not have client weapon component.");
            }
        }
        else 
        {
            Debug.LogWarning($"net weapon: {netWeapon != null}, netplayer {netPlayer != null}, clientPawn: { netPlayer.gameObject.GetComponent<Player_Pawn>() != null} does not exist");
        }
    }

    [ServerRPC(RequireOwnership = false)]
    void networkSpawnWeapon(int weaponIndex, ulong clientID)
    {
        
        Weapon weapon = playerPawn.NetSpawn(ApplicationGlobals.GetWeaponPrefab(weaponIndex), Vector3.zero, Quaternion.identity).GetComponent<Weapon>();

        NetworkedObject clientObj = GetNetworkedObject(clientID);
        if (clientObj && clientObj.TryGetComponent(out Player_Pawn clientPawn))
        {
            weapon.transform.parent = clientPawn.eyes;
        }

        if (weapon.TryGetComponent(out NetworkedObject netObj))
        {
            netObj.ChangeOwnership(OwnerClientId);
        }

        InvokeClientRpcOnEveryone(setWeaponParent, netObj.NetworkId, playerPawn.NetworkId);
    }

    [ServerRPC]
    void setCurrentWeaponIndexOnServer(ulong pawnID, int index)
    {

        NetworkedObject netPlayer = GetNetworkedObject(pawnID);
        if (netPlayer && netPlayer.TryGetComponent(out Player_Pawn clientPawn))
        {
            clientPawn.inventoryMan.currentWeaponIndex = index;
            setSelectedWeapon();
        }
        InvokeClientRpcOnEveryone(setCurrentWeaponIndexOnClients,pawnID,index);
    }

    [ClientRPC]
    void setCurrentWeaponIndexOnClients(ulong pawnID, int index)
    {
        NetworkedObject netPlayer = GetNetworkedObject(pawnID);
        if (netPlayer && netPlayer.TryGetComponent(out Player_Pawn clientPawn))
        {
            clientPawn.inventoryMan.currentWeaponIndex = index;
            setSelectedWeapon();
        }
        else 
        {
            Debug.LogWarning($"Net Player: {netPlayer != null}");
            Debug.LogWarning($"Pawn: {netPlayer.GetComponent<Player_Pawn>() != null}");
        }
    }

    void setSelectedWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (currentWeaponIndex == i)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }



    void setAmmoReference()
    {
        Ammo_UI_Script ammo = UI.transform.GetComponentInChildren<Ammo_UI_Script>();
        if (ammo)
        {
            AmmoReference = ammo;
        }
    }


}
