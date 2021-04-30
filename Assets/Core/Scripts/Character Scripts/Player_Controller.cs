using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player_Controller : Controller
{

    public SpawnPointManager spawnManager;

    [Header("Network Settings")]
    public bool PNCEnabled = true;
    public PlayerNetworkCenter PNC;

    [Header("Public Player Info")]
    public string playerName;
    public int playerCharacter;
    public bool PlayerSpawned = false;

    [Header("Characters")]
    public GameObject defaultPawn;
    public GameObject sashaPawn;
    public GameObject chappiePawn;
    public GameObject dictatorPawn;

    public GameObject GetSpawnPrefab(int prefabIndex)
    {
		switch (prefabIndex)
		{
			case 0:
				Debug.Log("spectator spawned");
				return defaultPawn;
			case 1:
				Debug.Log("dictator spawned");
				return dictatorPawn;
			case 2:
				Debug.Log("chappie spawned");
				return chappiePawn;
			case 3:
				Debug.Log("sasha spawned");
				return sashaPawn;
			default:
				return null;
		}
	}

    private void Start()
    {

        if (IsLocalPlayer)
        {
            gameObject.name = gameObject.name + "_Local_" + OwnerClientId;
            playerName = PlayerInformation.playerScreenName;
            playerCharacter = PlayerInformation.playerCharacter;
            plrCntrl = this;
        }
        else
        {
            gameObject.name = gameObject.name + "_Remote_" + OwnerClientId;
        }

        if (PNC && PNCEnabled)
        {
            PNC.initPNC(this);
        }
        PNC.isEnabled = PNCEnabled;

    }

    private void Update()
    {
        if (!PlayerSpawned)
        {
            if (Input.GetKeyDown(KeyCode.F2) && IsLocalPlayer)
            {
                SpawnPlayerPawn(1);
                PlayerSpawned = true;
            }
            if (Input.GetKeyDown(KeyCode.F3) && IsLocalPlayer)
            {
                SpawnPlayerPawn(2);
                PlayerSpawned = true;
            }
            if (Input.GetKeyDown(KeyCode.F4) && IsLocalPlayer)
            {
                SpawnPlayerPawn(3);
                PlayerSpawned = true;
            }
        }

        if (!IsLocalPlayer)
        { return; }

        if (!ControlledPawn)
        { return; }

        if(!IsLocalPlayer)
        {
            PNC.owner = PlayerInformation.controller;
        }

    }

    public void SpawnPlayerPawn(int whichPlayer)
    {
        if (IsLocalPlayer)
        {
            Debug.Log("Is owner, Spawn Player Pawn");
            if(PNCEnabled)
            {
                InvokeServerRpc(Server_SpawnPlayerPawn_Race, whichPlayer ,OwnerClientId);
            }
            else
            {
                InvokeServerRpc(Server_SpawnPlayerPawn, whichPlayer ,OwnerClientId);
            }
        }
    }

    //NETWORK FUNCTIONS

    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnPlayerPawn(int whichPawn, ulong clientID)
    {
        Vector3 location = Vector3.right * NetworkId;
        location += Vector3.up * 10;
        GameObject gobj = Instantiate(GetSpawnPrefab(whichPawn), location, Quaternion.identity);

        NetworkedObject netObj = gobj.GetComponent<NetworkedObject>();

        netObj.SpawnWithOwnership(clientID);
        PossessPawn(gobj, netObj.OwnerClientId, netObj.NetworkId); 
    }


    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnPlayerPawn_Race(int whichPawn, ulong clientID)
    {
        if(!spawnManager)
        {
            spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnPointManager>();
        }

        Debug.Log("Spawning Player...");

        Vector3 location = spawnManager.getSpawn();
        GameObject gobj = Instantiate(GetSpawnPrefab(whichPawn), location, Quaternion.identity);

        NetworkedObject netObj = gobj.GetComponent<NetworkedObject>();

        netObj.SpawnWithOwnership(clientID);
        PossessPawn(gobj, netObj.OwnerClientId, netObj.NetworkId);
    }
}
