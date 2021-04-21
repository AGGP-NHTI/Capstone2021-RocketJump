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
        
        if(IsLocalPlayer)
        {
            playerName = PlayerInformation.playerScreenName;
        }

        if (PNCEnabled)
        {
            PNC.initPNC(this);
        }
        PNC.enabled = PNCEnabled;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2) && IsLocalPlayer)
        {
            SpawnPlayerPawn();
        }

        if (!IsLocalPlayer)
        { return; }

        if (!ControlledPawn)
        { return; }

 
    }

    public void SpawnPlayerPawn()
    {
        if (IsLocalPlayer)
        {
            Debug.Log("Is owner, Spawn Player Pawn");
            if(PNCEnabled)
            {
                InvokeServerRpc(Server_SpawnPlayerPawn_Race, OwnerClientId);
            }
            else
            {
                InvokeServerRpc(Server_SpawnPlayerPawn, OwnerClientId);
            }
        }
    }

    //NETWORK FUNCTIONS

    public void updateNodePosition(PositionNodeScript node)
    {
        if (IsHost)
        {
            PNC.positionManager.updatePlayerPosition(gameObject, node.nodeNumber);
        }
        else if (IsClient)
        {
            //clientUpdateNodePosition(node, gameObject);
            InvokeServerRpc(PNC.serverUpdateNodePosition, node.nodeNumber, gameObject);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnPlayerPawn(ulong clientID)
    {
        Vector3 location = Vector3.right * NetworkId;
        location += Vector3.up * 10;
        GameObject gobj = Instantiate(defaultPawn, location, Quaternion.identity);

        NetworkedObject netObj = gobj.GetComponent<NetworkedObject>();

        netObj.SpawnWithOwnership(clientID);
        PossessPawn(gobj, netObj.OwnerClientId, netObj.NetworkId); 
    }


    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnPlayerPawn_Race(ulong clientID)
    {
        if(!spawnManager)
        {
            spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnPointManager>();
        }

        Debug.Log("Spawning Player...");

        Vector3 location = spawnManager.getSpawn();
        GameObject gobj = Instantiate(defaultPawn, location, Quaternion.identity);

        NetworkedObject netObj = gobj.GetComponent<NetworkedObject>();

        netObj.SpawnWithOwnership(clientID);
        PossessPawn(gobj, netObj.OwnerClientId, netObj.NetworkId);
    }
}
